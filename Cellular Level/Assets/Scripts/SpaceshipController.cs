using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float speed = 5f;             // Speed at which the spaceship moves
    public float targetTolerance = 0.5f; // How close to the target position before choosing a new one
    public float rotationSpeed = 5f;     // Speed of the rotation towards the target

    public float xMin = -50f, xMax = 50f;  // X range for target points
    public float yMin = 0f, yMax = 20f;    // Y range for target points
    public float zMin = -50f, zMax = 50f;  // Z range for target points

    private Vector3 targetPosition;

    void Start()
    {
        // Set the initial random target position within the defined area
        SetNewTargetPosition();
    }

    void Update()
    {
        // Move the spaceship towards the target position
        MoveTowardsTarget();

        // Rotate the spaceship to face the target position
        RotateTowardsTarget();

        // Check if the spaceship is close enough to the target to choose a new one
        if (Vector3.Distance(transform.position, targetPosition) <= targetTolerance)
        {
            // Call a method to destroy the spaceship
            DestroySpaceship();
        }
    }

    private void MoveTowardsTarget()
    {
        // Calculate the direction towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the spaceship in that direction
        transform.position += direction * speed * Time.deltaTime;
    }

    private void RotateTowardsTarget()
    {
        // Calculate the direction vector towards the target
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Create a rotation that faces the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SetNewTargetPosition()
    {
        // Generate a new random target position within the specified area
        targetPosition = new Vector3(
            Random.Range(xMin, xMax),
            Random.Range(yMin, yMax),
            Random.Range(zMin, zMax)
        );
    }

    private void DestroySpaceship()
    {
        // Destroy this spaceship object
        Destroy(gameObject);
    }

    // Optional: Visualize the target position in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.5f);
    }
}
