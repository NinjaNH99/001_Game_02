using System;
using System.IO;
using System.Linq;
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

    public static ObjInfo[,] LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Data.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Open);

            GameDataForFile data = bf.Deserialize(stream) as GameDataForFile;

            stream.Close();

            return data.Data;
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
    private ObjInfo[,] data = new ObjInfo[12, 9];

    public ObjInfo[,] Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }

    private ObjInfo SetData(int type, float data, int hp, int shield, int shieldON)
    {
        ObjInfo newData = new ObjInfo
        {
            type = type,
            saveData = data,
            hp = hp,
            shield = shield,
            shieldON = shieldON
        };
        return newData;
    }

    public GameDataForFile RestartGameScene()
    {
        data[0,0] = SetData(-1, 1, -1, -1, 1);
        data[0,1] = SetData(-1, 1, -1, -1, 1); 
        if (GameData.maxScore > GameData.score_Rows)
            data[0, 2] = SetData(-1, GameData.maxScore, -1, -1, 1);
        else
            data[0, 2] = SetData(-1, GameData.score_Rows, -1, -1, 1);
        data[0, 3] = SetData(-1, 0, -1, -1, 1);
        data[0, 4] = SetData(-1, Bonus.Instance.Bonus_01, -1, -1, 1);
        data[0, 5] = SetData(-1, 0, -1, -1, 1);
        data[0, 6] = SetData(-1, 0, -1, -1, 1);
        data[0, 7] = SetData(-1, 0, -1, -1, 1);

        return this;
    }

    public GameDataForFile CloseApp()
    {
        data[0, 0] = SetData(-1, GameData.amountBalls, -1, -1, 1);

        if (GameData.score_Rows > data[0, 1].saveData)
            data[0, 1] = SetData(-1, GameData.score_Rows, -1, -1, 1);

        if (GameData.maxScore > data[0, 2].saveData)
            data[0, 2] = SetData(-1, GameData.maxScore, -1, -1, 1);

        data[0, 3] = SetData(-1, GameData.nrRows, -1, -1, 1);
        data[0, 4] = SetData(-1, GameData.maxBonus_01, -1, -1, 1);
        data[0, 5] = SetData(-1, GameData.ballBomb, -1, -1, 1);
        data[0, 6] = SetData(-1, GameData.posXBall, -1, -1, 1);
        data[0, 7] = SetData(-1, 1, -1, -1, 1);

        for (int i = 1; i <= GameData.nrRows; i++)
        {
            ObjInfo[] temp = GameData.levelMap.ElementAt(i - 1);
            for (int j = 0; j < 9; j++)
                data[i, j] = temp[j];
        }

        return this;
    }

    public GameDataForFile LoseGameSaveData()
    {
        data[0, 0] = SetData(-1, 1, -1, -1, 1);
        data[0, 1] = SetData(-1, 1, -1, -1, 1);
        if (GameData.maxScore > GameData.score_Rows)
            data[0, 2] = SetData(-1, GameData.maxScore, -1, -1, 1);
        else
            data[0, 2] = SetData(-1, GameData.score_Rows, -1, -1, 1);
        data[0, 3] = SetData(-1, 0, -1, -1, 1);
        data[0, 4] = SetData(-1, Bonus.Instance.Bonus_01, -1, -1, 1);
        data[0, 5] = SetData(-1, 0, -1, -1, 1);
        data[0, 6] = SetData(-1, 0, -1, -1, 1);
        data[0, 7] = SetData(-1, 0, -1, -1, 1);

        return this;
    }

    public ObjInfo[,] ResetGameData()
    {
        data[0, 0] = SetData(-1, 1, -1, -1, 1);
        data[0, 1] = SetData(-1, 1, -1, -1, 1);
        data[0, 2] = SetData(-1, 1, -1, -1, 1);
        data[0, 3] = SetData(-1, 0, -1, -1, 1);
        data[0, 4] = SetData(-1, 0, -1, -1, 1);
        data[0, 5] = SetData(-1, 2, -1, -1, 1);
        data[0, 6] = SetData(-1, 0, -1, -1, 1);
        data[0, 7] = SetData(-1, 0, -1, -1, 1);

        return data;
    }

    public GameDataForFile ResetGameDataF()
    {
        data[0, 0] = SetData(-1, 1, -1, -1, 1);
        data[0, 1] = SetData(-1, 1, -1, -1, 1);
        data[0, 2] = SetData(-1, 1, -1, -1, 1);
        data[0, 3] = SetData(-1, 0, -1, -1, 1);
        data[0, 4] = SetData(-1, 0, -1, -1, 1);
        data[0, 5] = SetData(-1, 2, -1, -1, 1);
        data[0, 6] = SetData(-1, 0, -1, -1, 1);
        data[0, 7] = SetData(-1, 0, -1, -1, 1);

        return this;
    }

}

[Serializable]
public struct ObjInfo
{
    public int type { get; set; }
    public float saveData { get; set; }
    public int hp { get; set; }
    public int shield { get; set; }
    public int shieldON { get; set; }
}