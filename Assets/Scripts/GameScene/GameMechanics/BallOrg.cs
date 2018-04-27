using System;
using UnityEngine;

public class BallOrg : Ball
{
    public GameObject circleAnim;
    //public static Vector2 ballPosFolled;

    //private bool ballIsLanded = false;

    protected override void Awake()
    {
        base.Awake();
        //ballIsLanded = false;
        //rectPos.position = new Vector2(GameData.posXBall, -1.5f);
    }

    protected override void Start()
    {
        base.Start();
    }

    /*
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
                }
            }
        }
    }*/

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
        circleAnim.GetComponent<Animator>().SetTrigger("isShoot");
    }

    protected override void TouchFloor()
    {
        base.TouchFloor();
        circleAnim.GetComponent<Animator>().SetTrigger("isFell");
        GameController.Instance.FirstBallLanded(gameObject.GetComponent<RectTransform>().position);
        //ballIsLanded = true;

        GameController.Instance.IsAllBallLanded();

        ResetSpeed();
    }

    protected override void ResetSpeed()
    {
        base.ResetSpeed();
    }

    protected override void Teleport()
    {
        base.Teleport();
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
