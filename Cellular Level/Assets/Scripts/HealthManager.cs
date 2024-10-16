using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance; // Singleton instance

    public int playerHealth = 100; // Starting health
    public int itemCounter = 0;     // Counter for collected items

    private bool isDiminishingHealth = false; // Flag to control health diminishing

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            Instance = this; // Assign this instance to the singleton
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    private void Update()
    {
        // Diminish health only in Scene 1
        if (SceneManager.GetActiveScene().name == "CaveLevel" && isDiminishingHealth)
        {
            Debug.Log("In Cave Level");
            DiminishHealth(Time.deltaTime);
            Debug.Log(playerHealth);
        }
    }

    public void StartDiminishingHealth()
    {
        isDiminishingHealth = true;
    }

    public void StopDiminishingHealth()
    {
        isDiminishingHealth = false;
    }

    private void DiminishHealth(float amount)
    {
        playerHealth -= (int)(amount * 10); // Diminish health over time
        if (playerHealth < 0)
        {
            playerHealth = 0; // Ensure health doesn't go below 0
        }
    }

    public void CollectItem()
    {
        itemCounter++; // Increase item counter
    }

    public void ApplyHealthIncrease()
    {
        playerHealth += itemCounter * 10; // Increase health based on collected items
        itemCounter = 0; // Reset item counter after applying health
    }
}
