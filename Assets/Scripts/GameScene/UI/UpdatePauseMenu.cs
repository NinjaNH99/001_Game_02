using UnityEngine;
using TMPro;

public class UpdatePauseMenu : MonoBehaviour {

	public GameObject score;
	public GameObject bonus;
	public GameObject bestScore;

    public void UpdateGameStatus()
    { 
        score.GetComponent<TextMeshProUGUI>().text = GameData.score_Rows.ToString();
        bonus.GetComponent<TextMeshProUGUI>().text = Bonus.Instance.Bonus_01.ToString();
        bestScore.GetComponent<TextMeshProUGUI>().text = GameData.maxScore.ToString();
    }
	
}
