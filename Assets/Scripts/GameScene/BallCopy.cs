using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : Ball
{
    public GameController ballOr;
    [HideInInspector]
    public bool ballIsLanded;
    public Vector2 ballPos;

    private RectTransform statePos;
    private Vector2 ballCopPos;
    private Collider2D ballCopyCol;

    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<Image>().color = GameController.Instance.ballCopyColor;
        ballIsLanded = false;
    }

    protected override void Start()
    {
        base.Start();
        //rectPos = this.gameObject.GetComponent<RectTransform>();
        rectPos.position = ballPos;
        Debug.Log("ballCopPosStart :  " + ballPos);
    }

    private void Update()
    {
        if (ballIsLanded)
        {
            //if (!Instance.firstBallLanded)
                //rectPos.position = statePos;
            if(Instance.firstBallLanded)
            {
                gameObject.transform.position = Vector2.MoveTowards(new Vector2(gameObject.transform.position.x, GameController.ballOrgYPos), BallOrg.ballPosFolled, Time.deltaTime * speed);
                if ((Vector2)gameObject.transform.position == BallOrg.ballPosFolled)
                {
                    GameController.Instance.IsAllBallLanded();
                    //Destroy(gameObject);
                    ResetSpeed();
                    ballIsLanded = false;
                    //Debug.Log("ballCopPosInCycle :  " + ballPos);
                    //rectPos.position = ballPos;
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
        //ballCopPos = gameObject.transform.position;
        ballIsLanded = true;
        //rectPos.position = new Vector2(rectPos.position.x, GameController.ballOrgYPos);
        //statePos.position = new Vector2(ballCopPos.x, GameController.ballOrgYPos);
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
