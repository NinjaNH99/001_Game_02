using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowID;
    public int rowHP;

    public Container[] containers = new Container[9];
    public ObjInfo[] rowMapOrg = new ObjInfo[9];

    public delegate void EvDeSpawnContainer();
    public event EvDeSpawnContainer evDeSpawnContainer;

    private void Awake()
    {
        containers = GetComponentsInChildren<Container>();

        rowMapOrg = new ObjInfo[9];
        // Random sort containers by Fisher-Yates algorithm
        //new System.Random().Shuffle(containers);
    }

    // Spawn containters from rowMap
    public void SpawnCont(int rowIDP, ObjInfo[] rowMapP, int rowHP)
    {
        this.rowID = rowIDP;
        this.rowMapOrg = rowMapP;

        this.rowHP = rowHP;

        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].rowID = rowID;
            evDeSpawnContainer += containers[i].DeSpawnBlock;
            switch (rowMapOrg[i].type)
            {
                case 6:
                    {
                        containers[i].SpawnType((BlType)rowMapOrg[i].type);
                        containers[i].GetComponentInChildren<Block>().hpx2 = 2;
                        containers[i].GetComponentInChildren<Block>().isBonus = true;
                        break;
                    }

                case 9:
                    {
                        containers[i].SpawnType((BlType)rowMapOrg[i].type);
                        LevelManager.Instance.timerBoss = true;
                        break;
                    }
                case 10:
                    {
                        containers[i].SpawnType(BlType.space, false, false);
                        break;
                    }

                default:
                    {
                        containers[i].SpawnType((BlType)rowMapOrg[i].type);
                        break;
                    }
            }

        }

    }

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
