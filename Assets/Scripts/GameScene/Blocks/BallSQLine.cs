using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSQLine : Ball
{
    [HideInInspector]
    public Vector2 ballPos;

    //private RectTransform statePos;
    private Collider2D ballCopyCol;

    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<Image>().color = gameContr.ballCopyColor;
    }

    protected override void Start()
    {
        base.Start();
        //rectPos.position = ballPos;
    }

    private void OnEnable()
    {
        Start();
    }

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
    }

    protected override void ResetSpeed()
    {
        base.ResetSpeed();
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Wall) || coll.gameObject.CompareTag(Tags.WallR) || coll.gameObject.CompareTag(Tags.Square) || coll.gameObject.CompareTag(Tags.Square_01))
        {
            Destroy(gameObject, 0.1f);
        }

        ResetSpeed();
    }

}
