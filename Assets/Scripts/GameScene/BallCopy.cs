using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : MonoSingleton<BallCopy>
{
    public GameObject ballCopyPr;
    public Vector2 ballPos;
    public bool ballIsLanded;

    private Ball ballOr;
    private Vector2 lastColPosL;
    private Vector2 lastColPosR;
    private Vector2 ballCopPos;
    private bool stopBlocked;
    private Rigidbody2D rigid;
    private Collider2D ballCopyCol;
    private float speed;
    private RectTransform rectPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
        rigid.simulated = true;
        speed = GameController.speed;
        ballOr = Ball.Instance;
        lastColPosL = lastColPosR = Vector2.zero;
        gameObject.GetComponent<Image>().color = GameController.Instance.ballCopyColor;
        ballIsLanded = false;
    }

    private void Start()
    {
        //transform.position = ballPos;
        rectPos = GetComponent<RectTransform>();
        rectPos.position = ballPos;
    }

    private void Update()
    {
        if (ballIsLanded)
        {
            if (!ballOr.firstBallLanded)
                rectPos.position = new Vector2(ballCopPos.x, GameController.ballOrgYPos);
            else
                gameObject.transform.position = Vector2.MoveTowards(new Vector2(gameObject.transform.position.x, GameController.ballOrgYPos), ballOr.transform.position, Time.deltaTime * speed);
            if (gameObject.transform.position == ballOr.transform.position)
            {
                GameController.Instance.IsAllBallLanded(true);
                Destroy(gameObject);
            }
        }
    }

    public void SendBallInDirection(Vector2 dir)
    {
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    private void TouchFloor()
    {
        ballCopPos = gameObject.transform.position;
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        ballIsLanded = true;
    }

    public void CollectBall()
    {
        GameController.amountBalls++;
    }

    private void IfIsBlocked()
    {
        if (Math.Round(lastColPosL.y, 1) == Math.Round(lastColPosR.y, 1))
        {
            rigid.gravityScale = 0.02f;
        }
        else
            rigid.gravityScale = 0;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Floor))
        {
            TouchFloor();
        }
        if (coll.gameObject.CompareTag(Tags.Square))
        {
            coll.transform.parent.GetComponent<Block>().ReceiveHit();
        }
        if (coll.gameObject.CompareTag(Tags.Wall))
            lastColPosL = gameObject.transform.position;
        if (coll.gameObject.CompareTag(Tags.WallR))
        {
            lastColPosR = gameObject.transform.position;
            IfIsBlocked();
        }
    }

}
