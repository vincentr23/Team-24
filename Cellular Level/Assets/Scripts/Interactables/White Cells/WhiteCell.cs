using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCell : MonoBehaviour
{
    Vector3 target;
    [SerializeField] float speed = 3;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position =
            Vector3.MoveTowards(transform.position, target, step);
    }
    public void SetTarget(GameObject target)
    {
        this.target = target.transform.position;
    }
}
