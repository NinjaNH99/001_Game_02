using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ball : MonoBehaviour
{
    [HideInInspector]
    public bool allBallLanded, startFall, enterTeleport;
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
        startFall = enterTeleport = false;
        lastPosXTop = 0;
        countContactPosXTop = 5;
        enterTeleportCount = 10;
        rectPos.sizeDelta = new Vector2(17.5f, 17.5f);
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
        Debug.Log("TouchFloor");

        rigid.velocity = Vector2.zero;
        rigid.simulated = enterTeleport = false;
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
            var forceX = UnityEngine.Random.Range(0.01f, 0.05f);
            Debug.Log("Ball block on pos X");
            if (rectPos.position.x > 0)
            {
                rigid.AddForce(new Vector2(-forceX, 0) * speed, ForceMode2D.Impulse);
                countContactPosXTop = 5;
                Debug.Log("AddForce(" + (-forceX) + " , 0)");
            }
            else
            {
                rigid.AddForce(new Vector2(forceX, 0) * speed, ForceMode2D.Impulse);
                countContactPosXTop = 5;
                Debug.Log("AddForce(" + forceX + " , 0)");
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

    protected virtual void Teleport()
    {
        animator.SetTrigger("OutTeleport");
        rectPos.position = LevelManager.Instance.Teleports();
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.TeleportIn))
        {
            if (enterTeleportCount > 0)
            {
                enterTeleportCount--;
                enterTeleport = true;
                rectPos.position = LevelManager.Instance.Teleports();
                //animator.SetTrigger("OutTeleport");
                //animator.SetTrigger("InTeleport");      
            }
            else
                enterTeleportCount = 10;
        }
        /*
        else if(coll.gameObject.CompareTag(Tags.TeleportOut))
        {
            if (enterTeleport)
            {
                Debug.Log("Out");
                //animator.SetTrigger("OutTeleport");
                enterTeleport = false;
            }
        }*/
        ResetSpeed();
    }

}
