using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // The item prefab to spawn
    public MeshFilter meshFilter; // The mesh filter of the AI mesh
    public int numberOfItems = 10; // Number of items to spawn

    private void Start()
    {
        SpawnItems();
    }

    private void SpawnItems()
    {
        // Get the bounds of the mesh
        Bounds bounds = meshFilter.mesh.bounds;

        // Transform bounds to world space
        Vector3 center = meshFilter.transform.position + bounds.center;
        Vector3 size = bounds.size;

        for (int i = 0; i < numberOfItems; i++)
        {
            // Generate a random position within the bounds
            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                Random.Range(center.y - size.y / 2, center.y + size.y / 2),
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            // Instantiate the item at the random position
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
        }
    }
}
