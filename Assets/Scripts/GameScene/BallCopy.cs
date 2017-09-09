using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : MonoSingleton<BallCopy>
{
    public Vector2 ballPos;
    public bool ballIsLanded;
    //public Vector2 dir;
    public float speed;

    private Ball ballOr;
    private Vector2 lastColPosL;
    private Vector2 lastColPosR;
    private Vector2 ballCopPos;
    private Vector2 statePos;
    private bool stopBlocked;
    private bool doNotCheckL, doNotCheckR;
    private Rigidbody2D rigid;
    private Collider2D ballCopyCol;
    private RectTransform rectPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
        rigid.simulated = true;
        //speed = GameController.speed;
        ballOr = Ball.Instance;
        lastColPosL = lastColPosR = Vector2.zero;
        gameObject.GetComponent<Image>().color = GameController.Instance.ballCopyColor;
        ballIsLanded = false;
    }

    private void Start()
    {
        rectPos = GetComponent<RectTransform>();
        rectPos.position = ballPos;
        doNotCheckL = doNotCheckR = false;
        //SendBallInDirection();
        //Debug.Log("Copy :" + rigid.velocity.magnitude);
    }

    private void Update()
    {
        if (ballIsLanded)
        {
            if (!ballOr.firstBallLanded)
                rectPos.position = statePos;
            else
            {
                gameObject.transform.position = Vector2.MoveTowards(new Vector2(gameObject.transform.position.x, GameController.ballOrgYPos), ballOr.transform.position, Time.deltaTime * speed);
                if (gameObject.transform.position == ballOr.transform.position)
                {
                    GameController.Instance.IsAllBallLanded();
                    Destroy(gameObject);
                }
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
        statePos = new Vector2(ballCopPos.x, GameController.ballOrgYPos);
        ballIsLanded = true;
    }

    public void CollectBall()
    {
        GameController.amountBalls++;
    }

    private void IfIsBlocked(bool left, bool right)
    {
        if (left && !doNotCheckL)
        {
            lastColPosL = gameObject.transform.position;
            doNotCheckL = true;
        }
        else if (right && !doNotCheckR)
        {
            lastColPosR = gameObject.transform.position;
            doNotCheckR = true;
        }
        if (doNotCheckL && doNotCheckR)
        {
            if (Math.Round(lastColPosL.y, 1) == Math.Round(lastColPosR.y, 1))
            {
                rigid.gravityScale = 0.02f;
            }
            else
            {
                rigid.gravityScale = 0;
            }
            doNotCheckL = doNotCheckR = false;
        }

    }

    private void StartFall()
    {
        if (TimerGravity.Instance.startFall)
            rigid.gravityScale = 0.04f;
    }

    private void ResetSpeed()
    {
        rigid.velocity = rigid.velocity.normalized * speed;
        //Debug.Log("Copy :" + rigid.velocity.magnitude);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Floor))
        {
            TouchFloor();
        }
        
        if (coll.gameObject.CompareTag(Tags.Wall))
        {
            IfIsBlocked(true, false);
        }
        if (coll.gameObject.CompareTag(Tags.WallR))
        {
            IfIsBlocked(false, true);
        }
        StartFall();
        ResetSpeed();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Square_01))
        {
            ResetSpeed();
        }
    }

}
