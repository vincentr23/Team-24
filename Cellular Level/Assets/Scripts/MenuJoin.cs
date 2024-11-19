using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuJoin : MonoBehaviour
{
    [SerializeField] Transform[] spawnpoint;
    private int numPlayers;

    private void Awake()
    {
        // Subscribe to the onPlayerJoined event
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    public void Start()
    {
        numPlayers = 0;
    }
    public void OnPlayerJoined(PlayerInput player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.m_initialPosition = spawnpoint[numPlayers];
        numPlayers++;
        Debug.Log("Player joined");
    }
}