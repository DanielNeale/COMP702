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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, detectionRange);
        Debug.DrawRay(transform.position, transform.up * detectionRange, Color.blue);

        if (hit == true && hit.transform.position.y < transform.position.y)
        {
            if (hit.transform.GetComponent<CircleCollider2D>())
            {
                NewParent(hit.transform);
            }

            else if (hit.transform.GetComponent<BoxCollider2D>())
            {
                parent.GetComponent<CircleCollider2D>().enabled = true;
                transform.SetParent(hit.transform, false);
            }
        }

        
    }


    private void NewParent(Transform newParent)
    {
        if (parent)
        {
            parent.GetComponent<CircleCollider2D>().enabled = true;
        }    

        parent = newParent;
        transform.SetParent(parent, false);

        parent.GetComponent<CircleCollider2D>().enabled = false;
    }
}
