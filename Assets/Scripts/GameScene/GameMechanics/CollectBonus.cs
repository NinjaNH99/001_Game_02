using UnityEngine;

public class CollectBonus : MonoBehaviour
{
    public bool isByBonus = false;

    private Rigidbody2D rigid;
    private bool isCollected;
    private bool isDestroy;

    private void Awake()
    {
        isCollected = false;
        isDestroy = false;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (isByBonus)
        {
            EventManager.EvMethods += CollectByBons2;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(isByBonus)
        {
            return;
        }
        else
        {
            if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
                Collect();
            else if (coll.gameObject.CompareTag(Tags.EndLevel))
                DeathLevel();
        }
    }

    private void Collect()
    {
        Bonus.Instance.AddBonus_01();
        isCollected = true;
        if (!isDestroy)
        {
            GameController.Instance.UpdateUIText();
            isDestroy = true;
        }
        Destroy(this.gameObject);
    }

    private void CollectByBons2()
    {
        isByBonus = false;
        EventManager.EvMethods -= CollectByBons2;
    }

    public void DeathZone()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
    }

    public void DeathLevel()
    {
        if (!isCollected)
            Destroy(this.gameObject);
    }
}
