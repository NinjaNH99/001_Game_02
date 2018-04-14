using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoSingleton<GameController>
{
    private const float DEADZONE = 60.0f;
    private const float MAXIMUM_PULL = 200.0f;
    private const float TIMEWAITBOOSTSPEED = 2f; // 1.5

    public Transform Canvas1, Space2D;
    //public RectTransform ballContainer;
    public GameObject tutorialContainer/*, ballCopyPr ballOr*/;

    // Canvas 2
    public GameObject canvas2, loseMenu, statusBar, bonus_02Pr;

    //public Color ballColor;
    //public Color ballCopyColor;
    public Color[] blockColor;

    private BallInit ballInit;

    //public float ballOrgYPos;

    [HideInInspector]
    public bool isBreakingStuff = false, updateInputs, allBallLanded, firstBallLanded;
    //public Vector2 targetBallPosLanded;

    //public int amountBalls = 1;

    //private Vector2 sd;
    private float timeWaitBoostSpeed;

    // For UI Show
    public bool onBoostSpeed;
    [HideInInspector]
    public int amountBallsLeft, amountCollectBallsLeft, AddBallUI;
    private int amountBallsBack;
    //private bool showsABExit;
    //private TextMeshProUGUI amountBallsText;
    //public LineRenderer lineRend;
    public GameObject scoreText, maxScoreText/*, amountBallsTextPr*/;
    public Button BoostSpeedButton;
    //public Transform ballLaser;

    //For the new method of spawning the ball
    //private List<GameObject> BallsList;
    public int nrBallINeed;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        amountBallsBack = amountCollectBallsLeft = 0;
        isBreakingStuff = allBallLanded = firstBallLanded = false;
        //showsABExit = true;
        AddBallUI = 0;
        //score_Rows = 1;
        //amountBalls = 1;                                                                // std = 1
        //lineRend.enabled = false;

        //sd = MobileInputs.Instance.swipeDelta;
        //sd.Set(-sd.x, -sd.y);
        //BallsList = new List<GameObject>();
        //BallsList.Add(ballOr);
        //nrBallINeed = amountBalls - 1;
        //GenerateBalls(nrBallINeed, false);
        //amountBallsText = amountBallsTextPr.GetComponentInChildren<TextMeshProUGUI>();
        timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;
        //ballLaser.gameObject.SetActive(false);
        //amountBallsLeft = ballInit.amountBalls;
        //ballOrgYPos = ballOr.transform.position.y;
        onBoostSpeed = BoostSpeedButton.interactable = false;
        //targetBallPosLanded = ballOr.GetComponent<RectTransform>().position;
        //amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);
        updateInputs = true;
        tutorialContainer.SetActive(true);
    }

    private void Start()
    {
        ballInit = GetComponent<BallInit>();
        //ballColor = Ball.Instance.GetComponent<Image>().color;
        //ballCopyColor = ballColor;
        //ballCopyColor.a = 0.8f;
        //UpdateUIText();
        //amountBallsLeft = ballInit.amountBalls;
        ballInit.ShowAmBallsExitText(GameData.amountBalls);
        ballInit.GenerateBalls(GameData.amountBalls - 1, false);
        IncreaseMaxScore();
    }

    private void Update()
    {
        if(Time.timeScale != 0 && GameData.loadDataDone)
        {
            if (!isBreakingStuff)
                ballInit.PoolInput();
            if (allBallLanded)
            {
                ballInit.GenerateBalls(nrBallINeed, false);
                GameData.score_Rows++;
                IncreaseMaxScore();
                onBoostSpeed = false;
                timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;//TIMEWAITBOOSTSPEED + (amountBalls / 5f);
                BoostSpeedButtonAnim(true);
                //UpdateUIText();
                ballInit.ShowAmBallsExitText(GameData.amountBalls);
                allBallLanded = false;
                LevelManager.Instance.GenerateRow();
            }
            if (onBoostSpeed)
            {
                timeWaitBoostSpeed -= Time.deltaTime;
                if (timeWaitBoostSpeed < 0)
                {
                    BoostSpeedButtonAnim(false);
                    timeWaitBoostSpeed = TIMEWAITBOOSTSPEED + (GameData.amountBalls / 10f);
                    onBoostSpeed = false;
                }
            }
        }
    }

    public void IncreaseMaxScore()
    {
        if (GameData.score_Rows > GameData.maxScore)
            GameData.maxScore = GameData.score_Rows;
        if (Bonus.Instance.Bonus_01 > GameData.maxBonus_01)
            GameData.maxBonus_01 = Bonus.Instance.Bonus_01;
        if (Bonus.Instance.Bonus_02 > GameData.maxBonus_02)
            GameData.maxBonus_02 = Bonus.Instance.Bonus_02;
        UpdateUIText();
    }

    public bool FirstBallLanded(Vector2 ballPosX)
    {
        bool DisplayAtFloor = false;
        if (!firstBallLanded)
        {
            GameData.posXBall = ballPosX.x;
            ballInit.targetBallPosLanded = ballPosX;
            DisplayAtFloor = true;
        }
        firstBallLanded = true;
        return DisplayAtFloor;
    }

    public void IsAllBallLanded()
    {
        ballInit.ShowAmBallsEnterText(++amountBallsBack);
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
        amountBallsLeft = GameData.amountBalls;
        nrBallINeed = AddBallUI;
        AddBallUI = 0;
        amountBallsBack = amountCollectBallsLeft = 0;
        ballInit.targetBallPosLanded = ballInit.ballOr.transform.position;
        ScoreLEVEL.Instance.ResetSetting();
        Bonus.Instance.ActivateButton(true);
        ballInit.showsABExit = true;
        Time.timeScale = 1f;
        allBallLanded = updateInputs = true;
    }
    
    public void UpdateUIText()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = GameData.score_Rows.ToString();
        maxScoreText.GetComponent<TextMeshProUGUI>().text = GameData.maxScore.ToString();
        Bonus.Instance.UpdateUIText();
    }

    public void OnLoseMenu()
    {
        canvas2.SetActive(true);
        loseMenu.SetActive(true);
        statusBar.SetActive(false);
        GameData.score_Rows--;
        loseMenu.GetComponent<UpdateLoseMenu>().UpdateGameStatus();
        loseMenu.GetComponent<Animator>().SetTrigger("PanelON");
        Time.timeScale = 0f;
    }

    public Color ChangeColor(int hp)
    {
        int colorID = 0;
        if (hp / 3 > blockColor.Length - 1)
            colorID = blockColor.Length - 1;
        else
            colorID = hp / 3;
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
