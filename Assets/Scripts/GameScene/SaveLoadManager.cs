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

    public static float[,] LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Data.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Open);

            GameDataForFile data = bf.Deserialize(stream) as GameDataForFile;

            stream.Close();

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
    public float[,] data = new float[11,9];

    public GameDataForFile()
    {   }

    public GameDataForFile RestartGameScene()
    {
        data[0,0] = 1;
        data[0,1] = 1;
        if (GameData.maxScore > GameData.score_Rows)
            data[0, 2] = GameData.maxScore;
        else
            data[0, 2] = GameData.score_Rows;
        data[0, 3] = 1;
        data[0, 4] = Bonus.Instance.Bonus_01;
        data[0, 5] = 0;
        data[0, 6] = 0;
        data[0, 7] = 0;

        return this;
    }

    public GameDataForFile CloseApp()
    {
        data[0, 0] = GameData.amountBalls;

        if (GameData.score_Rows > data[0, 1])
            data[0, 1] = GameData.score_Rows;

        if (GameData.maxScore > data[0, 2])
            data[0, 2] = GameData.maxScore;

        data[0, 3] = GameData.nrRows;
        data[0, 4] = GameData.maxBonus_01;
        data[0, 5] = GameData.ballBomb;
        data[0, 6] = GameData.posXBall;
        data[0, 7] = 1;

        for (int i = 1; i < 10; i++)
            for (int j = 0; j < 9; j++)
                data[i, j] = GameData.levelMap[i - 1, j];

        return this;
    }

    public GameDataForFile LoseGameSaveData()
    {
        data[0, 0] = 1;
        data[0, 1] = 1;
        if (GameData.maxScore > GameData.score_Rows)
            data[0, 2] = GameData.maxScore;
        else
            data[0, 2] = GameData.score_Rows;
        data[0, 3] = 1;
        data[0, 4] = Bonus.Instance.Bonus_01;
        data[0, 5] = 0;
        data[0, 6] = 0;
        data[0, 7] = 0;

        return this;
    }

    public float[,] ResetGameData()
    {
        data[0, 0] = 1;
        data[0, 1] = 1;
        data[0, 2] = 1;
        data[0, 3] = 0;
        data[0, 4] = 0;
        data[0, 5] = 2;
        data[0, 6] = 0;
        data[0, 7] = 0;

        return data;
    }

    public GameDataForFile ResetGameDataF()
    {
        data[0, 0] = 1;
        data[0, 1] = 1;
        data[0, 2] = 1;
        data[0, 3] = 0;
        data[0, 4] = 0;
        data[0, 5] = 2;
        data[0, 6] = 0;
        data[0, 7] = 0;

        return this;
    }

}