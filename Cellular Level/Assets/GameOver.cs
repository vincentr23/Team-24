using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOver : MonoBehaviour
{
    
    public void PlayGame()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] deadPlayers = GameObject.FindGameObjectsWithTag("Dead");

        if (players != null)
            foreach (var player in players)
            {
                Destroy(player);
            }
        if (deadPlayers != null)
            foreach (var player in deadPlayers)
            {
                Destroy(player);
            }
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
