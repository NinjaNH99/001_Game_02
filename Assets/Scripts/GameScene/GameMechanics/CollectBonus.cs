using UnityEngine;
using UnityEngine.UI;

public class CollectBonus : MonoBehaviour
{
    public bool isByBonus = false;

    private bool isCollected, isDestroy;

    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        isCollected = false;
        isDestroy = false;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (isByBonus)
        {
            EventManager.EvMoveDownM += CollectByBons2;
            anim.SetTrigger("BonON");
        }
        else
            anim.SetTrigger("BonOFF");
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
            GetComponentInParent<Container>().AddInListFreeConts();

            //GetComponentInParent<Row>().nrSpace++;
            //GameController.Instance.UpdateUIText();
            isDestroy = true;
        }
        Destroy(this.gameObject);
    }

    private void CollectByBons2()
    {
        isByBonus = false;
        anim.SetTrigger("BonOFF");
        EventManager.EvMoveDownM -= CollectByBons2;
    }

    public void DeathZone()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
    }

    public void DeathLevel()
    {
        if (!isCollected)
        {
            Destroy(this.gameObject);
            if (!LevelManager.Instance.listFreeConts.Contains(GetComponentInParent<Container>()))
                LevelManager.Instance.listFreeConts.Remove(GetComponentInParent<Container>());
        }
    }
}
