using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pickupPrefab;
    public GameObject[] pickupSpawns;
    private List<int> randomList = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        pickupSpawns = GameObject.FindGameObjectsWithTag("Pickup Spawn");

        int MyNumber;
        for (int i = 0; i < 6; i++)
        { 
            MyNumber = Random.Range(0, pickupSpawns.Length);
            randomList.Add(MyNumber);
        }

        foreach (int pickupSpawn in randomList)
        {
            Instantiate(pickupPrefab, pickupSpawns[pickupSpawn].transform.position,
                pickupSpawns[pickupSpawn].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
