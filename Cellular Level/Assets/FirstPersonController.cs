using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;         // Movement speed of the player
    public float lookSpeed = 2f;         // Speed of the camera's look/rotation

    public Transform playerCamera;       // Reference to the camera
    private float xRotation = 0f;        // Rotation on the x-axis (up and down)

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Call movement and camera look functions
        MovePlayer();
        LookAround();
    }

    private void MovePlayer()
    {
        // Get input for movement (WASD keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Move the player based on input and speed
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void LookAround()
    {
        // Get the mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Rotate the player body left and right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to avoid looking too far up/down
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
