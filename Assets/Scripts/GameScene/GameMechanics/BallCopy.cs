using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : Ball
{
    [HideInInspector]
    public Vector2 ballPos;

    private RectTransform statePos;
    private bool ballIsLanded, DisplayAtFloor;
    private Collider2D ballCopyCol;

    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<Image>().color = BallInit.Instance.ballCopyColor;
        ballIsLanded = false;
    }

    protected override void Start()
    {
        base.Start();
        rectPos.position = ballPos;
    }

    private void OnEnable()
    {        Start();    }

    private void Update()
    {
        if (ballIsLanded)
        {
            if (gameContr.firstBallLanded)
            {
                gameObject.transform.position = Vector2.MoveTowards(new Vector2(gameObject.transform.position.x, BallInit.Instance.ballOrgYPos), BallInit.Instance.targetBallPosLanded, Time.deltaTime * speed);
                if ((Vector2)gameObject.transform.position == BallInit.Instance.targetBallPosLanded)
                {
                    gameContr.IsAllBallLanded();
                    ResetSpeed();
                    ballIsLanded = false;
                    if(!DisplayAtFloor)
                        gameObject.SetActive(false);
                }
            }
        }
    }

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
    }

    protected override void TouchFloor()
    {
        base.TouchFloor();
        rectPos.position = new Vector2(rectPos.position.x, BallInit.Instance.ballOrgYPos);
        DisplayAtFloor = gameContr.FirstBallLanded(gameObject.GetComponent<RectTransform>().position);
        ballIsLanded = true;
        ResetSpeed();
    }

    public override void CollectBall()
    {
        base.CollectBall();
    }

    protected override void StartFall()
    {
        base.StartFall();
    }

    protected override void ResetSpeed()
    {
        base.ResetSpeed();
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
