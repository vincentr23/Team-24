using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject[] players;
    [SerializeField] EnemyPatrol enemy;
    [SerializeField] float radius;
    [SerializeField] float maxPitch;
    [SerializeField] float minPitch;
    float pitch;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        players = GameObject.FindGameObjectsWithTag("Player");
        enemy = FindObjectOfType<EnemyPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        pitch = radius / GetClosest();
        if (pitch > maxPitch) pitch = maxPitch;
        if (pitch < minPitch) pitch = minPitch;
        audioSource.pitch = pitch;
    }
    
    float GetClosest()
    {
        if (players[0] == null) return 0;
        var enemyPos = enemy.gameObject.transform.position;
        var closest = Vector3.Distance(players[0].transform.position, enemyPos);
        foreach (var player in players)
        {
            var curr = Vector3.Distance(player.transform.position, enemyPos);
            if (curr < closest) closest = curr;
        }
        return closest;
    }

    public void RebasePlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
