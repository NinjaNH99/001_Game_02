using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRow : MonoSingleton<DestroyRow>
{
    public int nrBlock2HP;

    private void Awake()
    {
        nrBlock2HP = Random.Range(0, 3);
    }

    public void Update()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

}
