using UnityEngine;
using TMPro;

public class UpdateLoseMenu : MonoBehaviour {

    public GameObject score;
    public GameObject bonus;
    public GameObject bestScore;

    public void UpdateGameStatus()
    {
        score.GetComponent<TextMeshProUGUI>().text = GameController.score.ToString();
        bonus.GetComponent<TextMeshProUGUI>().text = GameController.bonus_01.ToString();
    }
}
