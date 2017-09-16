using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Bonus_02 : MonoSingleton<Ball_Bonus_02>
{
    public GameObject DeathEFX;
    public GameObject DestroyEFX;
    public Vector2 ballPos;

    public Rigidbody2D rigid;
    public RectTransform rectPos;

    private GameObject Space2D;

    private void Start()
    {
        rigid.gravityScale = 0f;
        rigid.simulated = true;
        rectPos.position = ballPos;
        Space2D = GameObject.FindGameObjectWithTag(Tags.Space2D);
    }

    public void SendBallInDirection(Vector2 dir, float speed)
    {
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
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
    
    void OnCollisionEnter2D(Collision2D coll)
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
