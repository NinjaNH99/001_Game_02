using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSq : MonoBehaviour
{
    public Vector2 dir;

	private LineRenderer lineRend;
    private RectTransform rectTransf;
    private RaycastHit2D hit;
    private bool check;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        rectTransf = GetComponent<RectTransform>();
        check = true;
    }

    private void Update()
    {
        hit = Physics2D.Raycast(rectTransf.position, dir, 1000, 1 << LayerMask.NameToLayer("Collision") | 1 << LayerMask.NameToLayer("Bloks"));
        lineRend.SetPosition(0, rectTransf.position);
        lineRend.SetPosition(1, hit.point);
        if (hit.collider.gameObject.CompareTag(Tags.Square) && check)
        {
            Debug.Log("Hit");
            hit.transform.SendMessage("ReceiveHit");
            check = false;
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
