using System;
using UnityEngine;

public class BallOrg : Ball
{
    public GameObject circleAnim;
    public static Vector2 ballPosFolled;

    private bool ballIsLanded;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (ballIsLanded)
        {
            if (GameController.firstBallLanded)
            {
                //Debug.Log("Instance.firstBallLanded Copy : " + Instance.firstBallLanded);
                gameObject.transform.position = Vector2.MoveTowards(new Vector2(this.gameObject.transform.position.x, GameController.ballOrgYPos), /*BallOrg.ballPosFolled*/ GameController.Instance.targetBallPosLanded, Time.deltaTime * speed);
                if ((Vector2)gameObject.transform.position == /*BallOrg.ballPosFolled*/ GameController.Instance.targetBallPosLanded)
                {
                    GameController.Instance.IsAllBallLanded();
                    ResetSpeed();
                    ballIsLanded = false;
                    //gameObject.SetActive(false);
                }
            }
        }
    }

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
        //Instance.firstBallLanded = false;
        circleAnim.GetComponent<Animator>().SetTrigger("isShoot");
    }

    protected override void TouchFloor()
    {
        base.TouchFloor();
        //Instance.firstBallLanded = true;
        //Debug.Log("First Ball Landed :" + firstBallLanded);
        GameController.Instance.IsAllBallLanded();
        circleAnim.GetComponent<Animator>().SetTrigger("isFell");
        //ballPosFolled = rectPos.position;
        GameController.Instance.FirstBallLanded(this.gameObject.GetComponent<RectTransform>().position);
        ballIsLanded = true;
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
