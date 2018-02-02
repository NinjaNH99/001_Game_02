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

    // Max obj 
    [HideInInspector]
    public int LSQ1MAX, LBLMAX, LBNMAX , SQBON, LSQLINE,nrRowsInGame;
    private int resSQ1Max, resBLMAX, resBNMAX, resSQBON, resSQLINE;

    private float curPosY;
    private float desiredPosition;

    private void Awake()
    {
        gameContr = GameController.Instance;
        LSQ1MAX = 1; LBLMAX = 3; LBNMAX = 0; SQBON = 4; LSQLINE = 2;
        resSQ1Max = resBLMAX = resBNMAX = resSQBON = resSQLINE = 0;
        curPosY = 0;
        desiredPosition = -130.0f;
    }

    private void Start()
    {
        GenerateRow();
        GenerateRow();
    }

    public void GenerateRow()
    {
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

    private bool CheckData()
    {
        if (LSQ1MAX <= 0)
        {
            resSQ1Max++;
            if(resSQ1Max >= RESETDATA - 1)
            {
                LSQ1MAX = 1;
                resSQ1Max = 0;
            }
        }
        if (LBLMAX <= 0)
        {
            resBLMAX++;
            if (resSQ1Max >= RESETDATA - 2)
            {
                if(gameContr.score_Rows - gameContr.amountBalls > 3)
                    LBLMAX = 4;
                else
                    LBLMAX = 2;
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
            if (resSQLINE >= RESETDATA + 1)
            {
                LSQLINE = 2;
                resSQLINE = 0;
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

    /*
    public Container GetContainer(BlockType bt, int visualIndex)
    {
        Container cont = containers.Find(x => x.blockType == bt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(cont == null)
        {
            GameObject go = null;

            go = Instantiate(go);
            cont = go.GetComponent<Container>();
            containers.Add(cont);
        }
        return cont;
    }*/

}
