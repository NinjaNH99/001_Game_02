using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Level move down
    public delegate void EvLevelMove();
    public static event EvLevelMove EvMoveDownM = null;

    // Despawn Liser and Teleport
    public delegate void EvDeSpawn();
    public static event EvDeSpawn EvDeSpawnM = null;

    // Random spawn
    public delegate void EvSpawnRand();
    public static event EvSpawnRand EvSpawnRandomM = null;

    private void Awake()
    {
        EvMoveDownM = null;
        EvSpawnRandomM = null;
        EvDeSpawnM = null;
    }

    public static void StartEvMoveDown()
    {
        if (EvMoveDownM != null)
            EvMoveDownM();
    }

    public static void StartEvDeSpawn()
    {
        if (EvDeSpawnM != null)
            EvDeSpawnM();
    }

    public static void StartEvSpawn()
    {
        if (EvSpawnRandomM != null)
            EvSpawnRandomM();
    }

}
