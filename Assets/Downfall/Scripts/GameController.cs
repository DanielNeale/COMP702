using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wheels;
    private Button[] wheelButtons;


    private void Start()
    {
        wheelButtons = new Button[wheels.Length];

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelButtons[i] = wheels[i].GetComponent<Button>();
            wheels[i].GetComponent<WheelMovement>().SetGameCont(this);
        }
    }


    public void ToggleButtons(bool enabled)
    {
        for (int i = 0; i < wheelButtons.Length; i++)
        {
            wheelButtons[i].enabled = enabled;
        }
    }
}
