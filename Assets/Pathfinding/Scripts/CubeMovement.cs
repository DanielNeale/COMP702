using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField]
    private LongRangePF pathFinder;
    [SerializeField]
    private ShortRangePF objectAvoidance;
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
        // starts the pathfinding
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(pathFinder.CalculatePath(this, transform.position, target.position));
        }
    }


    private void FixedUpdate()
    {
        // only runs when there is a path to follow
        if (path.Length > 0)
        {
            // try to shortcut the path
            for (int i = path.Length - 1; i > waypoint; i--)
            {
                if (!Physics.Linecast(transform.position, path[i]))
                {
                    waypoint = i;
                    break;
                }
            }
            
            // switch between systems depending on information
            if (Physics.Linecast(transform.position, path[waypoint]))
            {
                moveTarget = objectAvoidance.ObjectAvoidance();
                print(moveTarget);
            }

            else
            {               
                moveTarget = path[waypoint];
                print(moveTarget);
            }            

            // look and move
            transform.LookAt(moveTarget);
            transform.position += (transform.forward * speed);

            // move onto the next waypoint if close enough
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


    /// <summary>
    /// Path data sent from LongRangePF
    /// </summary>
    /// <param name="newPath"> new path data </param>
    public void SetPath(Vector3[] newPath)
    {
        path = newPath;
    }
}
