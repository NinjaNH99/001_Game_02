using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 65.0f; // 83.0
    private const int RESETDATA = 4;

    public GameObject rowPrefab;

    private GameController gameContr;
    // List of rows
    protected List<GameObject> listRows = new List<GameObject>();
    // List of teleports
    public List<GameObject> listTelep = new List<GameObject>();
    // List of teleports
    public List<Square_Line> listSquareLine = new List<Square_Line>();

    // Max obj 
    [HideInInspector]
    public int LSQ1MAX, LBLMAX, LBNMAX , SQBON, LSQLINE, LBOS, nrRowsInGame;
    private int resSQ1Max, resBLMAX, resBNMAX, resSQBON, resSQLINE, resBOS;

    private float curPosY;
    private float desiredPosition;

    private void Awake()
    {
        gameContr = GameController.Instance;
        LSQ1MAX = 0; LBLMAX = 3; LBNMAX = 0; SQBON = 3; LSQLINE = 1; LBOS = 0;
        resSQ1Max = resBLMAX = resBNMAX = resSQBON = resSQLINE = 0; resBOS = 20;
        curPosY = 0;
        desiredPosition = -130.0f;
    }

    private void Start()
    {
        GenerateRow();
        GenerateRow();
        GenerateRow();
        GenerateRow();
    }

    public void GenerateRow()
    {
        RotateSqLine();
        CheckRowsNull();
        if (CheckData())
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, desiredPosition + curPosY);
            GameObject go_row = Instantiate(rowPrefab, this.transform) as GameObject;
            go_row.GetComponent<Row>().SpawnCont(LBLMAX, LSQ1MAX, LSQLINE, LBNMAX, SQBON);
            listRows.Add(go_row);
            go_row.GetComponent<RectTransform>().anchoredPosition = Vector2.down * curPosY;
            curPosY -= DISTANCE_BETWEEN_BLOCKS;
        }

    }

    private void CheckRowsNull()
    {
        for (int i = 0; i < listRows.Count; i++)
        {
            if(!listRows[i].GetComponent<Row>().DeSpawn())
            {
                listRows.RemoveAt(i);
            }
        }
    }

    private void RotateSqLine()
    {
        for (int i = 0; i < listSquareLine.Count; i++)
        {
            listSquareLine[i].RotateSquare();
        }
    }

    private bool CheckData()
    {
        if (LSQ1MAX <= 0)
        {
            resSQ1Max++;
            if(resSQ1Max >= RESETDATA)
            {
                LSQ1MAX = 1;
                resSQ1Max = 0;
            }
        }
        if (LBLMAX <= 0)
        {
            resBLMAX++;
            if (resSQ1Max >= RESETDATA - 1)
            {
                if(gameContr.score_Rows - gameContr.amountBalls > 3)
                    LBLMAX = 4;
                else
                    LBLMAX = 1;
                resBLMAX = 0;
            }
        }
        if (LBNMAX <= 0)
        {
            resBNMAX++;
            if (resBNMAX >= RESETDATA)
            {
                LBNMAX = 1;
                resBNMAX = 0;
            }
        }
        if (SQBON <= 0)
        {
            resSQBON++;
            if (resSQBON >= RESETDATA - 1)
            {
                SQBON = 4;
                resSQBON = 0;
            }
        }
        if (LSQLINE <= 0)
        {
            resSQLINE++;
            if (resSQLINE >= RESETDATA)
            {
                LSQLINE = 1;
                resSQLINE = 0;
            }
        }
        if(LBOS <= 0)
        {
            resBOS++;
            if(resBOS >= RESETDATA * 5)
            {
                LBOS = 1;
                resBOS = 0;
            }
        }

        return true;
    }

    public void NrBlocksInGame()
    {
        nrRowsInGame = listRows.Count;
        //Debug.Log(nrRowsInGame);
        if (nrRowsInGame <= 0)
        {
            Bonus.bonus_02++;
            Bonus.Instance.AddBonus_02();
        }
    }

}
