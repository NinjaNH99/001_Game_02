using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 65.0f;
    private const int RESETDATA = 4;

    public GameObject rowPrefab;
    [HideInInspector]
    public RectTransform teleportIn = null, teleportOut = null;
    [HideInInspector]
    public Block_Boss bossObj = null;

    private GameController gameContr;
    // List of rows
    public List<GameObject> listRows = new List<GameObject>();
    // List of free containers
    public List<Container> listFreeConts = new List<Container>();

    // Max obj 
    public bool spawnRows = true, spawnBoss = false;
    [HideInInspector]
    public int LBLMAX = 3, LBNMAX = 0, SQBON = 3;

    private int resBLMAX = 0, resBNMAX = 0, resSQBON = 0; /*resSpawnRows = 0;*/

    private float curPosY = 0;
    private float desiredPosition = -130.0f;

    private void Awake()
    {
        listRows = new List<GameObject>();
        listFreeConts = new List<Container>();

        LBLMAX = 3; LBNMAX = 0; SQBON = 3;
        resBLMAX = resBNMAX = resSQBON = 0; /*resSpawnRows = 0;*/
        curPosY = 0;
        desiredPosition = -130.0f;
        spawnRows = true;
        spawnBoss = false;
        EventManager.EvSpawnRandomM += SpawnRandom;
    }

    private void Start()
    {
        if (GameData.restartGame)
            GenerateMapNewGame();
        else
        {
            int rowID;

            for(int i = GameData.nrRows - 1, j = 0; i >= 0; i--, j++)
            {
                rowID = GameData.nrRows - i;
                GenerateMapContGame(j, rowID);
            }
        }
    }

    public void GenerateMapNewGame()
    {
        EventManager.StartEvDeSpawn();
        EventManager.StartEvMoveDown();

        if (CheckData())
        {
            int[] rowMap = GenerateRowMap();

            GameData.levelMap.Enqueue(rowMap);

            int rowID = GameData.score_Rows;

            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, desiredPosition + curPosY);
            GameObject go_row = Instantiate(rowPrefab, transform) as GameObject;
            go_row.GetComponent<Row>().SpawnCont(rowID, rowMap);

            listRows.Add(go_row);
            go_row.GetComponent<RectTransform>().anchoredPosition = Vector2.down * curPosY;
            curPosY -= DISTANCE_BETWEEN_BLOCKS;

            if (listRows.Count > 6 && listRows.Count < 12)
            {
                EventManager.StartEvSpawn();
            }

            GameData.nrRows++;
        }
    }

    private void GenerateMapContGame(int index, int rowID)
    {
        int[] rowMap = new int[9];

        rowMap = GameData.levelMap.ElementAt(index);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, desiredPosition + curPosY);
        GameObject go_row = Instantiate(rowPrefab, transform) as GameObject;
        go_row.GetComponent<Row>().SpawnCont(rowID, rowMap);

        listRows.Add(go_row);
        go_row.GetComponent<RectTransform>().anchoredPosition = Vector2.down * curPosY;
        curPosY -= DISTANCE_BETWEEN_BLOCKS;

        if (listRows.Count > 6 && listRows.Count < 12 && index == GameData.nrRows - 1)
            EventManager.StartEvSpawn();

    }

    private int[] GenerateRowMap()
    {
        int[] rowMap = new int[9];

        if (spawnBoss)
        {
            for (int i = 0; i < rowMap.Length; i++)
            {
                if (i == 3 || i == 5 || i == 6)
                    rowMap[i] = 10;
                else if (i == 4)
                {
                    rowMap[i] = 9;
                    spawnBoss = false;
                }
                else
                    rowMap[i] = 8;
            }
            return rowMap;
        }
        if(!spawnRows)
        {
            for (int i = 0; i < rowMap.Length; i++)
            {
                if (i == 3 || i == 4 || i == 5 || i == 6)
                    rowMap[i] = 10;
                else
                    rowMap[i] = 8;
            }
            spawnRows = true;
            return rowMap;
        }

        bool god = false, kBL = true, kHPX2 = true;
        int SPMAX = 3, randType = 0;

        for (int i = 0; i < rowMap.Length; i++)
        {
            //if (!spawnRows)
            //{
            //    if (i == 3 || i == 4 || i == 5 || i == 6)
            //        rowMap[i] = 10;
            //    else
            //        rowMap[i] = 8;
            //}
            //else
            //{
                while (!god)
                {
                    randType = Random.Range(0, 11);
                    switch (randType)
                    {
                        case 0:
                            {
                                if (LBLMAX > 0 && kBL)
                                {
                                    rowMap[i] = 2;
                                    if (Random.Range(0, 4) != 1)
                                        kBL = false;
                                    LBLMAX--;
                                    god = true;
                                }
                                break;
                            }
                        case 3:
                            {
                                if (LBNMAX > 0)
                                {
                                    rowMap[i] = 3;
                                    LBNMAX--;
                                    god = true;
                                }
                                break;
                            }
                        case 4:
                            {
                                if (SQBON > 0 && kHPX2)
                                {
                                    rowMap[i] = 6;
                                    SQBON--;
                                    kHPX2 = false;
                                    SQBON--;
                                    god = true;
                                }
                                break;
                            }
                        case 6:
                            {
                                rowMap[i] = 1;
                                god = true;
                                break;
                            }
                        case 7:
                            {
                                rowMap[i] = 1;
                                god = true;
                                break;
                            }
                        case 9:
                            {
                                if (LBLMAX > 0 && kBL)
                                {
                                    rowMap[i] = 2;
                                    if (Random.Range(0, 4) != 1)
                                        kBL = false;
                                    LBLMAX--;
                                    god = true;
                                }
                                break;
                            }
                        default:
                            {
                                if (SPMAX > 0)
                                {
                                    rowMap[i] = 8;
                                    SPMAX--;
                                    god = true;
                                }
                                break;
                            }
                    }
                }

                god = false;

            //}
        }

        return rowMap;
    }

    public void ApplyBallBonus(int contID, int rowID)
    {
        //Debug.LogWarning("contID: " + contID + " rowID: " + rowID);
        Container[] conts;
        try
        {
            var row = listRows.FindIndex(x => x.GetComponent<Row>().rowID == rowID);
            conts = listRows[row + 1].GetComponentsInChildren<Container>();
        }
        catch (System.Exception)
        {
            return;
        }

        for (int i = 0; i < conts.Length; i++)
        {
            if ((conts[i].visualIndex == contID - 1 || conts[i].visualIndex == contID || conts[i].visualIndex == contID + 1) && conts[i].GetComponentInChildren<Block>() != null)
            {
                conts[i].GetComponentInChildren<Block>().ReceiveHit(true);
            }
        }
    }

    public void SpawnRandom()
    {
        //Debug.Log("listFreeConts[0].RowID : " + listFreeConts[0].GetComponentInParent<Row>().rowID);
        //Debug.Log("listFreeConts[listFreeConts.Count - 1].RowID : " + listFreeConts[listFreeConts.Count - 1].GetComponentInParent<Row>().rowID);
        //Debug.Log("SpawnRandom");

        int posTelep = -1, posTelep2 = -1, posLiser = -1;
        int rowPosTelep = -1, rowPosLiser = -1, contPosLiser = -1;

        // Spawn Teleport1
        posTelep = Random.Range(0, listFreeConts.Count);
        rowPosTelep = listFreeConts[posTelep].rowID;
        listFreeConts[posTelep].SpawnType(BlType.teleportIn);

        // Spawn Teleport2
        posTelep2 = SpawnTelep2(rowPosTelep);

        // Spawn Liser1
        SpawnLiser1(posTelep, posTelep2, out posLiser, out rowPosLiser, out contPosLiser);

        // Spawn Liser2
        SpawnLiser2(posTelep, posTelep2, rowPosLiser, contPosLiser);
    }

    private int SpawnLiser2(int posTelep, int posTelep2, int rowPosLiser, int contPosLiser)
    {
        int k = 0, option = 3;
        int posLiser2;
        do
        {
            k++;
            posLiser2 = Random.Range(0, listFreeConts.Count);
            if (k > listFreeConts.Count)
            {
                option = 1;
                k = 0;
            }
        }
        while (Mathf.Abs(listFreeConts[posLiser2].rowID - rowPosLiser) < option || listFreeConts[posLiser2].visualIndex == contPosLiser || posLiser2 == posTelep || posLiser2 == posTelep2);
        listFreeConts[posLiser2].SpawnType(BlType.square_Line);
        return posLiser2;
    }

    private void SpawnLiser1(int posTelep, int posTelep2, out int posLiser, out int rowPosLiser, out int contPosLiser)
    {
        do
        {
            posLiser = Random.Range(0, listFreeConts.Count);
        }
        while (posLiser == posTelep || posLiser == posTelep2);
        rowPosLiser = listFreeConts[posLiser].rowID;
        contPosLiser = listFreeConts[posLiser].visualIndex;
        listFreeConts[posLiser].SpawnType(BlType.square_Line);
    }

    private int SpawnTelep2(int rowPosTelep)
    {
        int k = 0, option = 4;
        int posTelep2;
        do
        {
            k++;
            posTelep2 = Random.Range(0, listFreeConts.Count);
            if (k > listFreeConts.Count)
            {
                option = 2;
                k = 0;
            }
        }
        while (Mathf.Abs(listFreeConts[posTelep2].rowID - rowPosTelep) < option);
        listFreeConts[posTelep2].SpawnType(BlType.teleportOut);
        return posTelep2;
    }

    private bool CheckData()
    {
        if (LBLMAX <= 0)
        {
            resBLMAX++;
            if (resBLMAX >= RESETDATA - 1)
            {
                if (GameData.score_Rows - GameData.amountBalls > 3)
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

        if ((GameData.score_Rows) / 10f == 1f)
        {
            spawnBoss = true;
            spawnRows = false;
        }

        //if (!spawnRows)
        //{
        //    resSpawnRows++;
        //    if (resSpawnRows >= 3)
        //    {
        //        spawnRows = true;
        //        resSpawnRows = 0;
        //    }
        //}

        if (bossObj != null)
            bossObj.ResetShield();

        return true;
    }

}
