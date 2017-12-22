using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Row : MonoBehaviour
{
    public int containerID { set; get; }

    public int length;

    private Container[] containers;

    private void Awake()
    {
        containers = GetComponentsInChildren<Container>();
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void DeSpawn()
    {
        gameObject.SetActive(false);
    }
}
