using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoSingleton<GameData>
{
    public static int amountBalls, score_Rows, maxScore, nrRows, maxBonus_01, maxBonus_02;
    public static float posXBall;
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
        maxBonus_02 = (int)loadedData[5];
        posXBall = loadedData[6];

        loadDataDone = true;
    }

    private void OnApplicationQuit()
    {
        if (restartScene)
        {
            restartScene = false;
            return;
        }
        SaveLoadManager.SaveDataCloseApp();
    }

    public static void SaveDataRestart()
    {
        restartScene = true;
        SaveLoadManager.SaveDataRestartScene();
        //LevelLoader.Instance.LoadLevel("Game");
    }

    public static void ResetData()
    {
        restartScene = true;
        SaveLoadManager.ResetData();
        //LevelLoader.Instance.LoadLevel("Game");
    }
}
