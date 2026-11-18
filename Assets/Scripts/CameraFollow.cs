using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Player and center of the level
    public Transform player;
    public Transform overviewCenter;

    // Normal and zoomed out camera sizes
    public float normalSize = 5f;
    public float overviewSize = 12f;

    // Check if the player is on the reveal platform
    public bool showingOverview = false;

    private Camera gameCamera;

    void Start()
    {
        // Get the Camera component
        gameCamera = GetComponent<Camera>();
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

        // Move camera while keeping its Z position
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        // Change camera zoom
        if (showingOverview)
        {
            gameCamera.orthographicSize = overviewSize;
        }
        else
        {
            gameCamera.orthographicSize = normalSize;
        }
    }
}
