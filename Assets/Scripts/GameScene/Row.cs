using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public int rowID { set; get; }
    protected int length;
    protected int SQ1MAX, BLMAX, BONMAX;

    private Container[] containers;

    private void Awake()
    {
        SQ1MAX = Random.Range(0, 3);
        BLMAX = Random.Range(0, 3);
        BONMAX = Random.Range(0, 2);
        containers = GetComponentsInChildren<Container>();
        length = containers.Length;
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < length; i++)
        {
            int rand = Random.Range(0, 100);
            if (rand >= 60)
            {
                containers[i].SpawnType(4);
            }
            else if (rand >= 40 && SQ1MAX > 0)
            {
                containers[i].SpawnType(1);
                SQ1MAX--;
            }
            else if (rand >= 20 && BLMAX > 0)
            {
                containers[i].SpawnType(2);
                BLMAX--;
            }
            else if (rand >= 10 && BONMAX > 0)
            {
                containers[i].SpawnType(3);
                BONMAX--;
            }
            else
            {
                containers[i].SpawnType(0);
            }
        }
    }

    public void DeSpawn()
    {
        gameObject.SetActive(false);
    }
}
