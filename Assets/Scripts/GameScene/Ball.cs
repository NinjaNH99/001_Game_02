using System;
using UnityEngine;

public class Ball : MonoSingleton<Ball>
{
    public GameObject circleAnim;

    [HideInInspector]
    public bool firstBallLanded, allBallLanded;
    [HideInInspector]
    public float speed;

    private Vector2 lastColPosL;
    private Vector2 lastColPosR;
    private Vector2 landingPosition;

    private Rigidbody2D rigid;
    private RectTransform rectPos;

    private float currentSpawnY;
    private bool doNotCheckL, doNotCheckR;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
        rigid.simulated = true;
    }

    private void Start()
    {
        firstBallLanded = false;
        rectPos = GetComponent<RectTransform>();
        lastColPosL = lastColPosR = Vector2.zero;
        doNotCheckL = doNotCheckR = false;
        //Debug.Log("Ball " + rigid.velocity.magnitude);
    }

    public void SendBallInDirection(Vector2 dir)
    {
        firstBallLanded = false;
        rigid.simulated = true;
        rigid.gravityScale = 0;
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
        circleAnim.GetComponent<Animator>().SetTrigger("isShoot");
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
        if (coll.gameObject.CompareTag(Tags.Wall))
        {
            IfIsBlocked(true, false);
        }
        if (coll.gameObject.CompareTag(Tags.WallR))
        {
            IfIsBlocked(false, true);
        }
        ResetSpeed();
        StartFall();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        ResetSpeed();
    }

}
