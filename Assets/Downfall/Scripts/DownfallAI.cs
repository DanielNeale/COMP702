using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownfallAI : MonoBehaviour
{
    [SerializeField]
    private Transform[] AIWheels;
    [SerializeField]
    private Transform[] exits;
    private List<PossibleMoves> moveList = new List<PossibleMoves>();


    private void Start()
    {
        List<Transform> slots = new List<Transform>();

        for (int i = 0; i < AIWheels.Length; i++)
        {
            for (int j = 0; j < AIWheels[i].childCount; j++)
            {
                slots.Add(AIWheels[i].GetChild(j));
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            List<Transform> newExits = new List<Transform>();

            for (int j = 0; j < exits.Length; j++)
            {
                for (int k = 0; k < exits[j].GetComponent<Exit>().ConnectedWheels.Length; k++)
                {
                    if (exits[j].GetComponent<Exit>().ConnectedWheels[k] == slots[i].parent)
                    {
                        newExits.Add(exits[j]);
                    }
                }
            }

            PossibleMoves newMove = new PossibleMoves();
            List<float> angles = new List<float>();

            for (int j = 0; j < newExits.Count; j++)
            {

            }

        }

        // Set wheels and start game
    }


    public Rotation Move()
    {
        Rotation newMove = new Rotation();

        

        return newMove;
    }
}


public class PossibleMoves
{
    private Transform slot;
    private float[] options;

    public void SetMoves(Transform newSlot, float[] newOptions)
    {
        slot = newSlot;
        options = newOptions;
    }

    public Transform GetSlot()
    {
        return slot;
    }

    public float GetMove()
    {
        return options[Random.Range(0, options.Length)];
    }
}
