using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSQLine : Ball
{
    [HideInInspector]
    public Vector2 ballPos;
    //public Color color;

    //private RectTransform statePos;
    private Collider2D ballCopyCol;

    protected override void Awake()
    {
        base.Awake();
        //gameObject.GetComponent<Image>().color = color;
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

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Wall) || coll.gameObject.CompareTag(Tags.WallR)
            || coll.gameObject.CompareTag(Tags.Square) || coll.gameObject.CompareTag(Tags.Square_01)
            || coll.gameObject.CompareTag(Tags.WallT) || coll.gameObject.CompareTag(Tags.Floor))
        {
            Destroy(gameObject);
        }
    }

}
