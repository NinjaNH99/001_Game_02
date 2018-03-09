using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddScoreLevelUI : MonoBehaviour
{
    public void Show(int value)
    {
        GetComponent<TextMeshProUGUI>().text = '+' + value.ToString();
    }
}
