using System;
using UnityEngine;

public class BallOrg : Ball
{
    public GameObject circleAnim;

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
        circleAnim.GetComponent<Animator>().SetTrigger("isShoot");
    }

    protected override void TouchFloor()
    {
        base.TouchFloor();
        circleAnim.GetComponent<Animator>().SetTrigger("isFell");
        GameController.Instance.BallLanded(gameObject.GetComponent<RectTransform>().position);

        GameController.Instance.IsAllBallLanded();
        speed = BallInit.Instance.ballSpeedGet;
        ResetSpeed();
    }

}
