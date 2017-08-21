using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLevel : MonoBehaviour
{
    private bool isLose;

    private void Awake()
    {
        isLose = false;
    } 

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Square))
            OnLoadScene();
        else if (!coll.gameObject.CompareTag(Tags.ballCopy) || !coll.gameObject.CompareTag(Tags.Player))
            coll.gameObject.SendMessage("DeathLevel");
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
