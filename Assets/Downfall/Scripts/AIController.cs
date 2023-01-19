using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private DownfallAI ai;
    [SerializeField]
    private GameController gameCont;
    
    [SerializeField]
    private Transform[] wheels;


    /// <summary>
    /// Triggers ai move
    /// </summary>
    public void StartMove()
    {
        MakeMove(ai.Move());
    }


    /// <summary>
    /// Actions the ai move
    /// </summary>
    /// <param name="newMove"> the ai move </param>
    public void MakeMove(Rotation newMove)
    {
        Transform target = wheels[newMove.Wheel()];
        StartCoroutine(RotateWheel(target, newMove.Clockwise(), newMove.Rotations()));
    }

    /// <summary>
    /// Rottes the wheel after an ai turn
    /// </summary>
    /// <param name="wheel"> the wheel to move </param>
    /// <param name="clockwise"> the direction to move </param>
    /// <param name="rotations"> how many rotations </param>
    /// <returns></returns>
    private IEnumerator RotateWheel(Transform wheel, bool clockwise, int rotations)
    {
        // rotates one snap angle every frame
        for (int i = 0; i < rotations; i++)
        {
            int snap = wheel.GetComponent<WheelMovement>().GetSnapAngle();
            int dir = 1;

            if (!clockwise)
            {
                dir = -1;
            }

            // rotates both the player and ai wheel
            wheel.Rotate(new Vector3(0, 0, snap * dir));
            Transform pairWheel = wheel.GetComponent<WheelMovement>().GetPair();
            pairWheel.Rotate(new Vector3(0, 0, snap * dir));

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        // enables player turn when done
        gameCont.ToggleButtons(true);
    }
}


/// <summary>
/// stores rotation information
/// </summary>
public class Rotation
{
    private int wheel;
    private bool clockwise;
    private int rotations;

    public void Set(int newWheel, bool newClockwise, int newRotations)
    {
        wheel = newWheel;
        clockwise = newClockwise;
        rotations = newRotations;
    }

    public int Wheel()
    {
        return wheel;
    }

    public bool Clockwise()
    {
        return clockwise;
    }

    public int Rotations()
    {
        return rotations;
    }
}
