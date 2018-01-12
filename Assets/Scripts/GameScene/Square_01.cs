using UnityEngine;

public class Square_01 : Row
{
    public GameObject Square_01EFX;
    public GameObject Square_01Pr;

    public void DeathZone()
    {
        GameObject goEFX = Instantiate(Square_01EFX, gameObject.transform) as GameObject;
        GetComponentInParent<Row>().CheckNrConts(false);
        Destroy(Square_01Pr);
        Destroy(goEFX, 1f);
        Destroy(gameObject, 1f);
    }

}
