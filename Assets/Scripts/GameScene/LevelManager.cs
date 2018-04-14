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
    // List of free containers
    public List<GameObject> listFreeConts = new List<GameObject>();

    // Max obj 
    public bool spawnRows, spawnBoss;
    [HideInInspector]
    public int LBLMAX, LBNMAX , SQBON, nrRowsInGame;
    [HideInInspector]
    private int resBLMAX, resBNMAX, resSQBON, resBOS, resSpawnRows;

    private float curPosY;
    private float desiredPosition;

    private void Awake()
    {
        listRows = new List<GameObject>();
        listTelep = new List<GameObject>();
        listFreeConts = new List<GameObject>();

        LBLMAX = 3; LBNMAX = 0; SQBON = 3;
        resBLMAX = resBNMAX = resSQBON = resBOS = resSpawnRows = 0;
        curPosY = 0;
        desiredPosition = -130.0f;
        spawnRows = true;
        spawnBoss = false;
        EventManager.EvSpawnRandomM += SpawnRandom;
    }

    private void Start()
    {
        int nrRows = GameData.nrRows;

        if (nrRows <= 0)
            nrRows = 1;

        for (int i = 0; i < 1; i++)
            GenerateRow();
        //EventManager.EvSpawnRandomM += SpawnRandom;
    }

    public void GenerateRow()
    {
        EventManager.StartEvDeSpawn();
        EventManager.StartEvMoveDown();

        if (CheckData())
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, desiredPosition + curPosY);
            GameObject go_row = Instantiate(rowPrefab, this.transform) as GameObject;
            go_row.GetComponent<Row>().SpawnCont(spawnRows, spawnBoss, LBLMAX, LBNMAX, SQBON);
            listRows.Add(go_row);
            go_row.GetComponent<RectTransform>().anchoredPosition = Vector2.down * curPosY;
            curPosY -= DISTANCE_BETWEEN_BLOCKS; 

            if(GameData.score_Rows > 6 && GameData.score_Rows < 12)
            {
                EventManager.StartEvSpawn();
            }

            //nrRows++;
            GameData.nrRows++;
        }
        //Debug.LogWarning(" Rows[0] : " + listRows[0].GetComponent<Row>().rowID);
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

    public void Teleports(GameObject currTeleport,GameObject ball)
    {
        if (listTelep.Count <= 1)
            return;

        var index = listTelep.FindIndex(x => x.gameObject == currTeleport);
        //Debug.Log("ID Teleport: " + index);

        try
        {
            ball.GetComponent<RectTransform>().position = listTelep[index + 1].gameObject.GetComponent<RectTransform>().position;
            ball.GetComponent<Ball>().enterTeleport = false;
        }
        catch (System.Exception)
        {
            ball.GetComponent<RectTransform>().position = listTelep[0].gameObject.GetComponent<RectTransform>().position;
            ball.GetComponent<Ball>().enterTeleport = false;
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
        rowPosTelep = listFreeConts[posTelep].GetComponent<Container>().rowID;
        listFreeConts[posTelep].GetComponent<Container>().SpawnType(BlType.square_Teleport);

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
                option = 1;
        }
        while (Mathf.Abs(listFreeConts[posLiser2].GetComponent<Container>().rowID - rowPosLiser) < option || listFreeConts[posLiser2].GetComponent<Container>().visualIndex == contPosLiser || posLiser2 == posTelep || posLiser2 == posTelep2);
        listFreeConts[posLiser2].GetComponent<Container>().SpawnType(BlType.square_Line);
        return posLiser2;
    }

    private void SpawnLiser1(int posTelep, int posTelep2, out int posLiser, out int rowPosLiser, out int contPosLiser)
    {
        do
            posLiser = Random.Range(0, listFreeConts.Count);
        while (posLiser == posTelep || posLiser == posTelep2);
        rowPosLiser = listFreeConts[posLiser].GetComponent<Container>().rowID;
        contPosLiser = listFreeConts[posLiser].GetComponent<Container>().visualIndex;
        listFreeConts[posLiser].GetComponent<Container>().SpawnType(BlType.square_Line);
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
                option = 2;
        }
        while (Mathf.Abs(listFreeConts[posTelep2].GetComponent<Container>().rowID - rowPosTelep) < option);
        listFreeConts[posTelep2].GetComponent<Container>().SpawnType(BlType.square_Teleport);
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

}
