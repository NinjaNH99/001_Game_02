using UnityEngine;

public enum TeleportType
{
    In, Out
}

public class Square_01 : MonoBehaviour
{
    public TeleportType type;

    public GameObject Square_01EFX;
    public GameObject Square_01Pr;

    [HideInInspector]
    public RectTransform rectTransform;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        rectTransform = this.GetComponent<RectTransform>();
        if (type == TeleportType.In)
            levelManager.teleportIn = rectTransform;
        else if (type == TeleportType.Out)
            levelManager.teleportOut = rectTransform;

        EventManager.EvDeSpawnM += Despawn;
    }

    public void Despawn()
    {
        EventManager.EvDeSpawnM -= Despawn;
        //levelManager.listTelep.Remove(this.GetComponent<RectTransform>());
        DeathZone();
    }

    public void DeathZone()
    {
        //Debug.Log("TeleportDied.RowID[" + GetComponentInParent<Row>().rowID + "]");
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        //GetComponentInParent<Row>().nrSpace++;
        Destroy(Square_01Pr);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

}
