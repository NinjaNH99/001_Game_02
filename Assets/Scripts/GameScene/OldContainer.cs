using UnityEngine;

public class OldContainer : MonoSingleton<OldContainer>
{
    public GameObject squareObject;
    public GameObject square_01Object;
    public GameObject ballObject;
    public GameObject bonus_01;

    public int posIn;

    private void LateUpdate()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnSquare_01()
    {
        DestroyBlock();
        Instantiate(square_01Object, gameObject.transform);
        LevelContainer.Instance.nrBlocksInGame--;
    }

    public void SpawnBall()
    {
        DestroyBlock();
        Instantiate(ballObject, gameObject.transform);
    }

    public void SpawnBonus()
    {
        DestroyBlock();
        Instantiate(bonus_01, gameObject.transform);
    }

    public void DestroyBlock()
    {
        Destroy(squareObject);
    }

    public void EndLevel(GameObject tag)
    {
        if (tag == squareObject)
        {
            squareObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        else if (tag == square_01Object)
        {
            Destroy(square_01Object);
        }
    }

    public void RunBonus_02()
    {
        ///GetComponentInParent<Row>().CheckNrBlock();
        //GetComponentInParent<Row>().UplayBonus_02(posIn);
    }

}
