using UnityEngine;

public class Square_01 : MonoBehaviour
{
    public GameObject Square_01EFX;
    public GameObject Square_01Pr;

    public void DeathZone()
    {
        //LevelManager.Instance.CheckTeleportsNull();
        LevelManager.Instance.listTelep.Remove(GetComponentInChildren<Square_01Teleport>().gameObject);
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        Destroy(Square_01Pr);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

}
