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
        else if (!coll.gameObject.CompareTag(Tags.Square_01) && !coll.gameObject.CompareTag(Tags.Row))
            coll.gameObject.SendMessage("DeathZone");
        else if (coll.gameObject.CompareTag(Tags.Square_01))
            coll.transform.parent.GetComponent<Square_01>().Despawn();
        else if (coll.gameObject.CompareTag(Tags.Square_Line))
            coll.transform.parent.GetComponent<Square_Line>().DeathZone();
    }
}
