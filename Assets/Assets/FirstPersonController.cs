using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float mouseSensitivity = 100f;

    // Private variables
    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;

    void Start()
    {
        // Get the CharacterController component attached to this object
        controller = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. CAMERA ROTATION (Mouse Look)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        // Clamp rotation so we can't look too far up or down (90 degrees limit)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera (looking up and down)
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotate the player body (turning left and right)
        transform.Rotate(Vector3.up * mouseX);

        // 2. MOVEMENT (WASD)
        float x = Input.GetAxis("Horizontal"); // A and D keys
        float z = Input.GetAxis("Vertical");   // W and S keys

        // Calculate move direction relative to where the player is facing
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the controller
        controller.Move(move * walkSpeed * Time.deltaTime);

        // 3. GRAVITY
        // Reset velocity if we are grounded to keep it stable
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;

        // Move the controller vertically (falling)
        controller.Move(velocity * Time.deltaTime);
    }
}