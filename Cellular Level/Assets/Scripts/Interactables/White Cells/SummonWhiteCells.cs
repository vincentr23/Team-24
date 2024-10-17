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
    }

    void Summon(InputAction.CallbackContext context)
    {
        if ((player.GetComponent<Interactor>().whiteCellsHeld > 0) &&
            (context.action.triggered))
        {
            player = GetComponent<PlayerController>();
            var closestEnemy = FindClosestEnemy();
            GameObject addedObj = Instantiate(WhiteCellPrefab,
                player.transform.position, transform.rotation);
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
