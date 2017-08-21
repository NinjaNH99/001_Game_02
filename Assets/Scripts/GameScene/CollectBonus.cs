using UnityEngine;

public class CollectBonus : MonoBehaviour
{
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
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
        {
            GameController.Instance.bonus_01++;
            isCollected = true;
            if (!isDestroy)
            {
                BlockContainer.Instance.nrBlocksInGame--;
                GameController.Instance.UpdateUIText();
                isDestroy = true;
            }
            Destroy(this.gameObject);
        }
        else if (coll.gameObject.CompareTag(Tags.EndLevel))
            DeathLevel();
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
