using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLevel : MonoBehaviour
{
    private bool isLoadScene;

    private void Awake()
    {
        isLoadScene = false;
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
        if(!isLoadScene)
        {
            SceneManager.LoadScene("Game");
            isLoadScene = true;
        }
    }
}
