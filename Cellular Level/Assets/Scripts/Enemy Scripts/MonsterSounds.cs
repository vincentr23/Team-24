using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSounds : MonoBehaviour
{
    [SerializeField] AudioSource[] growls;
    [SerializeField] AudioSource roar;
    [SerializeField] int roarTimer = 200;
    [SerializeField] int growlTimer = 200;
    [SerializeField] int maxTimer = 200;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<EnemyPatrol>().CanSeePlayer())
        {
            growlTimer = maxTimer;
            if (roarTimer == maxTimer) Roar();
            roarTimer--;
            if (roarTimer < 0) roarTimer = maxTimer;
        }
        //else roarTimer = maxTimer;

        //if (growlTimer < 0)
        //{
        //    Growls();
        //    growlTimer = maxTimer;
        //}
        //else growlTimer--;
    }

    public void Roar()
    {
        roar.Play();
    }
    void Growls()
    {
        growls[Random.Range(0, 3)].Play();
    }
}
