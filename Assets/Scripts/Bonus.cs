using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bonus : MonoSingleton<Bonus>
{
    public GameObject bonus_01UI;

    // Bonus 02 
    public GameObject bonus_02_text;
    public GameObject bonus_02UI;
    public Button bonus_02Button;

    [HideInInspector]
    public bool isReadyForLaunch;
    [HideInInspector]
    public static int AddBallUI, bonus_01, bonus_02;

    private bool firstBonus_02;
    private bool ballIsReady;

    private void Awake()
    {
        bonus_01 = 0;
        bonus_02 = 5;
        AddBallUI = 0;
        firstBonus_02 = ballIsReady = true;
        isReadyForLaunch = false;
        //ActivateButton(false);
    }

    public void UpdateUIText()
    {
        bonus_01UI.GetComponent<TextMeshProUGUI>().text = bonus_01.ToString();
        bonus_02_text.GetComponent<TextMeshProUGUI>().text = 'x' + bonus_02.ToString();
    }

    public void AddBonus_02()
    {
        if (firstBonus_02)
        {
            bonus_02UI.GetComponent<Animator>().SetTrigger("IsBonus_02");
            firstBonus_02 = false;
        }
        UpdateUIText();
    }

    public void Remove_02()
    {
        if (bonus_02 > 0 && ballIsReady)
        {
            bonus_02--;
            if (bonus_02 < 1)
            {
                bonus_02 = 0;
                firstBonus_02 = true;
                bonus_02UI.GetComponent<Animator>().SetTrigger("RmBonus_02");
            }
            GameController.bonus_02IsReady = true;
            UpdateUIText();
            ballIsReady = false;
        }
    }

    public void ActivateButton(bool activ)
    {
        if (activ)
        {
            bonus_02Button.interactable = true;
            ballIsReady = true;
        }
        else
            bonus_02Button.interactable = false;
    }
}
