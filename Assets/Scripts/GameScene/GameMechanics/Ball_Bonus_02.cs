using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Bonus_02 : Ball
{
    public GameObject DeathEFX;
    public GameObject DestroyEFX;
    public Vector2 ballPos;

    private GameObject Space2D;

    protected override void Start()
    {
        base.Start();
        rectPos.position = ballPos;
        Space2D = GameObject.FindGameObjectWithTag(Tags.Space2D);
    }

    public override void SendBallInDirection(Vector2 dir)
    {
        base.SendBallInDirection(dir);
    }

    private void Death(bool op)
    {
        switch (op)
        {
            case true:
                {
                    GameObject go = Instantiate(DeathEFX, Space2D.transform) as GameObject;
                    go.transform.position = gameObject.transform.position;
                    gameObject.SetActive(false);

                    Destroy(go, 1f);
                    Destroy(gameObject, 1f);

                    break;
                }
            case false:
                {
                    GameObject go = Instantiate(DestroyEFX, Space2D.transform) as GameObject;
                    go.transform.position = gameObject.transform.position;
                    gameObject.SetActive(false);

                    Destroy(go, 1f);
                    Destroy(gameObject, 1f);

                    break;
                }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag(Tags.Square))
        {
            Death(true);
        }
        if(coll.gameObject.CompareTag(Tags.EndLevel))
        {
            Death(false);
        }
    }

}
