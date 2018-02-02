using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStars : MonoBehaviour
{
    public GameObject starON, starOFF,collectStarEFX;

    private void Awake()
    {
        starON.SetActive(false);
        starOFF.SetActive(true);
    }

    public void ShowStar()
    {
        starON.SetActive(true);
        starOFF.SetActive(false);

        GameObject go = Instantiate(collectStarEFX, starON.transform) as GameObject;
        Destroy(go, 1f);
    }
}
