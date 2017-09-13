using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Bonus_02 : MonoSingleton<Ball_Bonus_02>
{
    public Vector2 ballPos;

    public Rigidbody2D rigid;
    public RectTransform rectPos;

    private void Start()
    {
        //rigid.GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
        rigid.simulated = true;
        //rectPos = GetComponent<RectTransform>();
        rectPos.position = ballPos;
    }

    public void SendBallInDirection(Vector2 dir, float speed)
    {
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag(Tags.Square))
        {
            coll.gameObject.GetComponentInParent<Container>().RunBonus_02();
            Destroy(gameObject);
        }
    }

}
