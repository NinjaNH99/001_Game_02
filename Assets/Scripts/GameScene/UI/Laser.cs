using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRend;
    private RectTransform rectTransf;
    private RaycastHit2D hit;
    private Color laserCol;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        rectTransf = GetComponent<RectTransform>();
        lineRend.SetPosition(0, rectTransf.position);
        lineRend.SetPosition(1, rectTransf.position);
        laserCol = GameController.Instance.ChangeColor(GameController.Instance.score_Rows);
        lineRend.startColor = laserCol;
        lineRend.endColor = laserCol;
    }

    private void OnEnable()
    {        Awake();    }

    private void OnDisable()
    {        Awake();    }

    private void Update()
    {
        hit = Physics2D.Raycast(rectTransf.position, rectTransf.up, 550, ~(1 << LayerMask.NameToLayer("DeathZone") | 1 << LayerMask.NameToLayer("ObjectIgR")));
        lineRend.SetPosition(0, rectTransf.position);
        lineRend.SetPosition(1, hit.point);
    }
}
