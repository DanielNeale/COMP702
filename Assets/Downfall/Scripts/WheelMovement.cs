using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMovement : MonoBehaviour
{
    private GameController gameCont;

    private RectTransform thisWheel;
    [SerializeField]
    private RectTransform pairWheel;

    [SerializeField]
    private int snapAngle = 30;
    [SerializeField]
    private float sensitivity = 30;

    private float mouseStartX;
    private float thisWheelZ;
    private float pairWheelZ;
    private bool rotating = false;
    private bool lockRotation = false;
    private bool clockwise;
    private int lastAngle;


    private void Start()
    {
        thisWheel = GetComponent<RectTransform>();
    }


    private void Update()
    {
        if (rotating)
        {
            // tracks the number of rotations by mouse x position
            int rotations = Mathf.FloorToInt((Input.mousePosition.x - mouseStartX) / sensitivity);

            // stops player from rotating in 2 directions
            if (lockRotation)
            {
                if (clockwise && rotations < lastAngle)
                {
                    rotations = lastAngle;
                }

                if (!clockwise && rotations > lastAngle)
                {
                    rotations = lastAngle;
                }
            }

            lastAngle = rotations;

            // rotates both player and ai wheels
            thisWheel.rotation = Quaternion.Euler(0, 0, thisWheelZ - (rotations * snapAngle));
            pairWheel.rotation = Quaternion.Euler(0, 0, pairWheelZ + (rotations * snapAngle));

            // locks rotation in one direction if the player starts rotating
            if (!lockRotation)
            {
                if ((Input.mousePosition.x - mouseStartX) / sensitivity > 1)
                {
                    lockRotation = true;
                    clockwise = true;
                }

                else if ((Input.mousePosition.x - mouseStartX) / sensitivity < -1)
                {
                    lockRotation = true;
                    clockwise = false;
                }
            }

            // ends player turn and stops rotating
            if (Input.GetMouseButton(0))
            {
                rotating = false;
                lockRotation = false;
                lastAngle = 0;
                gameCont.ToggleButtons(true);
            }
        }
    }


    /// <summary>
    /// Called by a button to start a rotation
    /// </summary>
    public void StartRotation()
    {
        if (rotating == false)
        {
            gameCont.ToggleButtons(false);
            rotating = true;
            mouseStartX = Input.mousePosition.x;
            thisWheelZ = thisWheel.eulerAngles.z;
            pairWheelZ = pairWheel.eulerAngles.z;
        }
    }


    public void SetGameCont(GameController newGameCont)
    {
        gameCont = newGameCont;
    }


    public int GetSnapAngle()
    {
        return snapAngle;
    }


    public Transform GetPair()
    {
        return pairWheel;
    }
}
