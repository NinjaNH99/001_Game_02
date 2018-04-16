using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bonus : MonoSingleton<Bonus>
{
    public GameObject bonus_01Text;
    public RectTransform bonus_01Icon;

    // Bonus 02 
    public GameObject bonus_02_text;
    public GameObject bonus_02UI;
    public RectTransform bonus_02Icon;

    public Button bonus_02Button;
    public GameObject EFX;

    [HideInInspector]
    public bool isReadyForLaunch;
    [HideInInspector]
    public static int AddBallUI;


    private GameController gameContr;
    private int bonus_01, ballBomb;

    public int Bonus_01 { get { return bonus_01; } set { } }
    public int BallBomb { get { return ballBomb; } set { } }

    private bool firstBonus_02;
    private bool ballIsReady;

    private void Awake()
    {
        //gameContr = GameController.Instance;
        AddBallUI = 0;
        firstBonus_02 = ballIsReady = true;
        isReadyForLaunch = false;
        //ActivateButton(false);
    }

    private void Start()
    {
        gameContr = GameController.Instance;
        bonus_01 = GameData.maxBonus_01;
        ballBomb = GameData.ballBomb;
        if(ballBomb > 0)
            bonus_02UI.GetComponent<Animator>().SetTrigger("IsBonus_02");
    }

    public void UpdateUIText()
    {
        bonus_01Text.GetComponent<TextMeshProUGUI>().text = Bonus_01.ToString();
        bonus_02_text.GetComponent<TextMeshProUGUI>().text = 'x' + BallBomb.ToString();
    }

    public void AddBonus_01()
    {
        bonus_01++;

        GameObject go = Instantiate(EFX, bonus_01Icon) as GameObject;
        var main = go.GetComponent<ParticleSystem>().main;
        main.startColor = bonus_01Icon.GetComponent<Image>().color;
        Destroy(go, 1f);

        gameContr.IncreaseMaxScore();
    }

    public void AddBonus_02()
    {
        ballBomb++;
        GameData.ballBomb++;
        if (firstBonus_02)
        {
            bonus_02UI.GetComponent<Animator>().SetTrigger("IsBonus_02");
            firstBonus_02 = false;
        }

        GameObject go = Instantiate(EFX, bonus_02Icon) as GameObject;
        var main = go.GetComponent<ParticleSystem>().main;
        main.startColor = bonus_02Icon.GetComponent<Image>().color;
        Destroy(go, 1f);

        gameContr.IncreaseMaxScore();
    }

    public void Remove_02()
    {
        if (ballBomb > 0 && ballIsReady)
        {
            ballBomb--;
            GameData.ballBomb--;
            if (ballBomb < 1)
            {
                ballBomb = 0;
                firstBonus_02 = true;
                bonus_02UI.GetComponent<Animator>().SetTrigger("RmBonus_02");
            }
            BallInit.Instance.createBallBomb = true;
            BallInit.Instance.GenerateBalls();
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
