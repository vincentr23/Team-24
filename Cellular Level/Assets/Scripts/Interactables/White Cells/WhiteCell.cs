using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhiteCell : MonoBehaviour
{
    GameObject target;
    Animator anim;
    [SerializeField] float speed = 3;
    public NavMeshAgent agent;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        anim = GetComponent<Animator>();
        anim.SetBool("Running", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            anim.SetBool("Running", false);
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 3)
        {
            anim.SetBool("Running", false);
            anim.SetBool("Destroy", true);
            agent.ResetPath();
            return;
        }
        agent.SetDestination(target.transform.position);
        //var newPos = Vector3.MoveTowards(transform.position, target, step);
        //transform.position = newPos;
        //transform.LookAt(newPos);
        //if ((Vector3.Distance(transform.position, target)) < 1)
        //{
        //    anim.SetBool("Running", false);
        //}
        //else 
        //    anim.SetBool("Running", true);
    }
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

}
