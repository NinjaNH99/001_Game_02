using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Square))
            coll.transform.parent.GetComponent<Block>().EndLevel(coll.gameObject);
        else if (!coll.gameObject.CompareTag(Tags.ballCopy) && !coll.gameObject.CompareTag(Tags.Player) && !coll.gameObject.CompareTag(Tags.Square_01))
            coll.gameObject.SendMessage("DeathZone");
        else if (coll.gameObject.CompareTag(Tags.Square_01))
            coll.transform.parent.GetComponent<Square_01>().DeathZone();
    }
}
