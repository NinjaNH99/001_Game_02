using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tags
{
    public const string Floor = "Floor";
    public const string Square = "Square";
    public const string Square_01 = "Square_01";
    public const string Bonus = "Bonus";
    public const string Wall = "Wall";
    public const string WallR = "WallR";
    public const string Player = "Player";
    public const string ballCopy = "ballCopy";
    public const string EndLevel = "EndLevel";
    public const string Background = "Background";
    public const string Space2D = "2DSpace";
    public const string Bonus_02 = "Bonus_02";
}

public class GameController : MonoSingleton<GameController>
{
    private const float DEADZONE = 60.0f;
    private const float MAXIMUM_PULL = 200.0f;
    private const float TIMEWAITBOOSTSPEED = 2f;
    public const float BALLSPEED = 4f;

    public Transform Canvas1, ballsPreview, Space2D;
    public RectTransform ballContainer;
    public GameObject tutorialContainer, ballPr, scoreText, amountBallsTextPr;
    public Button BoostSpeedButton;

    // Canvas 2
    public GameObject canvas2, loseMenu, statusBar, bonus_02Pr;

    public Color ballColor;
    public Color ballCopyColor;
    public Color[] blockColor;


    [HideInInspector]
    public static int score, amountBallsLeft, amountBalls;
    public static float ballOrgYPos;

    [HideInInspector]
    public int AddBallUI;

    [HideInInspector]
    public static bool onBoostSpeed, isBreakingStuff, updateInputs, startTimerGravity, allBallLanded, bonus_02IsReady, firstBallLanded;
    public Vector2 targetPosLanded;

    private Vector2 sd;
    private float timeWaitBoostSpeed;
    private TextMeshProUGUI amountBallsText;

    private void Awake()
    {
        Time.timeScale = score = 1;
        amountBalls = 2;
        sd = MobileInputs.Instance.swipeDelta;
        sd.Set(-sd.x, -sd.y);
        isBreakingStuff = allBallLanded = firstBallLanded = false;
        updateInputs = true;
        AddBallUI = 0;
    }

    private void Start()
    {
        ballColor = Ball.Instance.GetComponent<Image>().color;
        amountBallsText = amountBallsTextPr.GetComponent<TextMeshProUGUI>();
        ballCopyColor = ballColor;
        ballCopyColor.a = 0.8f;
        ballsPreview.parent.gameObject.SetActive(false);
        timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;
        UpdateUIText();
        ShowAmBallsText(amountBalls);
        amountBallsLeft = amountBalls;
        ballOrgYPos = Ball.Instance.transform.position.y;
        startTimerGravity = bonus_02IsReady = onBoostSpeed = BoostSpeedButton.interactable = false;
    }

    private void Update()
    {
        if(Time.timeScale != 0)
        {
            if (!isBreakingStuff)
                PoolInput();
            if (allBallLanded)
            {
                score++;
                onBoostSpeed = false;
                timeWaitBoostSpeed = TIMEWAITBOOSTSPEED + (amountBalls / 10f);
                BoostSpeedButtonAnim(true);
                LevelContainer.Instance.GenerateNewRow();
                allBallLanded = false;
                UpdateUIText();
                ShowAmBallsText(amountBalls);
                TimerGravity.Instance.nrBalls = amountBalls / 10f;
            }
            if (onBoostSpeed)
            {
                timeWaitBoostSpeed -= Time.deltaTime;
                if (timeWaitBoostSpeed < 0)
                {
                    BoostSpeedButtonAnim(false);
                    timeWaitBoostSpeed = TIMEWAITBOOSTSPEED + (amountBalls / 5f);
                    onBoostSpeed = false;
                }
            }
        }
    }

    private void PoolInput()
    {
        sd = MobileInputs.Instance.swipeDelta;
        sd.Set(-sd.x, -sd.y);
        if (sd != Vector2.zero)
        {
            if (sd.y < 3.5f)
            {
                ballsPreview.parent.gameObject.SetActive(false);
            }
            else
            {
                ballsPreview.parent.up = sd.normalized;
                ballsPreview.parent.gameObject.SetActive(true);
                ballsPreview.localScale = Vector2.Lerp(new Vector2(0.7f, 0.7f), new Vector2(1, 1), sd.magnitude / MAXIMUM_PULL);
                if (MobileInputs.Instance.release)
                {
                    tutorialContainer.SetActive(false);
                    isBreakingStuff = onBoostSpeed = true;
                    updateInputs = false;
                    MobileInputs.Instance.Reset();
                    if(bonus_02IsReady)
                    {
                        StartCoroutine(GenerateNewBall(amountBalls, false, sd.normalized));
                    }
                    ballsPreview.parent.gameObject.SetActive(false);
                    StartCoroutine(GenerateNewBall(amountBalls, true, sd.normalized));
                    startTimerGravity = true;
                    Bonus.Instance.ActivateButton(false);
                }
            }
        }
    }

    private IEnumerator GenerateNewBall(int nrBall, bool ballCopySP,  Vector2 sd)
    {
        if (nrBall == 1)
        {
            yield return null;
        }
        else if (ballCopySP)
        {
            Vector2 posIn = Ball.Instance.GetComponent<RectTransform>().position;
            int AmountBalls = nrBall;

            for (int i = 0; i < nrBall - 1; i++)
            {
                yield return new WaitForSeconds(0.1f);

                GameObject go = Instantiate(ballPr, ballContainer) as GameObject;
                BallCopy ballCopy = go.GetComponent<BallCopy>();
                ballCopy.ballPos = posIn;
                ballCopy.speed = BALLSPEED;
                ballCopy.SendBallInDirection(sd);
                AmountBalls--;
                ShowAmBallsText(AmountBalls);
            }
            yield return new WaitForSeconds(0.1f);
            Ball.Instance.speed = BALLSPEED;
            Ball.Instance.SendBallInDirection(sd);
        }
        else if (!ballCopySP)
        {
            Vector2 posIn = Ball.Instance.GetComponent<RectTransform>().position;

            GameObject goBonus_02 = Instantiate(bonus_02Pr, Space2D) as GameObject;
            Ball_Bonus_02 ball_02 = goBonus_02.GetComponent<Ball_Bonus_02>();
            ball_02.ballPos = posIn;
            ball_02.SendBallInDirection(sd, BALLSPEED);
            bonus_02IsReady = false;
        }
    }

    public void IsAllBallLanded()
    {
        amountBallsLeft--;
        if (amountBallsLeft <= 0)
        {
            startTimerGravity = false;
            if (TimerGravity.Instance.i == 1)
                AllBallLanded();
        }
    }

    private void AllBallLanded()
    {
        Time.timeScale = 1f;
        isBreakingStuff = firstBallLanded = false;
        allBallLanded = updateInputs = true;
        amountBallsLeft = amountBalls;
        AddBallUI = 0;
        Bonus.Instance.ActivateButton(true);
    }

    private void ShowAmBallsText(int amountBallsShow)
    {
        amountBallsText.text = 'x' + amountBallsShow.ToString();
    }

    public void UpdateUIText()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        Bonus.Instance.UpdateUIText();
    }

    public void OnLoseMenu()
    {
        canvas2.SetActive(true);
        loseMenu.SetActive(true);
        statusBar.SetActive(false);
        score--;
        loseMenu.GetComponent<UpdateLoseMenu>().UpdateGameStatus();
        Time.timeScale = 0f;
    }

    public Color ChangeColor(int hp)
    {
        int colorID = 0;
        if (hp / 5 > blockColor.Length)
            colorID = blockColor.Length;
        else
            colorID = hp / 5;
        return blockColor[colorID];                  
    }

    private void BoostSpeedButtonAnim(bool exit)
    {
        if (!exit)
        {
            BoostSpeedButton.GetComponent<Animator>().SetTrigger("BoostSpeed_Intro");
            BoostSpeedButton.interactable = true;
        }
        else if(BoostSpeedButton.interactable)
        {
            BoostSpeedButton.GetComponent<Animator>().SetTrigger("BoostSpeed_Exit");
            BoostSpeedButton.interactable = false;
        }
    }

}
