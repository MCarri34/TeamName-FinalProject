using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonImpact : MonoBehaviour
{
    [Header("References")]
    public Transform motionRoot;
    public Animator playerAnimator;
    public Camera mainCamera;
    public Canvas menuCanvas;
    public RectTransform startButton;

    [Header("Animated by StartAttack Clip")]
    [Range(0f, 1f)]
    public float attackProgress = 0f;

    [Header("Attack Windup (previous clip pose)")]
    [Min(0f)] public float windupDuration = 0.18f;
    public float windupRise = 0.8f;
    public float windupRotation = 30f;
    public float windupScaleMultiplier = 0.75f / 0.85f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip impactSound;

    [Header("Scene")]
    public string gameplaySceneName;


    [Header("Scene Transition")]
    public GameObject blackoutCanvas;
    public GlassBreakEvent glassBreakEvent;

    [Range(0.5f, 1f)]
    public float blackoutDuration = 0.8f;

    private bool isLoading;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalScale;
    private float poseProgress;
    private bool impacted;

    private bool started = false;

    public void OnStartClicked()
    {
        if (started) return;

        if (motionRoot == null ||
            playerAnimator == null ||
            mainCamera == null ||
            menuCanvas == null ||
            startButton == null)
        {
            Debug.LogError("Missing StartButtonImpact references");
            return;
        }

        // Make sure UI layout is up to date.
        Canvas.ForceUpdateCanvases();

        // Get the actual UI button center.
        Vector3 uiCenter = startButton.TransformPoint(
            startButton.rect.center
        );

        Camera uiCamera =
            menuCanvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : menuCanvas.worldCamera;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, uiCenter);

        // Convert screen position to Player's world plane.
        float depth = Vector3.Dot(
            transform.position - mainCamera.transform.position,
            mainCamera.transform.forward
        );

        Vector3 worldTarget = mainCamera.ScreenToWorldPoint(
            new Vector3(screenPoint.x, screenPoint.y, depth)
        );

        worldTarget.z = transform.position.z;

        // Capture the idle pose once; never accumulate offsets frame by frame.
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        initialLocalScale = transform.localScale;
        Vector3 localRise = Vector3.up * windupRise;
        Vector3 windupWorldOffset = 
            transform.parent != null
            ? transform.parent.TransformVector(localRise)
            : localRise;
        poseProgress = 0f;
        impacted = false;

        // Move the parent by the required displacement.
        startPosition = motionRoot.position;
        targetPosition = startPosition + (worldTarget - transform.position) - windupWorldOffset;

        started = true;
        attackProgress = 0f;

        playerAnimator.SetTrigger("StartAttack");
    }

    private void LateUpdate()
    {
        if (!started) return;

        // Apply after Animator evaluation, including its Write Defaults behavior.
        // Keep the final pose while waiting for the scene change.
        if (impacted)
        {
            poseProgress = 1f;
        }
        else
        {
            AnimatorStateInfo state = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("StartAttack"))
            {
                float elapsed = Mathf.Max(0f, state.normalizedTime * state.length);
                poseProgress = 
                    windupDuration > 0f
                    ? Mathf.Clamp01(elapsed / windupDuration)
                    : 1f;
            }
        }

        float eased = Mathf.SmoothStep(0f, 1f, poseProgress);
        transform.localPosition = initialLocalPosition + Vector3.up * (windupRise * eased);
        transform.localRotation = initialLocalRotation * Quaternion.Euler(0f, 0f, windupRotation * eased);


        float scale = Mathf.Lerp(1f, windupScaleMultiplier, eased);
        transform.localScale = new Vector3(
            initialLocalScale.x * scale,
            initialLocalScale.y * scale,
            initialLocalScale.z);

        motionRoot.position = Vector3.Lerp(
            startPosition,
            targetPosition,
            impacted ? 1f : Mathf.Clamp01(attackProgress)
        );
    }

    // Animation Event at the impact frame.
    public void Impact()
    {
        if (!started || impacted) return;

        motionRoot.position = targetPosition;
        impacted = true;

        if (glassBreakEvent != null) glassBreakEvent.StopFallingSound();

        if (audioSource != null && impactSound != null) audioSource.PlayOneShot(impactSound);
    }

    // Animation Event shortly after impact.
    public void LoadGame()
    {
        if (!started || isLoading) return;

        if (string.IsNullOrEmpty(gameplaySceneName))
        {
            Debug.LogError("Gameplay scene name is empty.", this);
            return;
        }

        isLoading = true;

        Impact();

        StartCoroutine(LoadGameAfterBlackout());
    }

    private IEnumerator LoadGameAfterBlackout()
    {
        if (blackoutCanvas != null) blackoutCanvas.SetActive(true);

        yield return new WaitForSecondsRealtime(blackoutDuration);

        SceneManager.LoadScene(gameplaySceneName);
    }
}
