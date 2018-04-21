using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Square) || coll.gameObject.CompareTag(Tags.Block_Boss))
        {
            coll.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GameController.Instance.isBreakingStuff = true;
        }
        else if (!coll.gameObject.CompareTag(Tags.Row) && !coll.gameObject.CompareTag(Tags.Square_Liser) && !coll.gameObject.CompareTag(Tags.Square_Teleport) && !coll.gameObject.CompareTag(Tags.BallBomb))
        {
            coll.gameObject.SendMessage("DeathZone");
        }
    }
}
