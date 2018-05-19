using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoSingleton<GameData>
{
    #region Proprety
    public static int amountBalls { get; set; }
    public static int score_Rows { get; set; }
    public static int maxScore { get; set; }
    public static int nrRows { get; set; }
    public static int maxBonus_01 { get; set; }
    public static int ballBomb { get; set; }
    public static float posXBall { get; set; }

    public static int amountBallsOrg { get; set; }
    public static int score_RowsOrg { get; set; }
    public static int maxScoreOrg { get; set; }
    public static int nrRowsOrg { get; set; }
    public static int maxBonus_01Org { get; set; }
    public static int ballBombOrg { get; set; }
    public static float posXBallOrg { get; set; }
    #endregion

    public static Queue<ObjInfo[]> levelMap = new Queue<ObjInfo[]>();
    public static Queue<ObjInfo[]> levelMapOrg = new Queue<ObjInfo[]>();

    public static bool restartGame = false;
    public static bool loadDataDone = false;

    private static bool restartScene = false;
    //public static bool loadData = true;

    private void Awake()
    {
        Time.timeScale = 1;
        loadDataDone = false;
        //loadData = true;

        levelMap = new Queue<ObjInfo[]>();
        levelMapOrg = new Queue<ObjInfo[]>();

        ObjInfo[,] loadedData = SaveLoadManager.LoadData();
        amountBalls = (int)loadedData[0, 0].saveData;
        score_Rows = (int)loadedData[0, 1].saveData;
        maxScore = (int)loadedData[0, 2].saveData;
        nrRows = (int)loadedData[0, 3].saveData;
        maxBonus_01 = (int)loadedData[0, 4].saveData;
        ballBomb = (int)loadedData[0, 5].saveData;
        posXBall = loadedData[0, 6].saveData;

        if (loadedData[0, 7].saveData == 1)
        {
            restartGame = false;
            for (int i = 0; i < nrRows; i++)
            {
                ObjInfo[] temp = new ObjInfo[9];
                for (int j = 0; j < 9; j++)
                {
                    temp[j].type = loadedData[i + 1, j].type;
                    temp[j].hp = loadedData[i + 1, j].hp;
                    temp[j].shield = loadedData[i + 1, j].shield;
                    temp[j].shieldON = loadedData[i + 1, j].shieldON;
                }
                levelMap.Enqueue(temp);
            }
        }
        else
            restartGame = true;

        UpdateData();

        loadDataDone = true;

        if (restartScene)
            restartScene = false;

    }

    //private void Start()
    //{
    //    EventManager.EvUpdateDataSavedM += UpdateData;
    //}

    public static void UpdateData()
    {
        amountBallsOrg = amountBalls;
        score_RowsOrg = score_Rows;
        maxScoreOrg = maxScore;
        nrRowsOrg = nrRows;
        maxBonus_01Org = maxBonus_01;
        ballBombOrg = ballBomb;
        posXBallOrg = posXBall;

        levelMapOrg.Clear();
        foreach (ObjInfo[] item in levelMap)
        {
            ObjInfo[] tmp = new ObjInfo[9];
            for (int i = 0; i < 9; i++)
                tmp[i] = item[i];

            levelMapOrg.Enqueue(tmp);
        }

    }

    private void OnApplicationQuit()
    {
        if (restartScene)
            return;
        SaveLoadManager.SaveDataCloseApp();
    }

    private void OnApplicationPause(bool pause)
    {
        if (restartScene)
            return;
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
