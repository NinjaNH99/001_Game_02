using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSq : MonoBehaviour
{
    public Vector2 dir;

	private LineRenderer lineRend;
    private RectTransform rectTransf;
    private RaycastHit2D hit;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        rectTransf = GetComponent<RectTransform>();
    }

    private void Start()
    {
        hit = Physics2D.Raycast(rectTransf.position, dir, 550, ~(1 << LayerMask.NameToLayer("DeathZone")));
        lineRend.SetPosition(0, rectTransf.position);
        lineRend.SetPosition(1, hit.point);
    }

    private void OnEnable()
    {
        Start();
    }

    /*
    private void Update()
    {
        hit = Physics2D.Raycast(rectTransf.position, rectTransf.up, 550, ~(1 << LayerMask.NameToLayer("DeathZone")));
        lineRend.SetPosition(0, rectTransf.position);
        lineRend.SetPosition(1, hit.point);
    }*/
}
