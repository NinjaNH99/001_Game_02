using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private GameController gameContr;

    private void Awake()
    {
        gameContr = GameController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Square) || coll.gameObject.CompareTag(Tags.Block_Boss))
        {
            coll.transform.parent.GetComponent<Container>().EndLevel(coll.gameObject);
            gameContr.isBreakingStuff = true;
        }
        else if (!coll.gameObject.CompareTag(Tags.Row) && !coll.gameObject.CompareTag(Tags.Square_Liser) && !coll.gameObject.CompareTag(Tags.Square_Teleport) && !coll.gameObject.CompareTag(Tags.BallBomb))
        {
            coll.gameObject.SendMessage("DeathZone");
            //Debug.Log("coll.gameObject.tag : " + coll.gameObject.tag);
        }
    }
}
