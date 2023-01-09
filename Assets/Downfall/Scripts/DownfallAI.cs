using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownfallAI : MonoBehaviour
{
    public Rotation Move()
    {
        Rotation newMove = new Rotation();

        newMove.Set(0, false, 40);

        return newMove;
    }
}
