using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square_01Teleport : MonoBehaviour
{
    //public Image imgSign;
    public Material imgSign;

    private GameController gameContr;
    private GameObject parentOBJ;

    private void Start()
    {
        gameContr = GameController.Instance;
        ChangeColor();
        parentOBJ = GetComponentInParent<Square_01>().gameObject;
    }
    
    public void ChangeColor()
    {
        var colorScore = GameData.score_Rows;
        imgSign.color = gameContr.ChangeColor(colorScore);
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
        {
            if (coll.gameObject.GetComponent<Ball>().enterTeleport)
                LevelManager.Instance.Teleports(parentOBJ, coll.gameObject);
        }
    }
}
