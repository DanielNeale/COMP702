using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private bool playerOneTurn = true;
    [SerializeField]
    private AIController aiCont;

    [SerializeField]
    private GameObject[] wheels;
    [SerializeField]
    private GameObject[] aiWheels;
    private Button[] wheelButtons;

    [SerializeField]
    private Transform SideOneStart;
    [SerializeField]
    private Transform SideTwoStart;
    [SerializeField]
    private Transform SideOneEnd;
    [SerializeField]
    private Transform SideTwoEnd;


    private void Start()
    {
        // sets wheel buttons and game controller on wheels
        wheelButtons = new Button[wheels.Length];

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelButtons[i] = wheels[i].GetComponent<Button>();
            wheels[i].GetComponent<WheelMovement>().SetGameCont(this);
        }
    }


    /// <summary>
    /// Toggles who's turn it is
    /// </summary>
    private void ToggleTurn()
    {     
        playerOneTurn = !playerOneTurn;

        if (!playerOneTurn)
        {
            ToggleButtons(false);
            aiCont.StartMove();
        }
    }


    /// <summary>
    /// Checks counter number to see if anyone has won
    /// </summary>
    private void CheckWin()
    {
        if (SideOneEnd.childCount == 5)
        {
            print("Player 1 wins");
        }

        if (SideTwoEnd.childCount == 5)
        {
            print("Player 2 wins");
        }
    }


    /// <summary>
    /// Controls the buttons on the wheels
    /// </summary>
    /// <param name="enabled"> the button state </param>
    public void ToggleButtons(bool enabled)
    {
        // changes button state
        for (int i = 0; i < wheelButtons.Length; i++)
        {
            wheelButtons[i].enabled = enabled;
        }

        CheckWin();

        if (enabled)
        {
            ToggleTurn();
        }
    }


    /// <summary>
    /// Randomly sets the wheels at the start of the game
    /// </summary>
    public void SetWheels()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        }

        for (int i = 0; i < aiWheels.Length; i++)
        {
            aiWheels[i].transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        }
    }
}
