using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SummonWhiteCells : MonoBehaviour
{
    private GameObject[] enemies;
    private PlayerController player;
    public GameObject WhiteCellPrefab;
 
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GetComponent<PlayerController>();
    }

    public void Summon(InputAction.CallbackContext context)
    {
        if ((player.GetComponent<Interactor>().whiteCellsHeld > 0) &&
            (context.action.triggered))
        {
            player = GetComponent<PlayerController>();
            var closestEnemy = FindClosestEnemy();
            var spawnPos = new Vector3(
                player.transform.localPosition.x,
                player.transform.localPosition.y + 1f, 
                player.transform.localPosition.z + 2f);
            GameObject addedObj = Instantiate(WhiteCellPrefab,
                spawnPos, transform.rotation);
            addedObj.GetComponent<WhiteCell>().SetTarget(closestEnemy);
            player.GetComponent<Interactor>().whiteCellsHeld--;
        }
    }
    GameObject FindClosestEnemy()
    {
        var closest = Vector3.Distance(player.transform.position,
            enemies[0].transform.position);
        var closestEnemy = enemies[0];
        foreach (GameObject enemy in enemies)
        {
            var current = Vector3.Distance(player.transform.position,
            enemies[0].transform.position);
            if (current < closest)
            {
                closestEnemy = enemy;
                closest = current;
            }
        }
        return closestEnemy;
    }
}
