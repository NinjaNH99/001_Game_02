using UnityEngine;

public class Square_01 : Row
{
    public GameObject Square_01EFX;
    public GameObject Square_01Pr;

    private void Start()
    {
        LevelManager.Instance.listTelep.Add(this.gameObject);
    }

    public void DeathZone()
    {
        //LevelManager.Instance.CheckTeleportsNull();
        LevelManager.Instance.listTelep.Remove(this.gameObject);
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        GetComponentInParent<Row>().CheckNrConts(false);
        Destroy(Square_01Pr);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

}
