using UnityEngine;
using TMPro;

public class AddBallUI : MonoBehaviour {

    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = '+' + GameController.Instance.AddBallUI.ToString();
    }

}
