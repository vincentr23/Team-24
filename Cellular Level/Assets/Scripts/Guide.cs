using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guide : MonoBehaviour
{
    [SerializeField] float speed = 2;
    private Vector3 chaseCoords;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        agent.SetDestination(chaseCoords);
        //transform.position = Vector3.MoveTowards(transform.position, chaseCoords, step);
    }
    
    public void SetCoords(Vector3 coords)
    {
        chaseCoords = coords;
    }
}
