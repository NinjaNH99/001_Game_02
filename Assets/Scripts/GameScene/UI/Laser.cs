using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //public Transform laserHit;

    private LineRenderer lineRend;
    private RectTransform rectTransf;
    private RaycastHit2D hit;

    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        rectTransf = GetComponent<RectTransform>();
        hit = Physics2D.Raycast(rectTransf.position, rectTransf.up, 550, ~(1 << LayerMask.NameToLayer("DeathZone")));
    }

    private void OnEnable()
    {
        Start();
    }

    private void Update()
    {
        hit = Physics2D.Raycast(rectTransf.position, rectTransf.up, 550, ~(1 << LayerMask.NameToLayer("DeathZone")));
        lineRend.SetPosition(0, rectTransf.position);
        lineRend.SetPosition(1, hit.point);
    }
}
