using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the blocker toggle
/// </summary>
public class Blockers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blocks;
    private bool active = true;


    private void Start()
    {
        ToggleBlocks();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleBlocks();
        }
    }


    private void ToggleBlocks()
    {
        active = !active;

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].SetActive(active);
        }
    }
}
