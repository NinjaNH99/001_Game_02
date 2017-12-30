using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 84.0f; // 83.0
    private const float ANIMPOSY_SPEED = 250.0f;
    private const int RESETDATA = 4;

    public GameObject rowPrefab;
    // List of rows
    protected List<GameObject> rows = new List<GameObject>();
    // Max obj 
    public int LSQ1MAX, LBLMAX, LBNMAX;
    private int resSQ1Max, resBLMAX, resBNMAX;

    private float curPosY;
    private float desiredPosition;

    private void Awake()
    {
        LSQ1MAX = 3; LBLMAX = 3; LBNMAX = 1;
        resSQ1Max = resBLMAX = resBNMAX = 0;
        curPosY = 0;
        desiredPosition = -168.0f;
    }

    private void Start()
    {
        GenerateRow();
    }

    public void GenerateRow()
    {
        if (CheckData())
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, desiredPosition + curPosY);
            GameObject go_row = Instantiate(rowPrefab, this.transform) as GameObject;
            go_row.GetComponent<Row>().SetData(LBLMAX, LSQ1MAX, LBNMAX);
            rows.Add(go_row);
            go_row.GetComponent<RectTransform>().anchoredPosition = Vector2.down * curPosY;
            curPosY -= DISTANCE_BETWEEN_BLOCKS;
        }
        CheckRowsNull();
    }

    private void CheckRowsNull()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            if(!rows[i].GetComponent<Row>().DeSpawn())
            {
                rows.RemoveAt(i);
            }
        }
    }

    private bool CheckData()
    {
        Debug.Log("LSQ1MAX : " + LSQ1MAX);
        Debug.Log("LBLMAX : " + LBLMAX);
        Debug.Log("LBNMAX : " + LBNMAX);
        Debug.Log("-------------------");
        if (LSQ1MAX <= 0)
        {
            resSQ1Max++;
            if(resSQ1Max >= RESETDATA)
            {
                LSQ1MAX = 3;
                resSQ1Max = 0;
            }
        }
        if (LBLMAX <= 0)
        {
            resBLMAX++;
            if (resSQ1Max >= RESETDATA - 1)
            {
                LBLMAX = 3;
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

        return true;
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
