using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square_Line : MonoBehaviour
{
    public GameObject Line;
    public GameObject Square_Img;
    public GameObject Square_01EFX;

    public Image Img1, Img2, Img3;

    private GameController gameContr;
    private Animator square_LineAnim;
    private Vector2 shootDirL, shootDirR;
    private RectTransform transofrmPos;

    private bool rotate;

    private void Start()
    {
        LevelManager.Instance.listSquareLine.Add(this);
        gameContr = GameController.Instance;
        square_LineAnim = GetComponent<Animator>();
        Change();
        rotate = true;
        shootDirL = Vector2.left;
        shootDirR = Vector2.right;
        //RotateSquare();
    }

    public void Change()
    {
        var colorScore = gameContr.score_Rows;
        Img1.color = Img2.color = Img3.color = gameContr.ChangeColor(colorScore);
    }


    private void ShootL()
    {
        GameObject go = Instantiate(Line, transofrmPos) as GameObject;
        go.GetComponent<RectTransform>().position = transofrmPos.position;
        BallSQLine ballSQLine = go.GetComponent<BallSQLine>();
        //ballSQLine.ballPos = transofrmPos.position;
        ballSQLine.speed = 4f;
        ballSQLine.color = Img1.color;
        ballSQLine.SendBallInDirection(shootDirL);
        go.SetActive(true);
    }

    private void ShootR()
    {
        GameObject go = Instantiate(Line, transofrmPos) as GameObject;
        go.GetComponent<RectTransform>().position = transofrmPos.position;
        BallSQLine ballSQLine = go.GetComponent<BallSQLine>();
        //ballSQLine.ballPos = transofrmPos.position;
        ballSQLine.speed = 4f;
        ballSQLine.color = Img1.color;
        ballSQLine.SendBallInDirection(shootDirR);
        go.SetActive(true);
    }

    public void RotateSquare()
    {
        if (rotate)
        {
            square_LineAnim.SetTrigger("Rotate90");
            shootDirL = Vector2.up;
            shootDirR = Vector2.down;
            rotate = false;
        }
        else
        {
            square_LineAnim.SetTrigger("Rotate180");
            shootDirL = Vector2.left;
            shootDirR = Vector2.right;
            rotate = true;
        }
    }

    public void DeathZone()
    {
        //LevelManager.Instance.CheckTeleportsNull();
        LevelManager.Instance.listSquareLine.Remove(this);
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        GetComponentInParent<Row>().CheckNrConts();
        Destroy(Square_Img);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy)/* && shoot */)
        {
            transofrmPos = gameObject.GetComponent<RectTransform>();
            square_LineAnim.SetTrigger("Hit");
            ShootL();
            ShootR();
            //Shoot(false);
            //shoot = false;
        }
    }

}
