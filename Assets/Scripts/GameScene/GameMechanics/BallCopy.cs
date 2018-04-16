using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : Ball
{
    private bool ballIsLanded = false;

    protected override void Awake()
    {
        base.Awake();
        EventManager.EvMoveDownM += TurnOFFBall;
        gameObject.GetComponent<Image>().color = BallInit.Instance.ballCopyColor;
        ballIsLanded = false;
        speed = BallInit.Instance.ballSpeedGet;

        rectPos.position = BallInit.Instance.targetBallPosLanded;
        Start();
    }

    protected override void Start()
    {
        SendBallInDirection(BallInit.Instance.shootDir);
        ResetSpeed();
        //rectPos.position = ballPos;
    }

    private void OnEnable()
    {        Awake();    }

    private void Update()
    {
        if (ballIsLanded)
        {
            if (GameController.Instance.firstBallLanded)
            {
                gameObject.transform.position = Vector2.MoveTowards(new Vector2(gameObject.transform.position.x, BallInit.Instance.ballOrgYPos), BallInit.Instance.targetBallPosLanded, Time.deltaTime * speed);
                if ((Vector2)gameObject.transform.position == BallInit.Instance.targetBallPosLanded)
                {
                    GameController.Instance.IsAllBallLanded();
                    ResetSpeed();
                    ballIsLanded = false;
                    //if(!DisplayAtFloor)
                       // gameObject.SetActive(false);
                }
            }
        }
    }

    private void TurnOFFBall()
    {
        EventManager.EvMoveDownM -= TurnOFFBall;
        gameObject.SetActive(false);
    }

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
    }

    protected override void TouchFloor()
    {
        base.TouchFloor();
        rectPos.position = new Vector2(rectPos.position.x, BallInit.Instance.ballOrgYPos);
        GameController.Instance.FirstBallLanded(gameObject.GetComponent<RectTransform>().position);
        ballIsLanded = true;
        ResetSpeed();
    }

    public override void CollectBall()
    {
        base.CollectBall();
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
