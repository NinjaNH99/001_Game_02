﻿using System;
using UnityEngine;

public class Ball : MonoSingleton<Ball>
{
    public bool firstBallLanded;
    public bool allBallLanded;
    public GameObject circleAnim;
    public float speed;

    private Vector2 lastColPosL;
    private Vector2 lastColPosR;
    private Vector2 landingPosition;

    private Rigidbody2D rigid;
    private RectTransform rectPos;

    private float currentSpawnY;
    private bool doNotCheck;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
        rigid.simulated = true;
        speed = GameController.BALLSPEED;
    }

    private void Start()
    {
        firstBallLanded = false;
        rectPos = GetComponent<RectTransform>();
        lastColPosL = lastColPosR = Vector2.zero;
        doNotCheck = false;
        //Debug.Log("Ball " + rigid.velocity.magnitude);
    }

    public void SendBallInDirection(Vector2 dir)
    {
        firstBallLanded = false;
        rigid.simulated = true;
        rigid.AddRelativeForce(dir * speed, ForceMode2D.Impulse);
        circleAnim.GetComponent<Animator>().SetTrigger("isShoot");
    }

    private void TouchFloor()
    {
        firstBallLanded = true;
        GameController.Instance.IsAllBallLanded(true);
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        // Reload position Y
        rectPos.position = new Vector2(rectPos.position.x, GameController.ballOrgYPos);
        circleAnim.GetComponent<Animator>().SetTrigger("isFell");
    }

    private void IfIsBlocked()
    {
        if (Math.Round(lastColPosL.y, 1) == Math.Round(lastColPosR.y, 1))
        {
            rigid.gravityScale = 0.02f;
            doNotCheck = false;
        }
        else
        {
            rigid.gravityScale = 0;
            doNotCheck = false;
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
        if (coll.gameObject.CompareTag(Tags.Wall) && !doNotCheck)
        {
            lastColPosL = gameObject.transform.position;
            doNotCheck = true;
        }
        if (coll.gameObject.CompareTag(Tags.WallR))
        {
            lastColPosR = gameObject.transform.position;
            IfIsBlocked();
        }
        ResetSpeed();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        ResetSpeed();
    }

}
