using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangePF : MonoBehaviour
{
    [SerializeField]
    private float nodeGap;
    [SerializeField]
    private Transform node;
    private Vector3[] nodeReach;

    public Transform debugStart;
    public Transform debugEnd;


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


    public Vector3[] NewPath(Vector3 start, Vector3 end)
    {


        return null;
    }


    public IEnumerator CalculatePath(CubeMovement cube, Vector3 start, Vector3 end)
    {
        Transform lastNode = null;

        Stack<Transform> nodes = new Stack<Transform>();
        Transform firstNode = Instantiate(node, start, node.rotation, transform);
        nodes.Push(firstNode);

        while (lastNode == null)
        {
            Transform currentNode = nodes.Pop();
            List<Transform> newNodes = new List<Transform>();

            for (int i = 0; i < nodeReach.Length; i++)
            {
                Vector3 tryPos = currentNode.position + nodeReach[i];

                if (!Physics.Linecast(currentNode.position, tryPos))
                {
                    Transform newNode = Instantiate(node, tryPos, node.rotation, currentNode);
                    newNodes.Add(newNode);

                    if (Vector3.Distance(newNode.position, end) < nodeGap)
                    {
                        lastNode = newNode;
                    }
                }
            }

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


            for (int i = sortedNodes.Count - 1; i >= 0; i--)
            {
                nodes.Push(sortedNodes[i]);
            }
        }


        List<Vector3> path = new List<Vector3>();
        path.Add(end);

        while(lastNode.parent != null)
        {
            path.Add(lastNode.position);
            lastNode = lastNode.parent;
        }

        path.Add(start);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.blue, 10);
        }

        path.Reverse();

        cube.SetPath(path.ToArray());

        Destroy(transform.GetChild(0).gameObject);

        for (int i = transform.childCount - 1; i > 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        yield return new WaitForEndOfFrame();
    }
}
