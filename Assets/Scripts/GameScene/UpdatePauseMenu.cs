using UnityEngine;
using TMPro;

public class UpdatePauseMenu : MonoBehaviour {

	public GameObject score;
	public GameObject bonus;
	public GameObject bestScore;

	public void UpdateGameStatus()
    { 
        score.GetComponent<TextMeshProUGUI>().text = GameController.score.ToString();
        bonus.GetComponent<TextMeshProUGUI>().text = Bonus.bonus_01.ToString();
    }
	
}
