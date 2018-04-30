using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ball : MonoBehaviour
{
    [HideInInspector]
    public bool allBallLanded, startFall, enterTeleport, isShoot = false;
    [HideInInspector]
    public float speed;
    [HideInInspector]

    public Rigidbody2D rigid;
    protected RectTransform rectPos;

    public float lastPosXTop;
    public int countContactPosXTop = 5, enterTeleportCount = 10;

    protected Animator animator;

    protected virtual void Awake()
    {
        rectPos = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0.001f;
        rigid.simulated = true;
        rigid.velocity = Vector2.zero;
        startFall = enterTeleport = isShoot = false;
        lastPosXTop = 0;
        countContactPosXTop = 5;
        enterTeleportCount = 10;
    }

    protected virtual void Start()
    {
        ResetSpeed();
    }

    public virtual void SendBallInDirection(Vector2 dir)
    {
        speed = BallInit.Instance.ballSpeedGet;
        rigid.simulated = true;
        rigid.gravityScale = 0.001f;
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
        isShoot = true;
    }

    protected virtual void TouchFloor()
    {
        rigid.simulated = enterTeleport = isShoot = false;
        rigid.velocity = Vector2.zero;
        rectPos.position = new Vector2(rectPos.position.x, BallInit.Instance.ballOrgYPos);
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
            if (rectPos.position.x > 0)
            {
                rigid.AddForce(new Vector2(-0.04f, 0) * speed, ForceMode2D.Impulse);
                countContactPosXTop = 5;
            }
            else
            {
                rigid.AddForce(new Vector2(0.04f, 0) * speed, ForceMode2D.Impulse);
                countContactPosXTop = 5;
            }
            ResetSpeed();
        }
    }

    protected virtual void ResetSpeed()
    {
        rigid.velocity = rigid.velocity.normalized * speed;
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
        if (coll.gameObject.CompareTag(Tags.TeleportIn))
        {
            if (enterTeleportCount > 0)
            {
                enterTeleportCount--;
                enterTeleport = true;
                rectPos.position = LevelManager.Instance.teleportOut.position;
            }
            else
                enterTeleportCount = 10;
        }
        ResetSpeed();
    }

}
