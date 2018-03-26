using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 65.0f; // 83.0
    private const int RESETDATA = 4;

    public GameObject rowPrefab;
    public Block_Boss bossObj = null;

    private GameController gameContr;
    // List of rows
    public List<GameObject> listRows = new List<GameObject>();
    // List of teleports
    public List<GameObject> listTelep = new List<GameObject>();
    // List of SquareLines
    //public List<Square_Line> listSquareLine = new List<Square_Line>();

    // Max obj 
    public bool spawnRows, spawnBoss;
    [HideInInspector]
    public int LSQ1MAX, LBLMAX, LBNMAX , SQBON, LSQLINE, nrRowsInGame;
    [HideInInspector]
    private int resSQ1Max, resBLMAX, resBNMAX, resSQBON, resSQLINE, resBOS, resSpawnRows;

    private float curPosY;
    private float desiredPosition;

    private void Awake()
    {
        gameContr = GameController.Instance;
        LSQ1MAX = 0; LBLMAX = 3; LBNMAX = 0; SQBON = 3; LSQLINE = 1;
        resSQ1Max = resBLMAX = resBNMAX = resSQBON = resSQLINE = resBOS = resSpawnRows = 0;
        curPosY = 0;
        desiredPosition = -130.0f;
        spawnRows = true;
        spawnBoss = false;
    }

    private void Start()
    {
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
            go_row.GetComponent<Row>().SpawnCont(spawnRows, spawnBoss, LBLMAX, LSQ1MAX, LSQLINE, LBNMAX, SQBON);
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
        EventManager.LevelMoveDown();
    }

    public void ApplyBallBonus(int contID, int rowID)
    {
        Debug.LogWarning("contID: " + contID + " rowID: " + rowID);

        Container[] conts = listRows[rowID].GetComponentsInChildren<Container>();
        for (int i = 0; i < conts.Length; i++)
        {
            if (conts[i].visualIndex == contID)
            {
                conts[i].GetComponentInChildren<Block>().ReceiveHit(true);
                if(conts[i + 1].GetComponentInChildren<Block>() != null)
                    conts[i + 1].GetComponentInChildren<Block>().ReceiveHit(true);
                if (conts[i - 1].GetComponentInChildren<Block>() != null)
                    conts[i - 1].GetComponentInChildren<Block>().ReceiveHit(true);
            }
        }
    }

    private bool CheckData()
    {
        if (LSQ1MAX <= 0)
        {
            resSQ1Max++;
            if (resSQ1Max >= RESETDATA)
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
                if (gameContr.score_Rows - gameContr.amountBalls > 3)
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

        resBOS++;
        if (resBOS >= ( RESETDATA * 2 ) + 2)
        {
            spawnBoss = true;
            spawnRows = false;
            resBOS = 0;
        }

        if (!spawnRows)
        {
            resSpawnRows++;
            if (resSpawnRows >= 3)
            {
                spawnRows = true;
                resSpawnRows = 0;
            }
        }

        if (bossObj != null)
            bossObj.ResetShield();

        return true;
    }

    public void NrBlocksInGame()
    {
        
    }

}
