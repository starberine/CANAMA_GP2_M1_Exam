using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public Vector3 offset = new Vector3(0, 2, -5); // Offset for third-person perspective
    public float smoothSpeed = 0.2f; // Smooth follow speed
    public float rotationSpeed = 3f; // Speed for RMB camera control

    private float yaw = 0f; // Horizontal rotation
    private float pitch = 15f; // Vertical rotation (default pitch for a better view)

    private void Start()
    {
        if (player != null)
        {
            yaw = player.eulerAngles.y;
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in the CameraFollow script!");
        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // Handle manual camera rotation with RMB
            if (Input.GetMouseButton(1)) // Right Mouse Button pressed
            {
                yaw += Input.GetAxis("Mouse X") * rotationSpeed;
                pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
                pitch = Mathf.Clamp(pitch, -35f, 60f); // Clamp pitch to prevent extreme angles
            }
            else
            {
                // Automatically align the camera behind the player
                yaw = Mathf.LerpAngle(yaw, player.eulerAngles.y, Time.deltaTime * rotationSpeed);
            }

            // Calculate the rotation and position of the camera
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 targetPosition = player.position + rotation * offset;

            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Look at the player
            transform.LookAt(player.position + Vector3.up * 1.5f); // Slightly above the player's center
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in the CameraFollow script!");
        }
    }
}
