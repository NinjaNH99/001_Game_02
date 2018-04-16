using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadManager
{
    public static void SaveDataCloseApp()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Create);

        GameDataForFile newData = new GameDataForFile();

        bf.Serialize(stream, newData.CloseApp());
        stream.Close();

    }

    public static void SaveDataRestartScene()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Create);

        GameDataForFile newData = new GameDataForFile();

        bf.Serialize(stream, newData.RestartGameScene());
        stream.Close();

        LevelLoader.Instance.LoadLevel("Game");
    }

    public static void SaveDataLoseGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Create);

        GameDataForFile newData = new GameDataForFile();

        bf.Serialize(stream, newData.LoseGameSaveData());
        stream.Close();
    }

    public static void ResetData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Create);

        GameDataForFile data = new GameDataForFile();

        bf.Serialize(stream, data.ResetGameDataF());
        stream.Close();

        LevelLoader.Instance.LoadLevel("Game");
    }

    public static float[] LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Data.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Open);

            GameDataForFile data = bf.Deserialize(stream) as GameDataForFile;

            stream.Close();

            Debug.Log(" Data load:");
            //Debug.Log(" data[0] amountBalls : " + data.data[0]);
            //Debug.Log(" data[1] score_Rows  : " + data.data[1]);
            //Debug.Log(" data[2] maxScore    : " + data.data[2]);
            //Debug.Log(" data[3] nrRows      : " + data.data[3]);
            //Debug.Log(" data[4] maxBonus_01 : " + data.data[4]);
            //Debug.Log(" data[5] ballBomb : " + data.data[5]);
            Debug.LogWarning(" data[6] posXBall : " + data.data[6]);

            return data.data;
        }
        else
        {
            GameDataForFile newData = new GameDataForFile();

            return newData.ResetGameData();
        }
    }
}

[Serializable]
public class GameDataForFile
{
    public float[] data = new float[7];

    public GameDataForFile()
    {   }

    public GameDataForFile RestartGameScene()
    {
        data[0] = 1;
        data[1] = 1;
        if (GameData.maxScore > GameData.score_Rows)
            data[2] = GameData.maxScore;
        else
            data[2] = GameData.score_Rows;
        data[3] = 1;
        data[4] = Bonus.Instance.Bonus_01;
        data[5] = 0;
        data[6] = 0;

        Debug.LogWarning(" Data restart:");
        //Debug.LogWarning(" data[0] amountBalls : " + data[0]);
        //Debug.LogWarning(" data[1] score_Rows  : " + data[1]);
        //Debug.LogWarning(" data[2] maxScore    : " + data[2]);
        //Debug.LogWarning(" data[3] nrRows      : " + data[3]);
        //Debug.LogWarning(" data[4] maxBonus_01 : " + data[4]);
        //Debug.LogWarning(" data[5] ballBomb : " + data.data[5]);
        Debug.LogWarning(" data[6] posXBall : " + data[6]);

        return this;
    }

    public GameDataForFile CloseApp()
    {
        data[0] = GameData.amountBalls;

        if (GameData.score_Rows > data[1])
            data[1] = GameData.score_Rows;

        if (GameData.maxScore > data[2])
            data[2] = GameData.maxScore;

        data[3] = GameData.nrRows;
        data[4] = GameData.maxBonus_01;
        data[5] = GameData.ballBomb;
        data[6] = GameData.posXBall;

        Debug.LogWarning(" Data create close app:");
        //Debug.LogWarning(" data[0] amountBalls : " + data[0]);
        //Debug.LogWarning(" data[1] score_Rows  : " + data[1]);
        //Debug.LogWarning(" data[2] maxScore    : " + data[2]);
        //Debug.LogWarning(" data[3] nrRows      : " + data[3]);
        //Debug.LogWarning(" data[4] maxBonus_01 : " + data[4]);
        //Debug.LogWarning(" data[5] ballBomb : " + data[5]);
        Debug.LogWarning(" data[6] posXBall : " + data[6]);

        return this;
    }

    public GameDataForFile LoseGameSaveData()
    {
        data[0] = 1;
        data[1] = 1;
        if (GameData.maxScore > GameData.score_Rows)
            data[2] = GameData.maxScore;
        else
            data[2] = GameData.score_Rows;
        data[3] = 1;
        data[4] = Bonus.Instance.Bonus_01;
        data[5] = 0;
        data[6] = 0;

        Debug.LogWarning(" Data lose game:");
        //Debug.LogWarning(" data[0] amountBalls : " + data[0]);
        //Debug.LogWarning(" data[1] score_Rows  : " + data[1]);
        //Debug.LogWarning(" data[2] maxScore    : " + data[2]);
        //Debug.LogWarning(" data[3] nrRows      : " + data[3]);
        //Debug.LogWarning(" data[4] maxBonus_01 : " + data[4]);
        //Debug.LogWarning(" data[5] ballBomb : " + data.data[5]);
        Debug.LogWarning(" data[6] posXBall : " + data[6]);

        return this;
    }

    public float[] ResetGameData()
    {
        data[0] = 1;
        data[1] = 1;
        data[2] = 1;
        data[3] = 0;
        data[4] = 0;
        data[5] = 2;
        data[6] = 0;

        return data;
    }

    public GameDataForFile ResetGameDataF()
    {
        data[0] = 1;
        data[1] = 1;
        data[2] = 1;
        data[3] = 0;
        data[4] = 0;
        data[5] = 2;
        data[6] = 0;

        return this;
    }

}