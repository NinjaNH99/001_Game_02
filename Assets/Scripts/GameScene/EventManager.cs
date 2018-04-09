using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Level move down
    public delegate void EvLevelMove();
    public static event EvLevelMove evMoveDown = null;

    // Rotate square
    public delegate void EvRotate();
    public static event EvRotate evRotate = null;

    // Random spawn
    public delegate void EvSpawnRand();
    public static event EvSpawnRand evSpawnRand = null;
    private static int pause;

    private void Awake()
    {
        evMoveDown = null;
        evRotate = null;
        evSpawnRand = null;

        pause = 1;
    }

    public static void StartEvMoveDown()
    {
        if (evMoveDown != null)
            evMoveDown();
    }

    public static void StartEvRotate()
    { 
        if (evRotate != null)
            evRotate();
    }

    public static void StartEvSpawn()
    {
        if (pause > 0)
        {
            pause--;
            return;
        }

        pause = 1;
        if (evSpawnRand != null)
            evSpawnRand();
    }

}
