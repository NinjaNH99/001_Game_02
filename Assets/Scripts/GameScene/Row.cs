using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Row : MonoBehaviour
{
    public int rowID;
    public int nrBlock = 0;

    protected int SQ1MAX, BLMAX, BONMAX, SPMAX = 2;

    private Container[] containers = new Container[8];

    private void Awake()
    {
        rowID = GameController.score;

        containers = GetComponentsInChildren<Container>();
        // Random sort containers by Fisher-Yates algorithm
        new System.Random().Shuffle(containers);
    }

    // Spawn random blockType from Cont in row
    public void SpawnCont(int BLMAX, int SQ1MAX, int BONMAX)
    {
        bool kBL = true, kSQ1 = true;
        int rand;
       
        for (int i = 0; i < containers.Length; i++)
        {
            rand = Random.Range(0, 101);

            if (rand >= 60 && BLMAX > Random.Range(1,5) && kBL)
            {
                containers[i].SpawnType(BlType.ball);
                if(Random.Range(0, 3) != 1)
                    kBL = false;
                BLMAX--;
                LevelManager.Instance.LBLMAX--;
            }
            else if (rand >= 50 && SQ1MAX > Random.Range(1,4) && kSQ1)
            {
                containers[i].SpawnType(BlType.square_01);
                kSQ1 = false;
                LevelManager.Instance.LSQ1MAX--;
            }
            else if (SPMAX > 0)
            {
                containers[i].SpawnType(BlType.space);
                SPMAX--;
            }
            else if (rand >= 5 && BONMAX > 0)
            {
                containers[i].SpawnType(BlType.bonus);
                BONMAX--;
                LevelManager.Instance.LBNMAX--;
            }
            else
                containers[i].SpawnType(BlType.square);
        }
        Debug.Log("BLMAX :" + BLMAX);
        Debug.Log("SQ1MAX :" + SQ1MAX);
    }

    public void CheckNrConts(bool swt)
    {
        nrBlock--;
        if (swt)
        {
            if (nrBlock <= 1)
            {
                if (GetComponentInChildren<Square_01>() != null)
                    GetComponentInChildren<Square_01>().DeathZone();
            }
        }
    }

    public bool DeSpawn()
    {
        bool r = true;
        containers = GetComponentsInChildren<Container>();
        for (int i = 0; i < containers.Length;)
        {
            if (containers[i].DeSpawnBlock())
                i++;
        }

        Debug.Log( " RowID :" + rowID + "  containers.Length :" + nrBlock);
        if (nrBlock <= 0)
        {
            LevelManager.Instance.NrBlocksInGame();
            r = false;
            Destroy(this.gameObject, 0.1f);
        }
        return r;
    }
}
