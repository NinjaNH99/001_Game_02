using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square_Line : MonoBehaviour
{
    public GameObject LineDir;
    public GameObject Line;

    private GameController gameContr;
    private Animator lineDirAnim;
    private Image img;

    private bool shoot;

    private void Start()
    {
        gameContr = GameController.Instance;
        lineDirAnim = LineDir.GetComponent<Animator>();
        img = GetComponent<Image>();
        Change();
        shoot = true;
    }

    public void Change()
    {
        var colorScore = gameContr.score_Rows;
        img.color = gameContr.ChangeColor(colorScore);
    }


    private void Shoot()
    {
        RectTransform transofrmPos = gameObject.GetComponent<RectTransform>();

        GameObject go = Instantiate(Line, transofrmPos) as GameObject;
        go.GetComponent<RectTransform>().position = transofrmPos.position;
        BallSQLine ballSQLine = go.GetComponent<BallSQLine>();
        //ballSQLine.ballPos = transofrmPos.position;
        ballSQLine.speed = 3.5f;
        go.SetActive(true);
        ballSQLine.SendBallInDirection(Vector2.left);

        GameObject go1 = Instantiate(Line, transofrmPos) as GameObject;
        go1.GetComponent<RectTransform>().position = transofrmPos.position;
        BallSQLine ballSQLine1 = go1.GetComponent<BallSQLine>();
       // ballSQLine1.ballPos = transofrmPos.position;
        ballSQLine1.speed = 3.5f;
        go1.SetActive(true);
        ballSQLine1.SendBallInDirection(Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy)/* && shoot */)
        {
            Shoot();
            //Shoot(false);
            //shoot = false;
        }
    }

}
