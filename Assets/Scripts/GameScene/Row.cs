using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoSingleton<Row>
{
    public GameObject square_01;

    public int nrBlock2HP;

    private bool isSquare_01;
    public Container[] container;

    private void Awake()
    {
        nrBlock2HP = Random.Range(0, 3);
    }

    private void Start()
    {
        isSquare_01 = false;
        if (GetComponentInChildren<Square_01>() != null)
            isSquare_01 = true;
        CheckNrBlock();
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

    public void UplayBonus_02(int index)
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].GetComponentInChildren<Container>() != null)
            {
                if (container[i].GetComponentInChildren<Container>().posIn == (index - 1))
                    if (container[i].GetComponentInChildren<Block>() != null)
                        container[i].GetComponentInChildren<Block>().ReciveHitByBonus(-1);

                if (container[i].GetComponentInChildren<Container>().posIn == (index + 1))
                    if (container[i].GetComponentInChildren<Block>() != null)
                        container[i].GetComponentInChildren<Block>().ReciveHitByBonus(1);
            }
        }
    }

    public void CheckNrBlock()
    {
        container = GetComponentsInChildren<Container>();
    }

}
