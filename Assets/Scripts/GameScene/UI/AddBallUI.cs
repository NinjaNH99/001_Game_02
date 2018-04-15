using UnityEngine;
using TMPro;

public class AddBallUI : MonoBehaviour {

    private GameController gameContr;

    private void Start()
    {
        gameContr = GameController.Instance;
        GetComponent<TextMeshProUGUI>().text = '+' + gameContr.AddBallUI.ToString();
    }

}
