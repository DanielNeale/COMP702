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

        if (hit == true && hit.transform.GetComponent<CircleCollider2D>() && hit.transform.position.y < transform.position.y)
        {
            NewParent(hit.transform);
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
