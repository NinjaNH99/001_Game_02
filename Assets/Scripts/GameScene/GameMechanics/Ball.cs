using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ball : MonoSingleton<Ball>
{
    [HideInInspector]
    public bool allBallLanded, startFall, enterTeleport;
    [HideInInspector]
    public float speed;
    [HideInInspector]

    public Rigidbody2D rigid;
    protected RectTransform rectPos;

    public float lastPosXTop;
    public int countContactPosXTop; 

    protected virtual void Awake()
    {
        rectPos = GetComponent<RectTransform>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0.001f;
        rigid.simulated = true;
        enterTeleport = true;
        startFall = false;
        lastPosXTop = 0;
        countContactPosXTop = 5;
        //checkPosX = 5;
    }

    protected virtual void Start()
    {
        ResetSpeed();
    }

    public virtual void SendBallInDirection(Vector2 dir)
    {
        rigid.simulated = true;
        rigid.gravityScale = 0.001f;
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    protected virtual void TouchFloor()
    {
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        rectPos.position = new Vector2(rectPos.position.x, BallInit.Instance.ballOrgYPos);
        enterTeleport = true;
        ResetSpeed();
    }

    protected virtual void DeblockOnPosX(float concatctPOs)
    {
        if(lastPosXTop != concatctPOs)
        {
            lastPosXTop = concatctPOs;
            return;
        }

        countContactPosXTop--;
        if(countContactPosXTop <= 0)
        {
            Debug.Log("Ball block on pos X");
            if (rectPos.position.x > 0)
            {
                rigid.AddForce(new Vector2(-0.01f, 0) * speed, ForceMode2D.Impulse);
                countContactPosXTop = 5;
                Debug.Log("AddForce(-0.01f, 0)");
            }
            else
            {
                rigid.AddForce(new Vector2(0.01f, 0) * speed, ForceMode2D.Impulse);
                countContactPosXTop = 5;
                Debug.Log("AddForce(0.01f, 0)");
            }
            ResetSpeed();
        }
    }

    protected virtual void ResetSpeed()
    {
        rigid.velocity = rigid.velocity.normalized * speed;
    }

    public virtual void CollectBall()
    {
        GameData.amountBalls++;
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Floor))
        {
            TouchFloor();
        }
        
        if (coll.gameObject.CompareTag(Tags.WallT))
        {
            DeblockOnPosX(rectPos.position.x);
        }
        ResetSpeed();
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Square_Teleport))
        {
            if (enterTeleport)
                rectPos.position = LevelManager.Instance.Teleports(coll.GetComponentInParent<Square_01>().gameObject.GetComponent<RectTransform>());
            enterTeleport = false;
        }
        ResetSpeed();
    }

}
