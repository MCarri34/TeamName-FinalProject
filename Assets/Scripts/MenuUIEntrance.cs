
using System.Collections;
using UnityEngine;

public class MenuUIEntrance : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform title;
    public RectTransform[] buttons;

    [Header("Animation Settings")]
    public float riseDistance = 60f;
    public float titleDuration = 0.6f;
    public float buttonDuration = 0.45f;
    public float buttonDelay = 0.12f;

    private bool hasStarted = false;

    private class UIElement
    {
        public RectTransform rect;
        public CanvasGroup canvasGroup;
        public Vector2 targetPosition;
        public Vector2 startPosition;
    }

    private UIElement titleElement;
    private UIElement[] buttonElements;

    private void Awake()
    {
        titleElement = PrepareElement(title);

        buttonElements = new UIElement[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttonElements[i] = PrepareElement(buttons[i]);
        }
    }

    private UIElement PrepareElement(RectTransform rect)
    {
        CanvasGroup group = rect.GetComponent<CanvasGroup>();

        if (group == null)
        {
            group = rect.gameObject.AddComponent<CanvasGroup>();
        }

        UIElement element = new UIElement();

        element.rect = rect;
        element.canvasGroup = group;

        // final UI position 
        element.targetPosition = rect.anchoredPosition;

        // Start slightly below the final position.
        element.startPosition = element.targetPosition + Vector2.down * riseDistance;

        rect.anchoredPosition = element.startPosition;

        // Hide UI before the entrance
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;

        return element;
    }

    // Called by Player's Animation
    public void RevealMenu()
    {
        if (hasStarted) return;

        hasStarted = true;
        StartCoroutine(PlayEntrance());
    }

    private IEnumerator PlayEntrance()
    {
        // First reveal the title.
        StartCoroutine(AnimateElement(titleElement, titleDuration));

        yield return new WaitForSeconds(0.25f);

        foreach (UIElement button in buttonElements)
        {
            StartCoroutine(AnimateElement(button, buttonDuration));

            yield return new WaitForSeconds(buttonDelay);
        }
    }

    private IEnumerator AnimateElement(UIElement element, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);


            float eased = 1f - Mathf.Pow(1f - t, 3f);

            element.rect.anchoredPosition = Vector2.Lerp(element.startPosition, element.targetPosition, eased);

            element.canvasGroup.alpha = t;

            yield return null;
        }

        element.rect.anchoredPosition = element.targetPosition;
        element.canvasGroup.alpha = 1f;
        element.canvasGroup.interactable = true;
        element.canvasGroup.blocksRaycasts = true;
    }
}

