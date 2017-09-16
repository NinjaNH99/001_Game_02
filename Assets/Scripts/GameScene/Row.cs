using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoSingleton<Row>
{
    public GameObject square_01;

    public int nrBlock2HP;

    private bool isSquare_01;

    private void Awake()
    {
        nrBlock2HP = Random.Range(0, 3);
    }

    private void Start()
    {
        isSquare_01 = false;
        if (GetComponentInChildren<Square_01>() != null)
            isSquare_01 = true;
    }

    public void LateUpdate()
    {
        if(isSquare_01)
        {
            if (transform.childCount == 1)
            {
                GetComponentInChildren<Square_01>().DeathZone();
                isSquare_01 = false;
            }
        }
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

}
