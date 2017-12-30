using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowID;

    protected int SQ1MAX, BLMAX, BONMAX, SPMAX;

    private Container[] containers = new Container[8];

    private void Awake()
    {
        rowID = GameController.score;
        int rand = Random.Range(0, 100);
        if (rand >= 60)
            SPMAX = 3;
        else if (rand >= 20)
            SPMAX = 2;

        containers = GetComponentsInChildren<Container>();
    }

    public void SetData(int x1, int x2, int x3)
    {
        BLMAX = x1;
        SQ1MAX = x2;
        BONMAX = x3;
        Spawn();
    }

    private void Spawn()
    {
        int kBL = 0, kSQ1 = 0;
        
        for (int i = 0; i < containers.Length; i++)
        {
            int rand = Random.Range(0, 101);

            if (rand >= 70 && BLMAX > 0 && kBL <= 1)
            {
                containers[i].SpawnType(2);
                BLMAX--;
                kBL++;
                LevelManager.Instance.LBLMAX--;
            }
            else if (rand >= 40 && SQ1MAX > 0 && kSQ1 <= 1)
            {
                containers[i].SpawnType(1);
                SQ1MAX--;
                kSQ1++;
                LevelManager.Instance.LSQ1MAX--;
            }
            else if (rand >= 20 && SPMAX > 0)
            {
                containers[i].SpawnType(5);
                SPMAX--;
            }
            else if (rand >= 10 && BONMAX > 0)
            {
                containers[i].SpawnType(3);
                BONMAX--;
                LevelManager.Instance.LBNMAX--;
            }
            else
                containers[i].SpawnType(4);
        }

    }

    public bool DeSpawn()
    {
        bool r = true;
        containers = GetComponentsInChildren<Container>();
        for (int i = 0; i < containers.Length; i++)
            containers[i].DeSpawnBlock();
        if (containers.Length == 0)
        {
            r = false;
            Destroy(this.gameObject, 0.5f);
        }
        return r;
    }
}
