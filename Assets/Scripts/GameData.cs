﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoSingleton<GameData>
{
    public static int amountBalls = 1, score_Rows = 1, maxScore = 1, nrRows = 0, maxBonus_01 = 0, ballBomb = 0;
    public static float posXBall = 0;
    public static bool restartGame = false;

    public static int[,] levelMap = new int[10, 9];
    //public static List<int[]> levelMap = new List<int[]>();

    public static bool loadDataDone = false;
    private static bool restartScene = false;

    private void Awake()
    {
        Time.timeScale = 1;
        loadDataDone = false;
        levelMap = new int[10, 9];

        float[,] loadedData = SaveLoadManager.LoadData();
        amountBalls = (int)loadedData[0, 0];
        score_Rows = (int)loadedData[0, 1];
        maxScore = (int)loadedData[0, 2];
        if (loadedData[0, 3] >= 10)
            nrRows = 8;
        else
            nrRows = (int)loadedData[0, 3];
        maxBonus_01 = (int)loadedData[0, 4];
        ballBomb = (int)loadedData[0, 5];
        posXBall = loadedData[0, 6];

        if (loadedData[0, 7] == 1)
        {
            restartGame = false;
            for (int i = 1; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                    levelMap[i-1,j] = (int)loadedData[i, j];
            }
        }
        else
            restartGame = true;

        loadDataDone = true;

        if (restartScene)
            restartScene = false;
    }

    private void OnApplicationQuit()
    {
        if (restartScene)
        {
            return;
        }
        SaveLoadManager.SaveDataCloseApp();
    }

    private void OnApplicationPause(bool pause)
    {
        if (restartScene)
        {
            return;
        }
        SaveLoadManager.SaveDataCloseApp();
    }

    public static void SaveDataRestart()
    {
        restartScene = true;
        SaveLoadManager.SaveDataRestartScene();
    }

    public static void LoseGameSaveData()
    {
        restartScene = true;
        SaveLoadManager.SaveDataLoseGame();
    }

    public static void ResetData()
    {
        restartScene = true;
        SaveLoadManager.ResetData();
    }
}
