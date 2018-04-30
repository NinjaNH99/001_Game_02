using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 65.0f; // 83.0
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
    [HideInInspector]
    private int resBLMAX = 0, resBNMAX = 0, resSQBON = 0, resSpawnRows = 0;

    private float curPosY = 0;
    private float desiredPosition = -130.0f;

    private void Awake()
    {
        listRows = new List<GameObject>();
        listFreeConts = new List<Container>();

        LBLMAX = 3; LBNMAX = 0; SQBON = 3;
        resBLMAX = resBNMAX = resSQBON = resSpawnRows = 0;
        curPosY = 0;
        desiredPosition = -130.0f;
        spawnRows = true;
        spawnBoss = false;
        EventManager.EvSpawnRandomM += SpawnRandom;
    }

    private void Start()
    {
        GenerateRow();
    }

    public void GenerateRow(int k = 0)
    {
        int rowID = GameData.score_Rows;
        EventManager.StartEvDeSpawn();
        EventManager.StartEvMoveDown();

        if (CheckData())
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, desiredPosition + curPosY);
            GameObject go_row = Instantiate(rowPrefab, this.transform) as GameObject;
            go_row.GetComponent<Row>().SpawnCont(rowID, spawnRows, spawnBoss, LBLMAX, LBNMAX, SQBON);
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

}
