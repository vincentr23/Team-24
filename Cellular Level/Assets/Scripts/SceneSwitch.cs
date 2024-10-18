using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneSwitch : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player")) // Ensure the player GameObject has the "Player" tag
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
