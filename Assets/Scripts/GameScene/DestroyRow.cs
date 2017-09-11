using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRow : MonoSingleton<DestroyRow>
{
    public GameObject square_01;

    public int nrBlock2HP;
    private int nrObjects;

    private void Awake()
    {
        nrBlock2HP = Random.Range(0, 3);
    }

    private void Start()
    {
        nrObjects = GetComponentsInChildren<Container>().Length;
        if (GetComponentsInChildren<Square_01>() == null)
            nrObjects--;
        Debug.Log(nrObjects);
    }

    public void LateUpdate()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    public void ForDestroyfSquare_01()
    {
        if (GetComponentsInChildren<Square_01>() == null)
        {
            nrObjects--;
            if (nrObjects <= 0)
                square_01.GetComponent<Square_01>().DeathZone();
        }
    }

}
