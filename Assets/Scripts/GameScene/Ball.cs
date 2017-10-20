using System;
using UnityEngine;

public class Ball : MonoSingleton<Ball>
{
    public GameObject circleAnim;

    [HideInInspector]
    public bool firstBallLanded, allBallLanded, startFall;
    [HideInInspector]
    public float speed;

    private Vector2 lastColPosL, lastColPosR, landingPosition, BallDir;

    private Rigidbody2D rigid;
    private RectTransform rectPos;

    private float currentSpawnY;
    private bool doNotCheckL, doNotCheckR;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        rigid.simulated = true;
    }

    private void Start()
    {
        firstBallLanded = false;
        rectPos = GetComponent<RectTransform>();
        lastColPosL = lastColPosR = Vector2.zero;
        doNotCheckL = doNotCheckR = startFall = false;
    }

    public void SendBallInDirection(Vector2 dir)
    {
        firstBallLanded = false;
        rigid.simulated = true;
        rigid.gravityScale = 0;
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
        circleAnim.GetComponent<Animator>().SetTrigger("isShoot");
        BallDir = dir;
    }

    private void TouchFloor()
    {
        firstBallLanded = true;
        GameController.Instance.IsAllBallLanded();
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        // Reload position Y
        rectPos.position = new Vector2(rectPos.position.x, GameController.ballOrgYPos);
        circleAnim.GetComponent<Animator>().SetTrigger("isFell");
        ResetSpeed();
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
                rigid.gravityScale = 0.02f;
            }
            else
            {
                rigid.gravityScale = 0.002f;
            }
            doNotCheckL = doNotCheckR = false;
        }

    }*/

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
        //Debug.Log("Copy :" + rigid.velocity.magnitude);
    }

    public void CollectBall()
    {
        GameController.amountBalls++;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Floor))
        {
            TouchFloor();
        }

        if (coll.gameObject.CompareTag(Tags.WallT))
        {
            StartFall();
            Debug.Log("+");
        }
        if (coll.gameObject.CompareTag(Tags.Wall))
        {
            //IfIsBlocked(true, false);
            //rigid.AddForce(Vector2.Reflect(rigid.velocity.normalized, coll.contacts[0].normal) * speed, ForceMode2D.Impulse);
            rigid.AddForce(new Vector2(0, -0.01f) * speed, ForceMode2D.Impulse);
        }
        if (coll.gameObject.CompareTag(Tags.WallR))
        {
            //rigid.AddForce(Vector2.Reflect(rigid.velocity.normalized, coll.contacts[0].normal) * speed, ForceMode2D.Impulse);
            //rigid.velocity = Vector2.Reflect(rigid.velocity.normalized, coll.contacts[0].normal);
            //IfIsBlocked(false, true);
            rigid.AddForce(new Vector2(-0, -0.01f) * speed, ForceMode2D.Impulse);
        }

        ResetSpeed();
        //StartFall();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        ResetSpeed();
    }

}
