using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Player and center of the level
    public Transform player;
    public Transform overviewCenter;

    // Normal and zoomed out camera sizes
    public float normalSize = 5f;
    public float overviewSize = 12f;

    // Speed of camera movement and zoom
    public float moveSpeed = 5f;
    public float zoomSpeed = 4f;

    // Check if the player is on the reveal platform
    public bool showingOverview = false;

    private Camera gameCamera;

    void Start()
    {
        // Get the Camera component
        gameCamera = GetComponent<Camera>();

        // Start the camera on the player
        if (player != null)
        {
            transform.position = new Vector3(
                player.position.x,
                player.position.y,
                transform.position.z
            );
        }

        // Start with the normal camera size
        gameCamera.orthographicSize = normalSize;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Follow the player by default
        Transform target = player;

        // Look at the level center during the reveal
        if (showingOverview && overviewCenter != null)
        {
            target = overviewCenter;
        }

        // Set the camera's target position
        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        // Move the camera toward the target
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Choose the camera zoom size
        float targetSize = normalSize;

        if (showingOverview)
        {
            targetSize = overviewSize;
        }

        // Zoom the camera in or out
        gameCamera.orthographicSize = Mathf.Lerp(
            gameCamera.orthographicSize,
            targetSize,
            zoomSpeed * Time.deltaTime
        );
    }
}
