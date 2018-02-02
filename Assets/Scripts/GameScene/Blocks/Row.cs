using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowID;
    public int nrBlock = 0;

    protected int SQ1MAX, SQLINE, BLMAX, BONMAX, SPMAX = 3;

    private GameController gameContr;

    private Container[] containers = new Container[10];

    private void Awake()
    {
        gameContr = GameController.Instance;
        rowID = gameContr.score_Rows;

        containers = GetComponentsInChildren<Container>();
        // Random sort containers by Fisher-Yates algorithm
        new System.Random().Shuffle(containers);
    }

    // Spawn random blockType from Cont in row
    public void SpawnCont(int BLMAX, int SQ1MAX, int SQLINE, int BONMAX, int SQBON)
    {
        var kBL = true;
        var kSQ1 = true;
        var kSQLINE = true;
        var kHPX2 = true;
        var rand = 0;
       
        for (int i = 0; i < containers.Length; i++)
        {
            rand = Random.Range(0, 101);

            if (rand >= 60 && BLMAX > 0 && kBL)
            {
                containers[i].SpawnType(BlType.ball);
                if (Random.Range(0, 4) != 1)
                    kBL = false;
                BLMAX--;
                LevelManager.Instance.LBLMAX--;
            }
            else if (rand >= 50 && SQ1MAX > 0 && kSQ1)
            {
                containers[i].SpawnType(BlType.square_Teleport);
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
            {
                if(SQBON > 0 && kHPX2)
                {
                    containers[i].SpawnType(BlType.square_Bonus);
                    containers[i].GetComponentInChildren<Block>().hpx2 = 2;
                    containers[i].GetComponentInChildren<Block>().isBonus = true;
                    SQBON--;
                    kHPX2 = false;
                    LevelManager.Instance.SQBON--;
                }
                else if (SQLINE > 0 && kSQLINE && (Random.Range(0, 4) != 1))
                {
                    containers[i].SpawnType(BlType.square_Line);
                    SQLINE--;
                    kSQLINE = false;
                    LevelManager.Instance.LSQLINE--;
                }
                else
                    containers[i].SpawnType(BlType.square);
            }
        }
    }

    public void CheckNrConts()
    {
        nrBlock--;
        /*if (swt)
        {
            if (nrBlock <= 1)
            {
                if (GetComponentInChildren<Square_01>() != null)
                    GetComponentInChildren<Square_01>().DeathZone();
            }
        }*/
    }

    public void RunBonus_02()
    {
        var blocks = GetComponentsInChildren<Block>();
        for (int i = 0; i < blocks.Length;)
        {
            blocks[i].ReciveHitByBonus();
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

        //Debug.Log( " RowID :" + rowID + "  containers.Length :" + nrBlock);
        if (containers.Length <= 0)
        {
            LevelManager.Instance.NrBlocksInGame();
            r = false;
            Destroy(this.gameObject, 0.1f);
        }
        return r;
    }
}
