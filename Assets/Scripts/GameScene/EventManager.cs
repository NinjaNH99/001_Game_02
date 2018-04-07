using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EvLevelMoveDown();
    public static event EvLevelMoveDown EvMethods = null;

    private void Awake()
    {
        EvMethods = null;
    }

    public static void LevelMoveDown()
    {

        if (EvMethods != null)
            EvMethods();
    }

}
