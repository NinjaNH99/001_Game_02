using UnityEngine;
using TMPro;

public class Bonus : MonoSingleton<Bonus>
{
    public static int bonus_01;
    public GameObject bonus_01UI;
    public int AddBallUI;

    // Bonus 02
    public static int bonus_02;
    public GameObject bonus_02_text;
    public GameObject bonus_02UI;
    public bool isReadyForLaunch;

    private bool firstBonus_02;

    private void Awake()
    {
        bonus_01 = 0;
        bonus_02 = 0;
        AddBallUI = 0;
        firstBonus_02 = true;
        isReadyForLaunch = false;
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
        bonus_02--;
        if (bonus_02 < 1)
        {
            bonus_02 = 0;
            firstBonus_02 = true;
            bonus_02UI.GetComponent<Animator>().SetTrigger("RmBonus_02");
        }
        isReadyForLaunch = true;
        UpdateUIText();
    }
}
