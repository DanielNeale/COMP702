using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterMovement : MonoBehaviour
{
    [SerializeField]
    private float detectionRange = 1f;
    private Transform parent;


    private void FixedUpdate()
    {
        // detects if the counter can move
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, detectionRange);

        // attempts move
        if (hit == true && hit.transform.position.y < transform.position.y)
        {
            // moves to next slot
            if (hit.transform.GetComponent<CircleCollider2D>())
            {
                NewParent(hit.transform);
            }

            // moves to end
            else if (hit.transform.GetComponent<BoxCollider2D>())
            {
                // re-enables slot collider and moves to the end
                parent.GetComponent<CircleCollider2D>().enabled = true;
                transform.SetParent(hit.transform, false);
            }
        }

        
    }


    /// <summary>
    /// moves the counter from one slot to another
    /// </summary>
    /// <param name="newParent"></param>
    private void NewParent(Transform newParent)
    {
        // re-enables parents collider if it exists
        if (parent)
        {
            parent.GetComponent<CircleCollider2D>().enabled = true;
        }    

        // sets a new parent and moves to it
        parent = newParent;
        transform.SetParent(parent, false);

        // disables parents collider
        parent.GetComponent<CircleCollider2D>().enabled = false;
    }
}
