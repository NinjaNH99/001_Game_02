using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoSingleton<GameData>
{
    public static int amountBalls = 1, score_Rows = 1, maxScore = 1, nrRows = 0, maxBonus_01 = 0, ballBomb = 0;
    public static float posXBall = 0;
    public static bool loadDataDone = false;

    private static bool restartScene = false;

    private void Awake()
    {
        Time.timeScale = 1;
        loadDataDone = false;

        float[] loadedData = SaveLoadManager.LoadData();
        amountBalls = (int)loadedData[0];
        score_Rows = (int)loadedData[1];
        maxScore = (int)loadedData[2];
        if (loadedData[3] >= 10)
            nrRows = 8;
        else
            nrRows = (int)loadedData[3];
        maxBonus_01 = (int)loadedData[4];
        ballBomb = (int)loadedData[5];
        posXBall = loadedData[6];

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
        #region Mobile Inputs
        if (restartScene)
        {
            return;
        }
        SaveLoadManager.SaveDataCloseApp();
        #endregion
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
