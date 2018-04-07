using UnityEngine;
using UnityEngine.UI;

public class CollectBall : MonoSingleton<CollectBall>
{
    public GameObject Sprite;
    public GameObject AddBallUIPr;
    public GameObject AddBallEFX;

    public bool isByBonus = false;

    private GameController gameContr;
    private bool collected;
    private bool ballIsLanded;
    private bool LoseBall;
    private bool isDestroy;
    private Rigidbody2D rigid;
    private Color spriteColor;
    private GameObject Space2D;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        GetComponent<Image>().color = spriteColor;
        Space2D = GameObject.FindGameObjectWithTag(Tags.Space2D);
        collected = false;
        ballIsLanded = false;
        LoseBall = false;
        isDestroy = true;
    }

    private void Start()
    {
        gameContr = GameController.Instance;
        spriteColor = BallOrg.Instance.GetComponent<Image>().color;

        if(isByBonus)
        {
            EventManager.EvMethods += CollectByBons;
        }
    }

    private void Update()
    {
        if (ballIsLanded)
        {
            if (gameContr.firstBallLanded)
            {
                gameObject.transform.position = Vector3.MoveTowards(new Vector3(gameObject.transform.position.x, gameContr.ballOrgYPos, 0), gameContr.targetBallPosLanded, Time.deltaTime * 4.0f);
            }
            else
                gameObject.transform.position = new Vector3(transform.position.x, gameContr.ballOrgYPos, 0);
            if ((Vector2)gameObject.transform.position == gameContr.targetBallPosLanded)
            {
                gameContr.amountCollectBallsLeft--;
                gameContr.IsAllBallLanded();
                Destroy(this.gameObject);
            }
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
            if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy) || coll.gameObject.CompareTag(Tags.ballSQLine))
                Collect();
            else if (coll.gameObject.CompareTag(Tags.EndLevel))
                DeathLevel();
        }
    }

    private void CollectByBons()
    {
        isByBonus = false;
        EventManager.EvMethods -= CollectByBons;
    }

    private void Collect()
    {
        if (!collected)
        {
            collected = true;
            gameContr.amountCollectBallsLeft++;
            GameObject go = Instantiate(AddBallEFX, gameObject.transform) as GameObject;
            Destroy(go, 2f);
            StartFalling();
            if (isDestroy)
            {
                gameContr.AddBallUI++;
                var AmountBallUIPos = gameContr.amountBallsTextPr.transform;
                GameObject goEFX = Instantiate(AddBallUIPr, AmountBallUIPos) as GameObject;
                goEFX.GetComponent<RectTransform>().localPosition = new Vector2(-95, -95);
                Destroy(goEFX, 1f);
                gameContr.amountBallsLeft++;
                isDestroy = false;
            }
            rigid.velocity = new Vector2(0, 0.5f) * 2.0f;
            spriteColor.a = 0.8f;
            transform.SetParent(Space2D.transform);
            GetComponent<Image>().color = spriteColor;
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
        }
        else
        {
            ballIsLanded = true;
            transform.position = new Vector2(transform.position.x, transform.position.y);
        }
    }
}
