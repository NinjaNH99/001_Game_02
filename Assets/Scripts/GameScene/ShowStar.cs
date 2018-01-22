using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStar : MonoBehaviour
{
    public GameObject starON, starOFF, starOFFCircle, collectEFX;

    private Image starOFFImg, starONImg, starOFFCircleImg;
    private bool changeColorStarON;

    private void Awake()
    {
        changeColorStarON = true;
        starON.SetActive(false);
        starOFF.SetActive(true);
        starOFFImg = starOFF.GetComponent<Image>();
        starONImg = starON.GetComponent<Image>();
        starOFFCircleImg = starOFFCircle.GetComponent<Image>();
    }

    public void Staroption(Color color)
    {
        if (changeColorStarON)
        {
            starON.SetActive(true);
            starOFF.SetActive(false);

            starONImg.color = color;
            starONImg.SetTransparency(0.85f);

            GameObject go = Instantiate(collectEFX, gameObject.transform) as GameObject;

            var main = go.GetComponent<ParticleSystem>().main;
            main.startColor = starONImg.color;

            Destroy(go, 1f);

            changeColorStarON = false;
        }
    }

    public void ChangeColor(int colorScore, Color color)
    {
        starOFFImg.color = color;
        starOFFImg.SetTransparency(0.7f);

        starOFFCircleImg.color = starOFFImg.color;
        starOFFCircleImg.SetTransparency(0.35f);
    }
	
}
