using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSpawner : MonoBehaviour
{
    public BlockType type;

    private Container currentCont;

    public void Spawn()
    {
        currentCont = LevelManager.Instance.GetContainer(type, 1);
        currentCont.gameObject.SetActive(true);
        currentCont.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        currentCont.gameObject.SetActive(false);
    }
}
