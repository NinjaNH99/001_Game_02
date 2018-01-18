using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square_01Teleport : MonoBehaviour
{
    public Image imgSign;

    private void Start()
    {
        ChangeColor();
    }

    public void Teleport(GameObject obj, float dir, float speed)
    {
        var i = Random.Range(0, LevelManager.Instance.listTelep.Count);
        obj.GetComponent<RectTransform>().position = LevelManager.Instance.listTelep[i].gameObject.GetComponent<RectTransform>().position;
    }

    public void ChangeColor()
    {
        var colorScore = GameController.score_Rows;
        imgSign.color = GameController.Instance.ChangeColor(colorScore);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
        {
            Teleport(coll.gameObject, coll.GetComponent<Ball>().rigid.velocity.magnitude, coll.GetComponent<Ball>().speed);
        }
    }
}
