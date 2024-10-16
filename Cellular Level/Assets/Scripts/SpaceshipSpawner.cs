using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{
    public GameObject[] spaceshipPrefabs;  // Array to store different spaceship prefabs
    public float spawnInterval = 5f;       // Interval between spawns
    public float speed = 5f;               // Speed at which the spaceship moves

    public float xMin = -50f, xMax = 50f;  // X range for spawn points
    public float zMin = -50f, zMax = 50f;  // Z range for spawn points
    public float yMin = 0f, yMax = 20f;    // Y range for target points

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnSpaceship();
            timer = 0f;  // Reset the timer
        }
    }

    private void SpawnSpaceship()
    {
        if (spaceshipPrefabs.Length == 0) return;

        // Randomly select a spaceship variant
        int randomIndex = Random.Range(0, spaceshipPrefabs.Length);
        GameObject selectedPrefab = spaceshipPrefabs[randomIndex];

        // Get a random position around the spawn area
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Instantiate the selected spaceship variant
        GameObject newSpaceship = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // Disable collisions with other spaceships (if needed)
        Collider newSpaceshipCollider = newSpaceship.GetComponent<Collider>();
        SpaceshipController[] allSpaceships = FindObjectsOfType<SpaceshipController>();
        foreach (SpaceshipController spaceship in allSpaceships)
        {
            Collider existingCollider = spaceship.GetComponent<Collider>();
            if (newSpaceshipCollider != null && existingCollider != null)
            {
                Physics.IgnoreCollision(newSpaceshipCollider, existingCollider);
            }
        }

        // Add movement to the spaceship
        Rigidbody rb = newSpaceship.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = GetRandomDirection(spawnPosition);
            rb.velocity = direction * speed;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Spawn randomly around the defined area
        int edge = Random.Range(0, 4);
        switch (edge)
        {
            case 0: return new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMin), zMax); // Top
            case 1: return new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMin), zMin); // Bottom
            case 2: return new Vector3(xMin, Random.Range(yMin, yMin), Random.Range(zMin, zMax)); // Left
            case 3: return new Vector3(xMax, Random.Range(yMin, yMin), Random.Range(zMin, zMax)); // Right
            default: return Vector3.zero;
        }
    }

    private Vector3 GetRandomDirection(Vector3 spawnPosition)
    {
        Vector3 targetPosition = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMin), Random.Range(zMin, zMax));
        return (targetPosition - spawnPosition).normalized;
    }
}
