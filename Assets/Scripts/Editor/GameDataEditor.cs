using UnityEngine;
using UnityEditor;

public class GameDataEditor : EditorWindow
{
    private string levelMapCurrent = "", saveLevelMap = ""; 

    [MenuItem("Window/GameData")]
    public static void ShowWindow()
    {
        GetWindow<GameDataEditor>("GameData");
    }

    private void OnGUI()
    {
        UpdateData2Current();
        UpdateDataSave();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Update"))
        {
            UpdateData2Current();
        }
        if(GUILayout.Button("Reset Data"))
        {
            SaveLoadManager.ResetData();
        }
        if(GUILayout.Button("Show Save Data"))
        {
            UpdateDataSave();
        }

        
        GUILayout.BeginArea(new Rect(10f, 50f, 123.5f, 1000f));

        GUILayout.Label("Current level map:");
        GUILayout.TextArea(levelMapCurrent);

        GUILayout.EndArea();


        GUILayout.BeginArea(new Rect(170f, 50f, 123.5f, 1000f));        // 123.5f

        GUILayout.Label("Object type:");
        GUILayout.TextField("1: square");
        GUILayout.TextField("2: ball");
        GUILayout.TextField("3: bonus");
        GUILayout.TextField("4: teleportIn");
        GUILayout.TextField("5: teleportOut");
        GUILayout.TextField("6: square_Bonus");
        GUILayout.TextField("7: square_Line");
        GUILayout.TextField("8: space");
        GUILayout.TextField("9: square_Boss");

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(330f, 50f, 150f, 1000f));        // 123.5f

        GUILayout.Label("Data save:");
        GUILayout.TextField("amountBalls:" + "\t" + GameData.amountBalls);
        GUILayout.TextField("score_Rows:" + "\t" + GameData.score_Rows);
        GUILayout.TextField("maxScore:  " + "\t" + GameData.maxScore);
        GUILayout.TextField("nrRows:        " + "\t" + GameData.nrRows);
        GUILayout.TextField("maxBonus_01:" + "\t" + GameData.maxBonus_01);
        GUILayout.TextField("ballBomb:  " + "\t" + GameData.ballBomb);
        GUILayout.TextField("posXBall:      " + "\t" + GameData.posXBall);

        GUILayout.EndArea();

        GUILayout.EndHorizontal();


        GUILayout.BeginArea(new Rect(10f, 250f, 123.5f, 1000f));

        GUILayout.Label("Saved level map:");
        GUILayout.TextArea(saveLevelMap);

        GUILayout.EndArea();
    }

    private void UpdateData2Current()
    {
        string res = "";
        int row = 10, column = 9;
        for (int i = 0; i < row; i++)
        {
            res = res + "[ ";
            for (int j = 0; j < column; j++)
            {
                if (GameData.levelMap[i, j] != 10)
                    res = res + GameData.levelMap[i, j] + " ";
                else
                    res = res + "X" + " ";
            }
            res = res.TrimEnd(' ');
            res = res + " ] ";
        }

        levelMapCurrent = res;
    }

    private void UpdateDataSave()
    {
        float[,] loadedData = SaveLoadManager.LoadData();
        string res = "";
        int row = 10, column = 9;
        for (int i = 0; i < row; i++)
        {
            res = res + "[ ";
            for (int j = 0; j < column; j++)
            {
                if ((int)loadedData[i + 1, j] != 10)
                    res = res + (int)loadedData[i + 1, j] + " ";
                else
                    res = res + "X" + " ";
            }
            res = res.TrimEnd(' ');
            res = res + " ] ";
        }

        saveLevelMap = res;
    }
}
