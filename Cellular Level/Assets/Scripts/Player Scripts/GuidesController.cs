using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidesController : MonoBehaviour
{
    private List<GameObject> guides = new();
    private GameObject[] pickupSpawns;
    [SerializeField] float frequency = 5.0f;
    [SerializeField] GameObject guidePrefab;
    private float timer = 0;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > frequency)
        {
            if (guides != null) DeleteAllGuides();

            pickupSpawns = GameObject.FindGameObjectsWithTag("PickUp");
            GameObject addedObject;
            var closest = Vector3.Distance(player.transform.position,
                pickupSpawns[0].transform.position);
            var closestObj = pickupSpawns[0];
            foreach (GameObject pickupSpawn in pickupSpawns)
            {
                var dist = Vector3.Distance(player.transform.position,
                    pickupSpawn.transform.position);
                if (dist < closest)
                {
                    closestObj = pickupSpawn;
                    closest = dist;
                }
            }
            var spawnPos = transform.position;
            spawnPos.y = 1f;
            addedObject = Instantiate(guidePrefab,
                spawnPos, transform.rotation);
            addedObject.GetComponent<Guide>().SetCoords(closestObj.transform.position);
            FixLayers(addedObject);
            guides.Add(addedObject);
            timer -= frequency;
        }
        timer += Time.deltaTime;
    }
    void DeleteAllGuides()
    {
        foreach (GameObject guide in guides)
        {
            if (guide != null)
            {
                Destroy(guide); // Destroy the GameObject associated with the Guide component
            }
        }
        guides.Clear();
    }

    void FixLayers(GameObject guide)
    {
        guide.layer = LayerMask.NameToLayer(player.DecipherLayer(4 + player.PlayerNum()));
    }
}
