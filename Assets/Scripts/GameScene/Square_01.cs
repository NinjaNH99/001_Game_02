using UnityEngine;

public class Square_01 : Row
{
    public GameObject Square_01EFX;
    public GameObject Square_01Pr;

    public void Teleport(GameObject obj ,float dir, float speed)
    {
        obj.GetComponent<RectTransform>().position = this.gameObject.transform.position;
    }

    public void DeathZone()
    {
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        GetComponentInParent<Row>().CheckNrConts(false);
        Destroy(Square_01Pr);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
        {
            Teleport(coll.gameObject, coll.GetComponent<Ball>().rigid.velocity.magnitude, coll.GetComponent<Ball>().speed);
        }
    }

}
