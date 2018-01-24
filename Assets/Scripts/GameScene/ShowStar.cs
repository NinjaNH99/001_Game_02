using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStar : MonoBehaviour
{
    public GameObject starON, starOFFCircle, collectEFX;

    private Image starONImg, starOFFCircleImg;
    private bool changeColorStarON;

    private void Awake()
    {
        changeColorStarON = true;
        starON.SetActive(false);
        starONImg = starON.GetComponent<Image>();
        starOFFCircleImg = starOFFCircle.GetComponent<Image>();
    }

    public void Staroption(Color color)
    {
        if (changeColorStarON)
        {
            starON.SetActive(true);

            GameObject go = Instantiate(collectEFX, gameObject.transform) as GameObject;

            var main = go.GetComponent<ParticleSystem>().main;
            main.startColor = starONImg.color;

            Destroy(go, 1f);

            changeColorStarON = false;
        }
    }

    public void ChangeColor(int colorScore, Color color)
    {
        starONImg.color = color;
        starONImg.SetTransparency(0.85f);

        starOFFCircleImg.color = color;
        starOFFCircleImg.SetTransparency(0.2f);
    }
	
}
