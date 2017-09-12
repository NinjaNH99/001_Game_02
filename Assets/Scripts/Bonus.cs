using UnityEngine;
using TMPro;

public class Bonus : MonoSingleton<Bonus>
{
    public static int bonus_01;
    public GameObject bonus_01UI;
    public int AddBallUI;

    public static int bonus_02;
    public GameObject bonus_02Obj;
    public GameObject bonus_02_text;

    private bool firstBonus_02;

    private void Awake()
    {
        bonus_01 = 0;
        bonus_02 = 0;
        AddBallUI = 0;
        firstBonus_02 = true;
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
            bonus_02Obj.GetComponent<Animator>().SetTrigger("IsBonus_02");
            firstBonus_02 = false;
        }
        UpdateUIText();
    }

    public void Remove_02()
    {
        bonus_02Obj.GetComponent<Animator>().SetTrigger("RmBonus_02");
        bonus_02--;
        if (bonus_02 < 0)
        {
            bonus_02 = 0;
            firstBonus_02 = true;
        }
        UpdateUIText();
    }

}
