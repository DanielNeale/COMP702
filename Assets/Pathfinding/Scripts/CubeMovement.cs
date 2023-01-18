using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField]
    private LongRangePF pathFinder;
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float waypointTolerance;

    private Vector3 moveTarget;
    private Vector3[] path = new Vector3[0];
    private int waypoint = 0;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(pathFinder.CalculatePath(this, transform.position, target.position));
        }
    }


    private void FixedUpdate()
    {
        if (path.Length > 0)
        {
            transform.LookAt(path[waypoint]);
            transform.position += (transform.forward * speed);

            if (Vector3.Distance(transform.position, path[waypoint]) < waypointTolerance)
            {
                waypoint++;

                if (waypoint >= path.Length)
                {
                    path = new Vector3[0];
                    waypoint = 0;
                }
            }
        }
    }


    public void SetPath(Vector3[] newPath)
    {
        path = newPath;
    }
}
