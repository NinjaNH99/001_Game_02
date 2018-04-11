using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadManager
{
    public static void SaveDataCloseApp(GameController gameController)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Create);

        GameData data = new GameData(gameController, 1);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveDataRestartApp(GameController gameController)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Create);

        GameData data = new GameData(gameController, 2);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static void ResetData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Create);

        GameData data = new GameData(null, 3);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static int[] LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Data.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/Data.sav", FileMode.Open);

            GameData data = bf.Deserialize(stream) as GameData;

            stream.Close();
            return data.data;
        }
        else
        {
            GameData data = new GameData(null, 3);
            return data.data;
        }
    }
}

[Serializable]
public class GameData
{
    public int[] data;

    public GameData(GameController gameController, int mode)       // mode: true - close app ; false restart scene
    {
        data = new int[6];
        switch (mode)
        {
            case 1:
                {
                    data[0] = gameController.amountBalls;
                    data[1] = gameController.score_Rows;
                    data[2] = gameController.maxScore;
                    data[3] = gameController.nrBallINeed;
                    data[4] = gameController.nrRows;
                    data[5] = gameController.nrBonus_01;
                    break;
                }
            case 2:
                {
                    data[0] = 1;
                    data[1] = 1;
                    data[2] = gameController.maxScore;
                    data[3] = 1;
                    data[4] = 1;
                    data[5] = gameController.nrBonus_01;
                    break;
                }
            case 3:
                {
                    data[0] = 1;
                    data[1] = 1;
                    data[2] = 1;
                    data[3] = 1;
                    data[4] = 1;
                    data[5] = 0;
                    break;
                }
            default:
                break;
        }
       
    }
}

