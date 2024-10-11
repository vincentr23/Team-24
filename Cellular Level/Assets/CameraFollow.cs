using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;    // The player to follow
    public Vector3 offset;               // Offset from the player

    public float smoothSpeed = 0.125f;   // Smoothing speed for camera follow

    void LateUpdate()
    {
        // Calculate the desired position based on the player's position and the offset
        Vector3 desiredPosition = playerTransform.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera position
        transform.position = smoothedPosition;

        // Optional: Make the camera look at the player
        transform.LookAt(playerTransform);
    }
}
