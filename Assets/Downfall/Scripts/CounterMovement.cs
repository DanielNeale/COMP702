using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterMovement : MonoBehaviour
{
    private float detectionRange = 1f;
    private Transform parent;


    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, detectionRange);
        Debug.DrawRay(transform.position, transform.up * detectionRange, Color.blue);

        if (hit.transform.parent.GetComponent<WheelMovement>())
        {
            NewParent(hit.transform);
        }
    }


    private void NewParent(Transform newParent)
    {
        parent = newParent;
        transform.parent = parent;
    }
}
