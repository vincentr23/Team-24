using UnityEngine;

public class Scene2HealthUpdater : MonoBehaviour
{
    private void Start()
    {
        HealthManager.Instance.ApplyHealthIncrease(); // Apply health increase from collected 
        Debug.Log(HealthManager.Instance.playerHealth);
        // You can add UI updates or other logic here if needed
        HealthManager.Instance.itemCounter = 0;
    }
}
