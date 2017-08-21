using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public GameObject squareObject;
    public GameObject square_01Object;
    public GameObject ballObject;
    public GameObject bonus_01;
    public GameObject goHpText;
    public GameObject DeathEFX;

    private TextMeshPro hpText;

    public int hp;

    private bool isDestroy;

    private void Start()
    {
        hpText = goHpText.GetComponent<TextMeshPro>();
        if (GameController.score % Random.Range(2,4) == 0 && Random.Range(0f,1f) > 0.3f)
            hp = GameController.score * 2;
        else
            hp = GameController.score;
        hpText.text = hp.ToString();
        isDestroy = true;
        squareObject.GetComponent<SpriteRenderer>().color = GameController.Instance.ChangeColor(hp);
    }

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
        BlockContainer.Instance.nrBlocksInGame--;
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

    public void ReceiveHit()
    {
        hp--;
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
        if (hp <= 0)
        {
            hpText.text = "1";
            GameObject go = Instantiate(DeathEFX, gameObject.transform) as GameObject;
            Destroy(squareObject);
            Destroy(go, 1f);
            squareObject.GetComponent<BoxCollider2D>().isTrigger = true;
            if (isDestroy)
            {
                BlockContainer.Instance.nrBlocksInGame--;
                BlockContainer.Instance.NrBlocksInGame();
                isDestroy = false;
            }
            return;
        }
        hpText.text = hp.ToString();
        squareObject.GetComponent<SpriteRenderer>().color = GameController.Instance.ChangeColor(hp); 
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

}
