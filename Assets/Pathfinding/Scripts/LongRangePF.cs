using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the long range aspect of the pathfinding
/// </summary>
public class LongRangePF : MonoBehaviour
{
    [SerializeField]
    private float nodeGap;
    [SerializeField]
    private Transform node;
    private Vector3[] nodeReach;

    public Transform debugStart;
    public Transform debugEnd;


    /// <summary>
    /// Sets up an array with the 8 cardinal and intercardinal directions
    /// </summary>
    private void Start()
    {
        nodeReach = new Vector3[8];

        nodeReach[0] = new Vector3(0, 0, 1) * nodeGap;
        nodeReach[1] = new Vector3(1, 0, 1) * nodeGap;
        nodeReach[2] = new Vector3(1, 0, 0) * nodeGap;
        nodeReach[3] = new Vector3(1, 0, -1) * nodeGap;
        nodeReach[4] = new Vector3(0, 0, -1) * nodeGap;
        nodeReach[5] = new Vector3(-1, 0, -1) * nodeGap;
        nodeReach[6] = new Vector3(-1, 0, 0) * nodeGap;
        nodeReach[7] = new Vector3(-1, 0, 1) * nodeGap;
    }


    /// <summary>
    /// Coroutine that will work out an overal path for the ai by placing nodes in a
    /// sequence until an appropriate path is found
    /// </summary>
    /// <param name="agent"> The ai agent </param>
    /// <param name="start"> Start of the path </param>
    /// <param name="end"> End of the path </param>
    /// <returns> Returns the path to the agent </returns>
    public IEnumerator CalculatePath(CubeMovement agent, Vector3 start, Vector3 end)
    {
        Transform lastNode = null;

        // puts all nodes into a stack organsied by distance to the end, closest at the top
        Stack<Transform> nodes = new Stack<Transform>();
        Transform firstNode = Instantiate(node, start, node.rotation, transform);
        nodes.Push(firstNode);

        // as long as a path isn't found, keep placing more nodes
        while (lastNode == null)
        {
            // takes the top closest unused node and generates new nodes
            Transform currentNode = nodes.Pop();
            List<Transform> newNodes = new List<Transform>();

            // tries to create 8 new nodes
            for (int i = 0; i < nodeReach.Length; i++)
            {
                Vector3 tryPos = currentNode.position + nodeReach[i];

                // only places nodes if space is free
                if (!Physics.Linecast(currentNode.position, tryPos))
                {
                    Transform newNode = Instantiate(node, tryPos, node.rotation, currentNode);
                    newNodes.Add(newNode);

                    // ends search if node is created near end
                    if (Vector3.Distance(newNode.position, end) < nodeGap)
                    {
                        lastNode = newNode;
                    }
                }
            }

            // sorts nodes from closest to furthest
            List<Transform> sortedNodes = new List<Transform>();
            int nodeCount = newNodes.Count;

            for (int i = 0; i < nodeCount; i++)
            {
                float closestDist = Mathf.Infinity;
                int index = 0;

                for (int j = 0; j < nodeCount - i; j++)
                {
                    float dist = Vector3.Distance(newNodes[j].position, end);

                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        index = j;
                    }
                }

                sortedNodes.Add(newNodes[index]);
                newNodes.RemoveAt(index);
            }

            // push sorted nodes onto the node stack
            for (int i = sortedNodes.Count - 1; i >= 0; i--)
            {
                nodes.Push(sortedNodes[i]);
            }
        }


        // creates a path in reverse
        List<Vector3> path = new List<Vector3>();
        path.Add(end);

        // adds nodes by parent
        while (lastNode.parent != null)
        {
            path.Add(lastNode.position);
            lastNode = lastNode.parent;
        }

        path.Add(start);


        // reverses the path so the ai can read it
        path.Reverse();
        // sends path data to ai
        agent.SetPath(path.ToArray());
        // destroys all the nodes created
        Destroy(transform.GetChild(0).gameObject);

        yield return new WaitForEndOfFrame();
    }
}
