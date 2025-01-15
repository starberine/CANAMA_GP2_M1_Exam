using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f; // Base movement speed
    public float jumpHeight = 1.5f; // Jump height
    public float gravity = -20.0f; // Gravity force

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump = true;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float rotationSpeed = 5.0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canJump = true;
        }

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isGrounded)
        {
            move = Vector3.ProjectOnPlane(move, GetGroundNormal());
        }

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        controller.Move(move * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded && move.magnitude > 0.1f && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }

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

    public IEnumerator ActivateSpeedBoost(float boostAmount, float duration)
    {
        float originalSpeed = speed;
        speed = boostAmount;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
    }
}
