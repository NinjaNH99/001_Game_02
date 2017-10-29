using System;
using UnityEngine;

public class BallOrg : Ball
{
    public GameObject circleAnim;
    public static Vector2 ballPosFolled;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
        Instance.firstBallLanded = false;
        circleAnim.GetComponent<Animator>().SetTrigger("isShoot");
    }

    protected override void TouchFloor()
    {
        base.TouchFloor();
        Instance.firstBallLanded = true;
        GameController.Instance.IsAllBallLanded();
        circleAnim.GetComponent<Animator>().SetTrigger("isFell");
        ballPosFolled = rectPos.position;
        ResetSpeed();
    }

    protected override void StartFall()
    {
        base.StartFall();
    }

    protected override void ResetSpeed()
    {
        base.ResetSpeed();
    }

    public override void CollectBall()
    {
        base.CollectBall();
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        base.OnTriggerEnter2D(coll);
    }

}
