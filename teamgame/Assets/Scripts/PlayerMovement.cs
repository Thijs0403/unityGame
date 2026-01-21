using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float gravity = -9.81f; // New: Gravity force

    [Header("References")]
    private CharacterController controller; // New: Reference to the controller
    private Camera playerCamera;
    private float xRotation = 0f;
    private Vector3 velocity; // New: Stores downward speed

    void Start()
    {
        // Get the CharacterController component on the Player root
        controller = GetComponent<CharacterController>();
        
        // Your original camera setup
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        // --------- Looking Logic ----------
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // --------- Movement Logic (Updated) ----------
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate direction based on where the player is facing
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the controller instead of the transform
        controller.Move(move * moveSpeed * Time.deltaTime);

        // --------- Gravity Logic (New) ----------
        // Reset downward velocity if we are on the ground
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}