using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Bonus_02 : MonoSingleton<Ball_Bonus_02>
{
    private Rigidbody2D rigid;

    private void Start()
    {
        rigid.GetComponent<Rigidbody2D>();
    }

    public void SendBallInDirection(Vector2 dir, float speed)
    {
        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag(Tags.Square))
        {

        }
    }

}
