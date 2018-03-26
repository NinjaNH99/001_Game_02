using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void RotateSquare();
    public static event RotateSquare RotateSquareMethods = null;

    private void Awake()
    {
        RotateSquareMethods = null;
    }

    public static void LevelMoveDown()
    {

        if (RotateSquareMethods != null)
            RotateSquareMethods();
    }

}
