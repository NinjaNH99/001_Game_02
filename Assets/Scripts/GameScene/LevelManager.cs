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
        gameContr = GameController.Instance;
        LBLMAX = 3; LBNMAX = 0; SQBON = 3;
        resBLMAX = resBNMAX = resSQBON = resBOS = resSpawnRows = 0;
        curPosY = 0;
        desiredPosition = -130.0f;
        spawnRows = true;
        spawnBoss = false;
    }

    private void Start()
    {
        GenerateRow();
        EventManager.evSpawnRand += SpawnRandom;
    }

    public void GenerateRow()
    {
        EventManager.StartEvMoveDown();

        if (CheckData())
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, desiredPosition + curPosY);
            GameObject go_row = Instantiate(rowPrefab, this.transform) as GameObject;
            go_row.GetComponent<Row>().SpawnCont(spawnRows, spawnBoss, LBLMAX, LBNMAX, SQBON);
            listRows.Add(go_row);
            go_row.GetComponent<RectTransform>().anchoredPosition = Vector2.down * curPosY;
            curPosY -= DISTANCE_BETWEEN_BLOCKS;

            EventManager.StartEvSpawn();
            EventManager.StartEvRotate();
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
        int maxObj = 1;
        int posTelep = -1, posTelep2 = -1, posLiser = -1, posLiser2 = -1;

        if (gameContr.score_Rows > 5)
            maxObj = 2;

        if (maxObj == 1)
        {
            posLiser = Random.Range(0, listFreeConts.Count);
            listFreeConts[posLiser].GetComponentInParent<Container>().SpawnType(BlType.square_Line);
        }
        else
        {
            // Spawn Teleport1
            posTelep = Random.Range(0, listFreeConts.Count);
            listFreeConts[posTelep].GetComponentInParent<Container>().SpawnType(BlType.square_Teleport);
            // Spawn Teleport2
            do
                posTelep2 = Random.Range(0, listFreeConts.Count);
            while (Mathf.Abs(posTelep2 - posLiser) < 4);
            listFreeConts[posTelep2].GetComponentInParent<Container>().SpawnType(BlType.square_Teleport);

            // Spawn Liser1
            do
                posLiser = Random.Range(0, listFreeConts.Count);
            while (Mathf.Abs(posLiser - posTelep) < 4 || Mathf.Abs(posLiser - posTelep2) < 4);
            listFreeConts[posLiser].GetComponentInParent<Container>().SpawnType(BlType.square_Line);
            // Spawn Liser2
            do
                posLiser2 = Random.Range(0, listFreeConts.Count);
            while (Mathf.Abs(posLiser2 - posTelep) < 4 || Mathf.Abs(posLiser2 - posTelep2) < 4 || Mathf.Abs(posTelep2 - posLiser) < 4);
            listFreeConts[posLiser2].GetComponentInParent<Container>().SpawnType(BlType.square_Line);
        }


    }

    private bool CheckData()
    {
        if (LBLMAX <= 0)
        {
            resBLMAX++;
            if (resBLMAX >= RESETDATA - 1)
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
