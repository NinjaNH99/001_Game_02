using UnityEngine;
using TMPro;

public class UpdateLoseMenu : MonoBehaviour {

    public GameObject score;
    public GameObject bonus;
    public GameObject bestScore;

    private GameController gameContr;

    private void Awake()
    {
        gameContr = GameController.Instance;
    }

    public void UpdateGameStatus()
    {
        score.GetComponent<TextMeshProUGUI>().text = gameContr.score_Rows.ToString();
        bonus.GetComponent<TextMeshProUGUI>().text = Bonus.Instance.Bonus_01.ToString();
    }
}
