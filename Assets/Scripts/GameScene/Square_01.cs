using UnityEngine;

public class Square_01 : MonoBehaviour
{
    public GameObject Square_01EFX;
    public GameObject Square_01Pr;

    public void DeathZone()
    {
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        Destroy(Square_01Pr);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }
}
