using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square_Line : MonoBehaviour
{
    public GameObject Line;
    public GameObject Square_Img;
    public GameObject Square_01EFX;
    public GameObject Laser;

    public Image Img1, Img2, Img3;

    private GameController gameContr;
    private Animator square_LineAnim;
    private Vector2 shootDirL, shootDirR;
    private RectTransform transofrmPos;

    private bool rotate;

    private void Awake()
    {
        square_LineAnim = GetComponent<Animator>();
        rotate = true;
        shootDirL = Vector2.left;
        shootDirR = Vector2.right;
    }

    private void Start()
    {
        gameContr = GameController.Instance;
        EventManager.EvMethods += RotateSquare;
        Change();
    }

    public void Change()
    {
        var colorScore = gameContr.score_Rows;
        Img1.color = Img2.color = Img3.color = gameContr.ChangeColor(colorScore);
    }


    private void ShootL()
    {
        GameObject line = Instantiate(Laser, transofrmPos) as GameObject;
        line.GetComponent<RectTransform>().position = Img1.GetComponent<RectTransform>().position;
        line.GetComponent<LaserSq>().dir = shootDirL;
        line.SetActive(true);
    }
    
    private void ShootR()
    {
        GameObject line = Instantiate(Laser, transofrmPos) as GameObject;
        line.GetComponent<RectTransform>().position = Img2.GetComponent<RectTransform>().position;
        line.GetComponent<LaserSq>().dir = shootDirR;
        line.SetActive(true);        
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
        //LevelManager.Instance.listSquareLine.Remove(this);
        EventManager.EvMethods -= RotateSquare;
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        Destroy(Square_Img);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
        {
            transofrmPos = gameObject.GetComponent<RectTransform>();
            //square_LineAnim.SetTrigger("Hit");
            ShootL();
            ShootR();
        }
    }

}
