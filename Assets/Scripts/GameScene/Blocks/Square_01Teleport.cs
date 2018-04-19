using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square_01Teleport : MonoBehaviour
{
    //public Image imgSign;
    public Material imgSign;

    //private GameController gameContr;
    //public GameObject parentOBJ;

    private void Start()
    {
        //gameContr = GameController.Instance;
        ChangeColor();
        //parentOBJ = GetComponentInParent<Square_01>().gameObject;
    }
    
    public void ChangeColor()
    {
        var colorScore = GameData.score_Rows;
        imgSign.color = GameController.Instance.ChangeColor(colorScore);
    }

}
