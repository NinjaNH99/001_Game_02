using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : Ball
{
    public GameController ballOr;
    [HideInInspector]
    public Vector2 ballPos;

    private RectTransform statePos;
    private bool ballIsLanded, DisplayAtFloor;
    //private Vector2 ballCopPos;
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
        rectPos.position = ballPos;
    }

    private void OnEnable()
    {
        Start();
    }

    private void Update()
    {
        if (ballIsLanded)
        {
            if(GameController.firstBallLanded)
            {
                //Debug.Log("Instance.firstBallLanded Copy : " + Instance.firstBallLanded);
                gameObject.transform.position = Vector2.MoveTowards(new Vector2(gameObject.transform.position.x, GameController.ballOrgYPos), /*BallOrg.ballPosFolled*/ GameController.Instance.targetBallPosLanded, Time.deltaTime * speed);
                if ((Vector2)gameObject.transform.position == /*BallOrg.ballPosFolled*/ GameController.Instance.targetBallPosLanded)
                {
                    GameController.Instance.IsAllBallLanded();
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
        //ballCopPos = this.gameObject.transform.position;
        rectPos.position = new Vector2(rectPos.position.x, GameController.ballOrgYPos);
        //statePos.position = new Vector2(ballCopPos.x, GameController.ballOrgYPos);
        DisplayAtFloor = GameController.Instance.FirstBallLanded(gameObject.GetComponent<RectTransform>().position);
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
