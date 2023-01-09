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
        wheelButtons = new Button[wheels.Length];

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelButtons[i] = wheels[i].GetComponent<Button>();
            wheels[i].GetComponent<WheelMovement>().SetGameCont(this);
        }
    }


    private void ToggleTurn()
    {     
        playerOneTurn = !playerOneTurn;
        print("player one turn = " + playerOneTurn);

        if (!playerOneTurn)
        {
            ToggleButtons(false);
            aiCont.StartMove();
        }
    }


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


    public void ToggleButtons(bool enabled)
    {
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
}
