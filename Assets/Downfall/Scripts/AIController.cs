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


    public void StartMove()
    {
        MakeMove(ai.Move());
    }


    public void MakeMove(Rotation newMove)
    {
        Transform target = wheels[newMove.Wheel()];
        StartCoroutine(RotateWheel(target, newMove.Clockwise(), newMove.Rotations()));
    }

    private IEnumerator RotateWheel(Transform wheel, bool clockwise, int rotations)
    {
        for (int i = 0; i < rotations; i++)
        {
            int snap = wheel.GetComponent<WheelMovement>().GetSnapAngle();
            int dir = 1;

            if (!clockwise)
            {
                dir = -1;
            }

            wheel.Rotate(new Vector3(0, 0, snap * dir));
            Transform pairWheel = wheel.GetComponent<WheelMovement>().GetPair();
            pairWheel.Rotate(new Vector3(0, 0, snap * dir));

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        gameCont.ToggleButtons(true);
    }
}


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
