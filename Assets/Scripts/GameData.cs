using System.Collections.Generic;
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

    //public static int amountBalls { get; set; }
    //public static int score_Rows { get; set; }
    //public static int maxScore { get; set; }
    //public static int nrRows { get; set; }
    //public static int maxBonus_01 { get; set; }
    //public static int ballBomb { get; set; }
    //public static float posXBall { get; set; }
    #endregion

    public static Queue<ObjInfo[]> levelMap = new Queue<ObjInfo[]>();
    //public static Queue<ObjInfo[]> levelMap = new Queue<ObjInfo[]>();

    public static bool restartGame = false;
    public static bool loadDataDone = false;

    private static bool restartScene = false;
    public static bool loadData = true;

    private void Awake()
    {
        Time.timeScale = 1;
        loadDataDone = false;
        loadData = true;

        levelMap = new Queue<ObjInfo[]>();
        //levelMap = new Queue<ObjInfo[]>();

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

        //UpdateData();

        loadDataDone = true;

        if (restartScene)
            restartScene = false;

    }

    //private void Start()
    //{
    //    EventManager.EvUpdateDataM += UpdateData;
    //}

    //public static void UpdateData()
    //{
    //    amountBalls = amountBallsTemp;
    //    score_Rows = score_RowsTemp;
    //    maxScore = maxScoreTemp;
    //    nrRows = nrRowsTemp;
    //    maxBonus_01 = maxBonus_01Temp;
    //    ballBomb = ballBombTemp;
    //    posXBall = posXBallTemp;
    //    levelMap = levelMapTemp;

    //    Debug.Log("UpdateData()");
    //}

    private void OnApplicationQuit()
    {
        if (restartScene)
            return;
        if (loadData)
            SaveLoadManager.SaveDataCloseApp();
    }

    private void OnApplicationPause(bool pause)
    {
        if (restartScene)
            return;
        if (loadData)
            SaveLoadManager.SaveDataCloseApp();
    }

    public static void SaveDataRestart()
    {
        restartScene = true;
        if (loadData)
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
