using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangePF : MonoBehaviour
{
    [SerializeField]
    private int angle;
    [SerializeField]
    private float dist;
    [SerializeField]
    private float cushion;


    private void Start()
    {
        cushion *= Mathf.PI / 180;
    }


    /// <summary>
    /// Runs when the path is blocked by an object to find an alternate route
    /// </summary>
    /// <returns> new direction </returns>
    public Vector3 ObjectAvoidance()
    {
        List<float> safePaths = new List<float>();

        // scans area in front of the ai
        for (int i = -angle; i < angle; i++)
        {
            // calculates the position if a point at i angle
            float thisAngle = i * Mathf.PI / 180;
            Vector3 target = new Vector3(Mathf.Sin(thisAngle), 0, Mathf.Cos(thisAngle));
            target = transform.TransformPoint(target * dist);       

            // adds path as safe if it doesn't hit
            if (!Physics.Linecast(transform.position, target))
            {
                safePaths.Add(thisAngle);
            }

            // removes close safe angles if it does hit
            else
            {
                for (int j = safePaths.Count - 1; j > 0; j--)
                {
                    if (Mathf.Abs(safePaths[j] - thisAngle) < cushion)
                    {
                        safePaths.RemoveAt(j);
                    }
                }
            }
        }

        float bestAngle = 181;

        // calculates best safe direction
        for (int i = 0; i < safePaths.Count; i++)
        {
            if (Mathf.Abs(safePaths[i]) < Mathf.Abs(bestAngle))
            {
                bestAngle = safePaths[i];                
            }
        }

        // transforms safest angle into a direction
        Vector3 dir = new Vector3(Mathf.Sin(bestAngle), 0, Mathf.Cos(bestAngle));

        return dir;
    }
}
