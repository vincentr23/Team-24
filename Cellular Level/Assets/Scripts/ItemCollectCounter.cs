using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oxygen")) // Assuming your items have the tag "Item"
        {
            HealthManager.Instance.CollectItem(); // Collect item
            Debug.Log(HealthManager.Instance.itemCounter);
            Destroy(other.gameObject); // Destroy the collected item
        }
    }
}
