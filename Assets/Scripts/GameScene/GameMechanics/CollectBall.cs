using UnityEngine;
using UnityEngine.UI;

public class CollectBall : MonoBehaviour
{
    public GameObject Sprite;
    public GameObject AddBallUIPr;
    public GameObject AddBallEFX, despawnEFX;

    public bool isByBonus = false;

    private bool collected, LoseBall, isDestroy;

    private Rigidbody2D rigid;
    private GameObject Space2D;
    private Animator anim;
    private RectTransform rectPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rectPos = GetComponent<RectTransform>();
        //GetComponent<Image>().color = spriteColor;
        Space2D = GameObject.FindGameObjectWithTag(Tags.Space2D);
        collected = false;
        LoseBall = false;
        isDestroy = true;

    }

    private void Start()
    {
        if (isByBonus)
        {
            anim.SetTrigger("BonON");
            EventManager.EvMoveDownM += CollectByBons;
        }
        else
            anim.SetTrigger("BonOFF");
    }

    private void CollectByBons()
    {
        isByBonus = false;
        anim.SetTrigger("BonOFF");
        EventManager.EvMoveDownM -= CollectByBons;
    }

    private void Collect()
    {
        if (!collected)
        {
            collected = true;
            GameController.Instance.amountCollectBallsLeft++;
            GameObject go = Instantiate(AddBallEFX, gameObject.transform) as GameObject;
            Destroy(go, 2f);

            GetComponentInParent<Container>().AddInListFreeConts();
            ScoreLEVEL.Instance.BGShake(1);
            //GetComponentInParent<Row>().nrSpace++;
            StartFalling();
            if (isDestroy)
            {
                GameController.Instance.AddBallUI++;
                var AmountBallUIPos = BallInit.Instance.amountBallsTextPr.transform;
                GameObject goEFX = Instantiate(AddBallUIPr, AmountBallUIPos) as GameObject;
                goEFX.GetComponent<RectTransform>().localPosition = new Vector2(-95, -95);
                Destroy(goEFX, 1f);
                GameController.Instance.amountBallsLeft++;
                isDestroy = false;
            }
            rigid.velocity = new Vector2(0, 0.5f) * 2.0f;
            //spriteColor.a = 0.8f;
            transform.SetParent(Space2D.transform);
            //GetComponent<Image>().color = spriteColor;
            GameData.amountBalls++;
            Destroy(Sprite);
        }
    }

    private void StartFalling()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.simulated = true;
        rigid.gravityScale = 0.5f;
    }

    public void DeathZone()
    {
        if (!collected)
        {
            StartFalling();
            LoseBall = true;
        }
    }

    public void DeathLevel()
    {
        if(LoseBall)
        {
            Destroy(this.gameObject);
            if (!LevelManager.Instance.listFreeConts.Contains(GetComponentInParent<Container>()))
                LevelManager.Instance.listFreeConts.Remove(GetComponentInParent<Container>());
        }
        else
        {
            rigid.simulated = false;

            GameController.Instance.amountCollectBallsLeft--;
            GameController.Instance.IsAllBallLanded();

            GetComponent<Image>().SetTransparency(0.1f);

            GameObject go = Instantiate(despawnEFX, rectPos) as GameObject;
            Destroy(go, 1f);

            rectPos.position = new Vector2(rectPos.position.x, BallInit.Instance.ballOrgYPos);

            //transform.position = new Vector2(transform.position.x, transform.position.y);

            Destroy(this.gameObject, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (isByBonus)
        {
            return;
        }
        else
        {
            if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy) || coll.gameObject.CompareTag(Tags.ballSQLine))
                Collect();
            else if (coll.gameObject.CompareTag(Tags.EndLevel))
                DeathLevel();
        }
    }
}
