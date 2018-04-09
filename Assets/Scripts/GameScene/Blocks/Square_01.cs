using UnityEngine;

public class Square_01 : MonoBehaviour
{
    public GameObject Square_01EFX;
    public GameObject Square_01Pr;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        levelManager.listTelep.Add(this.gameObject);
        EventManager.evSpawnRand += Despawn;
    }

    public void Despawn()
    {
        EventManager.evSpawnRand -= Despawn;
        levelManager.listTelep.Remove(this.gameObject);
        DeathZone();
    }

    public void DeathZone()
    {
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        //GetComponentInParent<Row>().nrSpace++;
        Destroy(Square_01Pr);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

}
