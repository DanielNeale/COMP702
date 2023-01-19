using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownfallAI : MonoBehaviour
{
    [SerializeField]
    private GameController gameCont;
    [SerializeField]
    private Transform[] AIWheels;
    [SerializeField]
    private Transform[] exits;

    private List<PossibleMoves> moveList = new List<PossibleMoves>();

    /// <summary>
    /// At the start of the game a data set is made of all sensible moves
    /// </summary>
    private void Start()
    {
        // gets a list of all the slots
        List<Transform> slots = new List<Transform>();

        for (int i = 0; i < AIWheels.Length; i++)
        {
            for (int j = 0; j < AIWheels[i].childCount; j++)
            {
                slots.Add(AIWheels[i].GetChild(j));
            }
        }

        // calculates all moves for each slot
        for (int i = 0; i < slots.Count; i++)
        {
            // gets all exits available to the slot
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
            
            // calculates the angle for each exit
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

                // adds the new angle and the new angle with a full rotation
                angles.Add(newAngle);
                angles.Add(newAngle + 360);
            }

            // formats and adds all options to the data set
            PossibleMoves newMove = new PossibleMoves();
            float[] newOptions = angles.ToArray();

            newMove.SetMoves(slots[i], newOptions);
            moveList.Add(newMove);
        }

        // Set wheels and start game
        gameCont.SetWheels();
    }


    /// <summary>
    /// Called when the ai needs to make a move
    /// </summary>
    /// <returns> returns the ai movement decision </returns>
    public Rotation Move()
    {
        Rotation newMove = new Rotation();

        float highScore = -1;

        // runs through all options and evaluates
        for (int i = 0; i < moveList.Count; i++)
        {
            int moveScore = 0;

            PossibleMoves tryMove = moveList[i];
            Transform thisSlot = tryMove.GetSlot();
            Transform wheel = thisSlot.parent;
            float[] rotations = tryMove.GetMoves();
            
            // gets angle slot is at
            float slotOffset = Vector3.Angle(Vector3.up, thisSlot.position - wheel.position);

            if (wheel.position.x < thisSlot.position.x)
            {
                slotOffset = 360 - slotOffset;
            }

            // runs through the rotations for the slot
            for (int j = 0; j < rotations.Length; j++)
            {
                // the attempted rotation
                float thisRotation = rotations[j] - wheel.rotation.z;
                // the end rotation
                float rotatedPos = slotOffset + thisRotation;

                if (rotatedPos > 360)
                {
                    rotatedPos -= 360;
                }

                // reward move if counter is moved down
                if (rotatedPos > 90 && rotatedPos < 270 && thisSlot.childCount > 0)
                {
                    moveScore += 2;
                }

                // reward move if slot with no counter is moved up
                else if (rotatedPos < 90 && rotatedPos > 270 && thisSlot.childCount == 0)
                {
                    moveScore += 1;
                }

                // sets move if it's best found
                if (moveScore > highScore)
                {
                    highScore = moveScore;
                    newMove.Set(WheelToInt(wheel), true, ((int)Mathf.Floor(thisRotation / 6)));
                }
            } 
        }

        return newMove;
    }


    /// <summary>
    /// Gets the index of the wheel
    /// </summary>
    /// <param name="wheel"> wheel to be indexed </param>
    /// <returns> the wheel's index </returns>
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


/// <summary>
/// Holds the data of possible moves
/// </summary>
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

    public float[] GetMoves()
    {
        return options;
    }
}
