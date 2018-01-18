using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStar : MonoBehaviour
{
    public GameObject starON, starOFF;

    private void Awake()
    {
        starON.SetActive(false);
        starOFF.SetActive(true);
    }

    public void Staroption()
    {
        starON.SetActive(true);
        starOFF.SetActive(false);
    }
	
}
