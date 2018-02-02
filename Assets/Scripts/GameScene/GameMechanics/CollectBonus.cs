using UnityEngine;

public class CollectBonus : MonoBehaviour
{
    public bool isByBonus = false;

    private Rigidbody2D rigid;
    private bool isCollected;
    private bool isDestroy;

    private void Start()
    {
        isCollected = false;
        isDestroy = false;
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(isByBonus)
        {
            if (coll.gameObject.CompareTag(Tags.Player))
                Collect();
            else if (coll.gameObject.CompareTag(Tags.EndLevel))
                DeathLevel();
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
        Bonus.bonus_01++;
        isCollected = true;
        if (!isDestroy)
        {
            GetComponentInParent<Row>().CheckNrConts();
            //LevelContainer.Instance.nrBlocksInGame--;
            GameController.Instance.UpdateUIText();
            isDestroy = true;
        }
        Destroy(this.gameObject);
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
