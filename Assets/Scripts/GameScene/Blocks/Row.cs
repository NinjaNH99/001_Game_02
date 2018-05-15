using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowID;
    public int rowHP;

    protected int  SPMAX = 3;

    public Container[] containers = new Container[9];
    public ObjInfo[] rowMap = new ObjInfo[9];

    public delegate void EvDeSpawnContainer();
    public event EvDeSpawnContainer evDeSpawnContainer;

    private void Awake()
    {
        containers = GetComponentsInChildren<Container>();
        // Random sort containers by Fisher-Yates algorithm
        //new System.Random().Shuffle(containers);
    }

    // Spawn containters from rowMap
    public void SpawnCont(int rowIDP, ObjInfo[] rowMapP, int rowHP)
    {
        rowID = rowIDP;
        rowMap = rowMapP;
        this.rowHP = rowHP;

        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].rowID = rowID;
            evDeSpawnContainer += containers[i].DeSpawnBlock;
            switch (rowMap[i].type)
            {
                case 6:
                    {
                        containers[i].SpawnType((BlType)rowMap[i].type);
                        containers[i].GetComponentInChildren<Block>().hpx2 = 2;
                        containers[i].GetComponentInChildren<Block>().isBonus = true;
                        break;
                    }

                case 9:
                    {
                        containers[i].SpawnType((BlType)rowMap[i].type);
                        LevelManager.Instance.spawnBoss = false;
                        break;
                    }
                case 10:
                    {
                        containers[i].SpawnType(BlType.space, false, false);
                        break;
                    }

                default:
                    {
                        containers[i].SpawnType((BlType)rowMap[i].type);
                        break;
                    }
            }

        }

    }

    /*
    // Spawn random blockType from Cont in row
    public void SpawnCont(int rowIDP, bool spawnRows, bool SPBOSS, int BLMAX, int BONMAX, int SQBON)
    {
        rowID = rowIDP;

        var kBL = true;
        var kHPX2 = true;
        var god = false;
        var randType = 0;
       
        if(SPBOSS)
        {
            for (int i = 0; i < containers.Length; i++)
            {
                containers[i].rowID = rowID;
                evDeSpawnContainer += containers[i].DeSpawnBlock;
                if (i == 3 || i == 5 || i == 6)
                {
                    containers[i].SpawnType(BlType.space, false, false);
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
            containers[i].rowID = rowID;
            evDeSpawnContainer += containers[i].DeSpawnBlock;
            if (!spawnRows)
            {
                if (i == 3 || i == 4 || i == 5 || i == 6)
                    containers[i].SpawnType(BlType.space, false, false);
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
    */

    public void DeSpawn()
    {
        evDeSpawnContainer();
        //Debug.LogWarning("DeSpawnRow[" + rowID + "]");
        LevelManager.Instance.listRows.Remove(this.gameObject);
        GameData.nrRows--;
        GameData.levelMap.Dequeue();
        Destroy(this.gameObject);
    }
}
