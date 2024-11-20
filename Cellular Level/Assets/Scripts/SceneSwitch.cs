using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneSwitch : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load
    [SerializeField] GameObject[] players;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player")) // Ensure the player GameObject has the "Player" tag
        {
            // Load the specified scene
            //SceneManager.LoadScene(sceneToLoad);
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().ToggleSpawn();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
