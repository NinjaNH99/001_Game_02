using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowID;
    public int nrSpace;

    protected int  BLMAX, BONMAX, SPMAX = 3;

    public Container[] containers = new Container[9];

    public delegate void EvDeSpawnContainer();
    public event EvDeSpawnContainer evDeSpawnContainer;

    private GameController gameContr;

    private void Awake()
    {
        gameContr = GameController.Instance;
        rowID = gameContr.score_Rows;
        nrSpace = 0;
        containers = GetComponentsInChildren<Container>();
        // Random sort containers by Fisher-Yates algorithm
        //new System.Random().Shuffle(containers);
    }

    public Block GetContIDBlock(int ID)
    {
        Debug.Log("ID: " + ID);
        if(containers[ID].GetComponentInChildren<Block>())
            return containers[ID].GetComponentInChildren<Block>();
        else
            return null;
    }

    // Spawn random blockType from Cont in row
    public void SpawnCont(bool spawnRows, bool SPBOSS, int BLMAX, int BONMAX, int SQBON)
    {
        var kBL = true;
        var kHPX2 = true;
        var god = false;
        var randType = 0;
       
        if(SPBOSS)
        {
            for (int i = 0; i < containers.Length; i++)
            {
                if (i == 3 || i == 5)
                {
                    containers[i].SpawnType(BlType.space, false);
                    nrSpace++;
                }
                else if (i == 4)
                {
                    containers[i].SpawnType(BlType.square_Boss);
                    LevelManager.Instance.spawnBoss = false;
                }
                else
                {
                    containers[i].SpawnType(BlType.space);
                    nrSpace++;
                }
            }
            return;
        }

        for (int i = 0; i < containers.Length; i++)
        {
            if (!spawnRows)
            {
                if (i == 3 || i == 4 || i == 5)
                    containers[i].SpawnType(BlType.space, false);
                else
                    containers[i].SpawnType(BlType.space);
                nrSpace++;
            }
            else
            {
                while (!god)
                {
                    randType = Random.Range(0, 11);
                    switch (randType)
                    {
                        case 0:
                            {
                                if (BLMAX > 0 && kBL)
                                {
                                    containers[i].SpawnType(BlType.ball);
                                    if (Random.Range(0, 4) != 1)
                                        kBL = false;
                                    BLMAX--;
                                    LevelManager.Instance.LBLMAX--;
                                    god = true;
                                }
                                break;
                            }
                        case 1:
                            {
                                /*
                                if (SQ1MAX > 0 && kSQ1)
                                {
                                    containers[i].SpawnType(BlType.square_Teleport);
                                    kSQ1 = false;
                                    LevelManager.Instance.LTelepMAX--;
                                    nrSpace++;
                                    god = true;
                                }*/
                                break;
                            }
                        case 3:
                            {
                                if (BONMAX > 0)
                                {
                                    containers[i].SpawnType(BlType.bonus);
                                    BONMAX--;
                                    LevelManager.Instance.LBNMAX--;
                                    god = true;
                                }
                                break;
                            }
                        case 4:
                            {
                                if (SQBON > 0 && kHPX2)
                                {
                                    containers[i].SpawnType(BlType.square_Bonus);
                                    containers[i].GetComponentInChildren<Block>().hpx2 = 2;
                                    containers[i].GetComponentInChildren<Block>().isBonus = true;
                                    SQBON--;
                                    kHPX2 = false;
                                    LevelManager.Instance.SQBON--;
                                    god = true;
                                }
                                break;
                            }
                        case 5:
                            {
                                /*
                                if (SQLINE > 0 && kSQLINE && (Random.Range(0, 4) != 1))
                                {
                                    containers[i].SpawnType(BlType.square_Line);
                                    SQLINE--;
                                    kSQLINE = false;
                                    nrSpace++;
                                    LevelManager.Instance.LSQLINE--;
                                    god = true;
                                }*/
                                break;
                            }
                        case 6:
                            {
                                containers[i].SpawnType(BlType.square);
                                god = true;
                                break;
                            }
                        case 7:
                            {
                                containers[i].SpawnType(BlType.square);
                                god = true;
                                break;
                            }
                        case 9:
                            {
                                if (BLMAX > 0 && kBL)
                                {
                                    containers[i].SpawnType(BlType.ball);
                                    if (Random.Range(0, 4) != 1)
                                        kBL = false;
                                    BLMAX--;
                                    LevelManager.Instance.LBLMAX--;
                                    god = true;
                                }
                                break;
                            }
                        default:
                            {
                                if (SPMAX > 0)
                                {
                                    containers[i].SpawnType(BlType.space);
                                    nrSpace++;
                                    SPMAX--;
                                    god = true;
                                }
                                break;
                            }
                    }
                }

                god = false;

            }
        }
    }

    public void DeSpawn()
    {
        Debug.Log("NrSpace: " + nrSpace);
        /*
        containers = GetComponentsInChildren<Container>();
        for (int i = 0; i < containers.Length;)
        {
            if (containers[i].DeSpawnBlock())
                i++;
        }*/

        //Debug.Log( " RowID :" + rowID + "  containers.Length :" + nrBlock);
        if (nrSpace >= 9)
        {
            evDeSpawnContainer();
            LevelManager.Instance.listRows.Remove(this.gameObject);
            /*
            containers = GetComponentsInChildren<Container>();
            for (int i = 0; i < containers.Length; i++)
                containers[i].DeSpawnBlock();
            */

            Debug.Log("DeSpawnRow");
            Destroy(this.gameObject);
        }

    }
}
