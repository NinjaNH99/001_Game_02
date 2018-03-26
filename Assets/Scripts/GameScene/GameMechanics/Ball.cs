using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ball : MonoSingleton<Ball>
{
    [HideInInspector]
    public bool allBallLanded, startFall;//, firstBallLanded = false;
    [HideInInspector]
    public float speed;
    [HideInInspector]

    public Rigidbody2D rigid;
    protected RectTransform rectPos;
    protected GameController gameContr;
    protected float currentSpawnY;
    protected int checkPosX;

    protected virtual void Awake()
    {
        gameContr = GameController.Instance;
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        rigid.simulated = true;
        checkPosX = 5;
    }

    protected virtual void Start()
    {
        Awake();
        //firstBallLanded = false;
        startFall = false;
        rectPos = GetComponent<RectTransform>();
        ResetSpeed();
    }

    public virtual void SendBallInDirection(Vector2 dir)
    {
        rigid.simulated = true;
        rigid.gravityScale = 0;
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    protected virtual void TouchFloor()
    {
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        rectPos.position = new Vector2(rectPos.position.x, gameContr.ballOrgYPos);
        checkPosX = 5;
        ResetSpeed();
    }

    protected virtual void StartFall()
    {
        checkPosX--;
        if (checkPosX <= 0)
        {
            if (rectPos.position.x > 0)
            {
                rigid.AddForce(new Vector2(-0.01f, -0.01f) * speed, ForceMode2D.Impulse);
                checkPosX = 5;
            }
            else
            {
                rigid.AddForce(new Vector2(0.01f, -0.01f) * speed, ForceMode2D.Impulse);
                checkPosX = 5;
            }
            ResetSpeed();
        }

        if (startFall)
        {
            rigid.gravityScale = 0.1f;
            startFall = false;
        }
    }

    protected virtual void ResetSpeed()
    {
        rigid.velocity = rigid.velocity.normalized * speed;
    }

    public virtual void CollectBall()
    {
        gameContr.amountBalls++;
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Floor))
        {
            TouchFloor();
        }
        /*
        if (coll.gameObject.CompareTag(Tags.WallT))
        {
            StartFall();
        }*/
        if (coll.gameObject.CompareTag(Tags.Wall) || coll.gameObject.CompareTag(Tags.WallR))
        {
            rigid.AddForce(new Vector2(0, -0.0005f) * speed, ForceMode2D.Impulse);
        }

        StartFall();
        ResetSpeed();
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {        ResetSpeed();    }

}
