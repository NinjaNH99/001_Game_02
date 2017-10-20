using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : MonoSingleton<BallCopy>
{
    public Vector2 ballPos;
    public bool ballIsLanded;
    //public Vector2 dir;
    public float speed;
    public static bool startFall;

    private Ball ballOr;
    private Vector2 lastColPosL, lastColPosR, ballCopPos, statePos, BallDir;
    private bool doNotCheckL, doNotCheckR, stopBlocked;
    private Rigidbody2D rigid;
    private Collider2D ballCopyCol;
    private RectTransform rectPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.simulated = true;
        rigid.gravityScale = 0;
        ballOr = Ball.Instance;
        lastColPosL = lastColPosR = Vector2.zero;
        gameObject.GetComponent<Image>().color = GameController.Instance.ballCopyColor;
        ballIsLanded = false;
        startFall = false;
    }

    private void Start()
    {
        rectPos = GetComponent<RectTransform>();
        rectPos.position = ballPos;
        doNotCheckL = doNotCheckR = false;
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
        BallDir = dir;
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
    /*
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
                rigid.gravityScale = 0.04f;
            }
            else
            {
                rigid.gravityScale = 0.002f;
            }
            doNotCheckL = doNotCheckR = false;
        }

    }
    */
    private void StartFall()
    {
        if (rectPos.position.x > 0)
        {
            rigid.AddForce(new Vector2(-0.1f, 0) * speed, ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(new Vector2(0.1f, 0) * speed, ForceMode2D.Impulse);
        }
        ResetSpeed();

        if (startFall)
        {
            rigid.gravityScale = 0.04f;
            startFall = false;
        }
    }

    private void ResetSpeed()
    {
        rigid.velocity = rigid.velocity.normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Floor))
        {
            TouchFloor();
        }
        

        if(coll.gameObject.CompareTag(Tags.WallT))
        {
            StartFall();
            Debug.Log("+");
        }
        if (coll.gameObject.CompareTag(Tags.Wall))
        {
            //IfIsBlocked(true, false);
            //rigid.AddForce(Vector2.Reflect(rigid.velocity, coll.contacts[0].normal) * speed, ForceMode2D.Impulse);
            rigid.AddForce(new Vector2(0, -0.01f) * speed, ForceMode2D.Impulse);
        }
        if (coll.gameObject.CompareTag(Tags.WallR))
        {
            //rigid.AddForce(Vector2.Reflect(rigid.velocity , coll.contacts[0].normal) * speed, ForceMode2D.Impulse);
            //IfIsBlocked(false, true);
            rigid.AddForce(new Vector2(-0, -0.01f) * speed, ForceMode2D.Impulse);
        }

        //StartFall();
        ResetSpeed();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        ResetSpeed();
    }

}
