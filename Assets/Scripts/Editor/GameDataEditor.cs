using UnityEngine;
using UnityEditor;
using System.Linq;

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
        GUILayout.Label("Data:");
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
        int row = GameData.nrRows, column = 9;
        for (int i = 0; i < row; i++)
        {
            ObjInfo[] temp = GameData.levelMap.ElementAt(i);

            res = res + "[ ";
            for (int j = 0; j < column; j++)
            {
                if(temp[j].type != 10)
                    res = res + temp[j].type + " ";
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
        string res = "";
        ObjInfo[,] loadedData = SaveLoadManager.LoadData();
        int row = (int)loadedData[0, 3].saveData, column = 9;
        for (int i = 0; i < row; i++)
        {
            res = res + "[ ";
            for (int j = 0; j < column; j++)
            {
                if (loadedData[i + 1,j].type != 10)
                    res = res + loadedData[i + 1, j].type + " ";
                else
                    res = res + "X" + " ";
            }
            res = res.TrimEnd(' ');
            res = res + " ] ";
        }

        saveLevelMap = res;
    }
}
