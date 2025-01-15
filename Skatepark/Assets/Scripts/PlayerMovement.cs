using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f; // Movement speed
    public float jumpHeight = 1.5f; // Jump height
    public float gravity = -20.0f; // Gravity force (increased for quicker descent)

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump = true; // Prevent jump spamming

    public Transform groundCheck; // Empty GameObject to check if the player is on the ground
    public float groundDistance = 0.4f; // Radius of the sphere for ground check
    public LayerMask groundMask; // Layer mask for ground

    public float rotationSpeed = 5.0f; // Rotation speed multiplier

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset velocity when grounded
            canJump = true; // Allow jumping again
        }

        // Get input for movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Adjust movement for slopes
        if (isGrounded)
        {
            move = Vector3.ProjectOnPlane(move, GetGroundNormal());
        }

        // Rotate player to face the movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        controller.Move(move * speed * Time.deltaTime);

        // Jump only while grounded
        if (Input.GetButtonDown("Jump") && isGrounded && move.magnitude > 0.1f && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false; // Disable jumping until grounded again
        }

        // Prevent further jumping until grounded
        if (!isGrounded)
        {
            canJump = false;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private Vector3 GetGroundNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance + 0.1f, groundMask))
        {
            return hit.normal;
        }
        return Vector3.up;
    }
}
