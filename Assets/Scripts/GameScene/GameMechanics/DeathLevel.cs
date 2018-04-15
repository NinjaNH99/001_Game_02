using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLevel : MonoBehaviour
{
    private bool isLose = false;

    private void Awake()
    {
        isLose = false;
    } 

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Square) || coll.gameObject.CompareTag(Tags.Block_Boss))
            OnLoadScene();
        else if (!coll.gameObject.CompareTag(Tags.ballCopy) || !coll.gameObject.CompareTag(Tags.Player) || !coll.gameObject.CompareTag(Tags.Square_Liser) || !coll.gameObject.CompareTag(Tags.Square_Teleport) || !coll.gameObject.CompareTag(Tags.Row))
            coll.gameObject.SendMessage("DeathLevel");
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Row))
            coll.gameObject.SendMessage("DeSpawn");
    }

    private void OnLoadScene()
    {
        if(!isLose)
        {
            GameController.Instance.OnLoseMenu();
            isLose = true;
        }
    }
}
