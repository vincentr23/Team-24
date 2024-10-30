using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Pickups")]
    public GameObject pickupPrefab;
    public GameObject[] pickupSpawns;
    [SerializeField] uint numObjects;
    private List<int> randomList = new();

    [Header("Cancer")]
    public GameObject cancerPrefab;
    public GameObject[] cancerSpawns;
    [SerializeField] uint numCancers;
    public List<GameObject> cancers = new();

    [Header("Oxygen")]
    [SerializeField] float oxygen = 100f;
    [SerializeField] float totalOxygen = 100f;
    [SerializeField] float oxygenTick = 0.5f;
    [SerializeField] float oxyMult = 10;
    // Start is called before the first frame update
    void Start()
    {
        pickupSpawns = GameObject.FindGameObjectsWithTag("Pickup Spawn");
        cancerSpawns = GameObject.FindGameObjectsWithTag("Cancer Spawn");

        if (numObjects > pickupSpawns.Length) numObjects = (uint)pickupSpawns.Length;

        if (numCancers > cancerSpawns.Length) numCancers = (uint)cancerSpawns.Length;

        // creating pickups (Oxygen)
        int MyNumber;
        // creating a list with possible spawns
        for (int i = 0; i < numObjects; i++)
        { 
            // create a random num
            MyNumber = (int)Random.Range(0, numObjects);
            // keep creating until we get a unique num
            while (randomList.Contains(MyNumber))
                MyNumber = (int)Random.Range(0, numObjects);

            randomList.Add(MyNumber);
        }

        // instantiating pickups
        foreach (int pickupSpawn in randomList)
        {
            Instantiate(pickupPrefab, pickupSpawns[pickupSpawn].transform.position,
                pickupSpawns[pickupSpawn].transform.rotation);
        }

        randomList.Clear();
        // just a copy paste of the above with cancer instead
        for (int i = 0; i < numCancers; i++)
        {
            MyNumber = (int)Random.Range(0, numCancers);
            while (randomList.Contains(MyNumber))
                MyNumber = (int)Random.Range(0, numCancers);

            randomList.Add(MyNumber);
        }

        foreach (int cancerSpawn in randomList)
        {
            Instantiate(cancerPrefab, cancerSpawns[cancerSpawn].transform.position,
                cancerSpawns[cancerSpawn].transform.rotation);
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
        foreach (var cancer in cancers)
        {
            cancer.SetActive(false);
        }
        cancers.Clear();
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
