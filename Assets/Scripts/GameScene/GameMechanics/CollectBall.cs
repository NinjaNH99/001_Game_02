using UnityEngine;
using UnityEngine.UI;

public class CollectBall : MonoSingleton<CollectBall>
{
    public GameObject Sprite;
    public GameObject AddBallUIPr;
    public GameObject AddBallEFX;

    public bool isByBonus = false;

    private bool collected, ballIsLanded, LoseBall, isDestroy;

    private Rigidbody2D rigid;
    private GameObject Space2D;
    private Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //GetComponent<Image>().color = spriteColor;
        Space2D = GameObject.FindGameObjectWithTag(Tags.Space2D);
        collected = false;
        ballIsLanded = false;
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

    private void Update()
    {
        if (ballIsLanded)
        {
            if (GameController.Instance.firstBallLanded)
            {
                gameObject.transform.position = Vector3.MoveTowards(new Vector3(gameObject.transform.position.x, BallInit.Instance.ballOrgYPos, 0), BallInit.Instance.targetBallPosLanded, Time.deltaTime * 4.0f);
            }
            else
                gameObject.transform.position = new Vector3(transform.position.x, BallInit.Instance.ballOrgYPos, 0);
            if ((Vector2)gameObject.transform.position == BallInit.Instance.targetBallPosLanded)
            {
                GameController.Instance.amountCollectBallsLeft--;
                GameController.Instance.IsAllBallLanded();
                Destroy(this.gameObject);
            }
        }
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

            GetComponentInParent<Row>().nrSpace++;
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
            Ball.Instance.CollectBall();
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
            if (!LevelManager.Instance.listFreeConts.Contains(GetComponentInParent<Container>().gameObject))
                LevelManager.Instance.listFreeConts.Remove(GetComponentInParent<Container>().gameObject);
        }
        else
        {
            ballIsLanded = true;
            transform.position = new Vector2(transform.position.x, transform.position.y);
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
