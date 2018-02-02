using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Tags
{
    public const string Floor = "Floor";
    public const string Square = "Square";
    public const string Square_01 = "Square_01";
    public const string Bonus = "Bonus";
    public const string Wall = "Wall";
    public const string WallR = "WallR";
    public const string WallT = "WallT";
    public const string Player = "Player";
    public const string ballCopy = "ballCopy";
    public const string ballSQLine = "ballSQLine";
    public const string EndLevel = "EndLevel";
    public const string Background = "Background";
    public const string Space2D = "2DSpace";
    public const string Bonus_02 = "Bonus_02";
}

//  Fisher-Yates algorithm 
static class RandomExtensions
{
    public static void Shuffle<T>(this System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}

static class SetTransparencyExtensions
{
    public static void SetTransparency(this Image p_image, float p_transparency)
    {
        if (p_image != null)
        {
            Color __alpha = p_image.color;
            __alpha.a = p_transparency;
            p_image.color = __alpha;
        }
    }
}

public class GameController : MonoSingleton<GameController>
{
    private const float DEADZONE = 60.0f;
    private const float MAXIMUM_PULL = 200.0f;
    private const float TIMEWAITBOOSTSPEED = 2f; // 1.5
    public const float BALLSPEED = 4f;

    public Transform Canvas1, ballsPreview, Space2D;
    public RectTransform ballContainer;
    public GameObject tutorialContainer, ballCopyPr, ballOr , scoreText, amountBallsTextPr;
    public Button BoostSpeedButton;

    // Canvas 2
    public GameObject canvas2, loseMenu, statusBar, bonus_02Pr;

    public Color ballColor;
    public Color ballCopyColor;
    public Color[] blockColor;


    [HideInInspector]
    public float ballOrgYPos;

    [HideInInspector]
    public bool isBreakingStuff, updateInputs, allBallLanded, bonus_02IsReady, firstBallLanded;
    public Vector2 targetBallPosLanded;

    public int amountBalls;

    private Vector2 sd;
    private float timeWaitBoostSpeed;

    // For UI Show
    public bool onBoostSpeed;
    [HideInInspector]
    public int score_Rows, scoreLevel, amountBallsLeft, amountCollectBallsLeft, AddBallUI;
    private int amountBallsBack;
    private bool showsABExit;
    private TextMeshProUGUI amountBallsText;

    //For the new method of spawning the ball
    private List<GameObject> BallsList;
    private int nrBallINeed;

    private void Awake()
    {
        //Application.targetFrameRate = 60;

        Time.timeScale = 1;
        score_Rows = 1;
        amountBalls = 1;                                                                // std = 1
        scoreLevel = 0;
        amountBallsBack = amountCollectBallsLeft = 0;
        sd = MobileInputs.Instance.swipeDelta;
        sd.Set(-sd.x, -sd.y);
        isBreakingStuff = allBallLanded = firstBallLanded = false;
        showsABExit = true;
        updateInputs = true;
        AddBallUI = 0;
    }

    private void Start()
    {
        BallsList = new List<GameObject>();
        BallsList.Add(ballOr);
        nrBallINeed = amountBalls - 1;
        GenerateBalls(nrBallINeed, false);
        ballColor = Ball.Instance.GetComponent<Image>().color;
        amountBallsText = amountBallsTextPr.GetComponentInChildren<TextMeshProUGUI>();
        ballCopyColor = ballColor;
        //ballCopyColor.a = 0.8f;
        ballsPreview.parent.gameObject.SetActive(false);
        timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;
        UpdateUIText();
        ShowAmBallsExitText(amountBalls);
        amountBallsLeft = amountBalls;
        ballOrgYPos = ballOr.transform.position.y;
        bonus_02IsReady = onBoostSpeed = BoostSpeedButton.interactable = false;
        targetBallPosLanded = ballOr.GetComponent<RectTransform>().position;
        amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);
    }

    private void Update()
    {
        if(Time.timeScale != 0)
        {
            if (!isBreakingStuff)
                PoolInput();
            if (allBallLanded)
            {
                GenerateBalls(nrBallINeed, false);
                score_Rows++;
                onBoostSpeed = false;
                timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;//TIMEWAITBOOSTSPEED + (amountBalls / 5f);
                BoostSpeedButtonAnim(true);
                LevelManager.Instance.GenerateRow();
                UpdateUIText();
                ShowAmBallsExitText(amountBalls);
                allBallLanded = false;
            }
            if (onBoostSpeed)
            {
                timeWaitBoostSpeed -= Time.deltaTime;
                if (timeWaitBoostSpeed < 0)
                {
                    BoostSpeedButtonAnim(false);
                    timeWaitBoostSpeed = TIMEWAITBOOSTSPEED + (amountBalls / 10f);
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
                        GenerateBalls(1, true);
                    }
                    ballsPreview.parent.gameObject.SetActive(false);
                    StartCoroutine(FireBalls(sd.normalized));
                    Bonus.Instance.ActivateButton(false);
                }
            }
        }
    }

    private void GenerateBalls(int nrBallINeed, bool isBonBall)
    {
        if (!isBonBall)
        {
            for (int i = 0; i < nrBallINeed; i++)
            {
                GameObject go = Instantiate(ballCopyPr, ballContainer) as GameObject;
                go.SetActive(false);
                BallsList.Add(go);
            }
        }
        else
        {
            for (int i = 0; i < nrBallINeed; i++)
            {
                GameObject go = Instantiate(bonus_02Pr, Space2D) as GameObject;
                go.SetActive(false);
                BallsList.Add(go);
            }
            bonus_02IsReady = false;
        }
        nrBallINeed = 0;
    }

    private IEnumerator FireBalls(Vector2 sd)
    {
        int AmountBalls = BallsList.Count - 1;

        Vector2 posIn = targetBallPosLanded;
        for (int i = BallsList.Count - 1; i > 0; i--)
        {
            if (BallsList[i].GetComponent<BallCopy>())
            {
                BallCopy ballCopy = BallsList[i].GetComponent<BallCopy>();
                ballCopy.ballPos = posIn;
                ballCopy.speed = BALLSPEED;
                BallsList[i].SetActive(true);
                ballCopy.SendBallInDirection(sd);
                AmountBalls--;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                Ball_Bonus_02 ballCopy = BallsList[i].GetComponent<Ball_Bonus_02>();
                ballCopy.ballPos = posIn;
                ballCopy.speed = BALLSPEED;
                BallsList[i].SetActive(true);
                ballCopy.SendBallInDirection(sd);
                AmountBalls--;
                BallsList.RemoveAt(i);
                yield return new WaitForSeconds(0.1f);
            }
            ShowAmBallsExitText(AmountBalls);
        }
        BallsList[0].GetComponent<BallOrg>().speed = BALLSPEED;
        BallsList[0].GetComponent<BallOrg>().SendBallInDirection(sd);
        ShowAmBallsExitText(AmountBalls);
        if (AmountBalls <= 0)
        {
            showsABExit = false;
        }
        yield return new WaitForSeconds(0.1f);
    }

    public bool FirstBallLanded(Vector2 ballPosX)
    {
        bool DisplayAtFloor = false;
        if (!firstBallLanded)
        {
            targetBallPosLanded = ballPosX;
            DisplayAtFloor = true;
        }
        firstBallLanded = true;
        return DisplayAtFloor;
    }

    public void IsAllBallLanded()
    {
        ShowAmBallsEnterText(++amountBallsBack);
        amountBallsLeft--;
        if (amountBallsLeft <= 0)
        {
            if (amountCollectBallsLeft <= 0)
                AllBallLanded();
        }
    }

    private void AllBallLanded()
    {
        Time.timeScale = 1f;
        isBreakingStuff = false;
        firstBallLanded = false;
        amountBallsLeft = amountBalls;
        nrBallINeed = AddBallUI;
        AddBallUI = 0;
        amountBallsBack = amountCollectBallsLeft = 0;
        targetBallPosLanded = ballOr.transform.position;
        ScoreLEVEL.Instance.ResetSetting();
        Bonus.Instance.ActivateButton(true);
        showsABExit = true;
        Time.timeScale = 1f;
        allBallLanded = updateInputs = true;
    }

    private void ShowAmBallsExitText(int amountBallsShowExit)
    {
        amountBallsText.text = 'x' + amountBallsShowExit.ToString();

        if (amountBallsShowExit <= 0)
            amountBallsTextPr.GetComponentInChildren<Animator>().SetTrigger("Exit");
    }

    private void ShowAmBallsEnterText(int amountBallsShowEnter)
    {
        if(!showsABExit)
        {
            amountBallsText.text = 'x' + amountBallsShowEnter.ToString();

            if(amountBallsShowEnter >= amountBalls)
                amountBallsTextPr.GetComponentInChildren<Animator>().SetTrigger("Enter");

            amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);
            if (amountBallsTextPr.GetComponent<RectTransform>().position.x >= 0.9f)
                amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(-0.2f, 0.1f);
            else if (amountBallsTextPr.GetComponent<RectTransform>().position.x <= -0.7f)
                amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.3f, 0.1f);
        }
    }

    public void UpdateUIText()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = score_Rows.ToString();
        Bonus.Instance.UpdateUIText();
    }

    public void OnLoseMenu()
    {
        canvas2.SetActive(true);
        loseMenu.SetActive(true);
        statusBar.SetActive(false);
        score_Rows--;
        loseMenu.GetComponent<UpdateLoseMenu>().UpdateGameStatus();
        loseMenu.GetComponent<Animator>().SetTrigger("PanelON");
        Time.timeScale = 0f;
    }

    public Color ChangeColor(int hp)
    {
        int colorID = 0;
        if (hp / 2 > blockColor.Length - 1)
            colorID = blockColor.Length - 1;
        else
            colorID = hp / 2;
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
