using System;
using UnityEngine;
using UnityEngine.UI;

public class BallCopy : Ball
{
    public GameObject despawnEFX;

    private GameObject Space2D;

    protected override void Awake()
    {
        base.Awake();
        rectPos.sizeDelta = new Vector2(17.5f, 17.5f);
        gameObject.GetComponent<Image>().color = BallInit.Instance.ballCopyColor;
        //speed = BallInit.Instance.ballSpeedGet;

        rectPos.position = BallInit.Instance.shootBallPos;
        Space2D = GameObject.FindGameObjectWithTag(Tags.Space2D);
        Start();
    }

    protected override void Start()
    {
        SendBallInDirection(BallInit.Instance.shootDir);
        ResetSpeed();
    }

    private void OnEnable()
    {        Awake();    }


    private void Despawn()
    {
        rectPos.sizeDelta = new Vector2(17.5f, 17.5f);
        gameObject.SetActive(false);
    }

    protected override void TouchFloor()
    {
        base.TouchFloor();
        rectPos.position = new Vector2(rectPos.position.x, BallInit.Instance.ballOrgYPos);
        ResetSpeed();

        GameController.Instance.IsAllBallLanded();

        GameObject go = Instantiate(despawnEFX, this.rectPos) as GameObject;
        go.transform.SetParent(Space2D.transform);
        Destroy(go, 1f);

        Despawn();
        //animator.SetTrigger("Despawn");
    }

}
