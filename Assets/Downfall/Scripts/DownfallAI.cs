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
            
            List<float> angles = new List<float>();

            for (int j = 0; j < newExits.Count; j++)
            {
                Vector2 exitPos = newExits[j].position;
                Vector2 wheelPos = slots[i].parent.position;

                float newAngle = Vector2.Angle(Vector2.up, exitPos - wheelPos);
                
                if (exitPos.x < wheelPos.x)
                {
                    newAngle = 360 - newAngle;
                }

                angles.Add(newAngle);
                angles.Add(newAngle + 360);
            }

            PossibleMoves newMove = new PossibleMoves();
            float[] newOptions = angles.ToArray();

            newMove.SetMoves(slots[i], newOptions);
            moveList.Add(newMove);
        }

        // Set wheels and start game
    }


    public Rotation Move()
    {
        Rotation newMove = new Rotation();

        PossibleMoves randMove = moveList[Random.Range(0, moveList.Count)];
        float rotation = randMove.GetMove();
        Transform wheel = randMove.GetSlot().parent;

        rotation = (rotation - wheel.rotation.z) / 6;

        newMove.Set(WheelToInt(wheel), true, ((int)Mathf.Floor(rotation)));

        return newMove;
    }


    private int WheelToInt(Transform wheel)
    {
        for (int i = 0; i < AIWheels.Length; i++)
        {
            if (AIWheels[i] == wheel)
            {
                return i;
            }
        }

        return 0;
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
