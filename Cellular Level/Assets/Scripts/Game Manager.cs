using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pickupPrefab;
    public GameObject[] pickupSpawns;
    [SerializeField] uint numObjects;
    private List<int> randomList = new List<int>();
    
    [SerializeField] float oxygen = 100f;
    [SerializeField] float totalOxygen = 100f;
    [SerializeField] float oxygenTick = 0.5f;
    [SerializeField] float oxyMult = 10;
    // Start is called before the first frame update
    void Start()
    {
        pickupSpawns = GameObject.FindGameObjectsWithTag("Pickup Spawn");
        if (numObjects > pickupSpawns.Length) numObjects = (uint)pickupSpawns.Length; 

        int MyNumber;
        for (int i = 0; i < numObjects; i++)
        { 
            MyNumber = Random.Range(0, pickupSpawns.Length);
            while (randomList.Contains(MyNumber))
                MyNumber = Random.Range(0, pickupSpawns.Length);

            randomList.Add(MyNumber);
        }

        foreach (int pickupSpawn in randomList)
        {
            Instantiate(pickupPrefab, pickupSpawns[pickupSpawn].transform.position,
                pickupSpawns[pickupSpawn].transform.rotation);
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().ToggleSpawn(); ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        oxygen -= oxygenTick;
    }

    public float GetOxygen()
    {
        return oxygen;
    }
    public float GetTotalOxy()
    {
        return totalOxygen;
    }
    public void CollectOxygen(float increase)
    {
        oxygen += (increase * oxyMult);
        if (oxygen > totalOxygen)
            oxygen = totalOxygen;
    }
}
