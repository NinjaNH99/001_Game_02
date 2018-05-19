using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoSingleton<GameController>
{
    private const float TIMEWAITBOOSTSPEED = 2f; // 1.5

    public Transform Canvas1, Space2D;
    public GameObject tutorialContainer;

    // Canvas 2
    public GameObject canvas2, loseMenu, statusBar, bonus_02Pr;
    public Color[] blockColor;

    [HideInInspector]
    public bool isBreakingStuff = false, updateInputs = true, allBallLanded = false, onBoostSpeed = false, isShoot = false;
    [HideInInspector]
    public int amountBallsLeft = 0, amountCollectBallsLeft = 0, AddBallUI = 0;

    public GameObject scoreText, maxScoreText, ballInitObj;
    public Button BoostSpeedButton;

    private BallInit ballInit;
    private int amountBallsBack = 0;
    private float timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;
    private Vector2 posBallFolled;

    private void Awake()
    {
        ballInit = ballInitObj.GetComponent<BallInit>();

        amountBallsLeft = GameData.amountBalls;
        amountBallsBack = amountCollectBallsLeft = 0;
        isBreakingStuff = allBallLanded = isShoot = false;
        ballInit.showsABExit = true;
        updateInputs = true;
        AddBallUI = 0;
        posBallFolled = new Vector2(GameData.posXBall, 0);
        timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;
        onBoostSpeed = BoostSpeedButton.interactable = false;
        tutorialContainer.SetActive(true);
    }

    private void Start()
    {
        IncreaseMaxScore();
    }

    private void Update()
    {
        if(Time.timeScale != 0 && GameData.loadDataDone)
        {
            if (!isBreakingStuff)
                ballInit.PoolInput();
            if(isShoot)
            {
                //GameData.loadData = false;
                tutorialContainer.SetActive(false);

                updateInputs = false;
                MobileInputs.Instance.Reset();
                isBreakingStuff = onBoostSpeed = true;
                isShoot = false;
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
        UpdateUIText();
    }

    public void BallLanded(Vector2 ballPosX)
    {
        GameData.posXBall = ballPosX.x;
        posBallFolled = ballPosX;
        ballInit.targetBallPosLanded = ballPosX;
    }

    public void IsAllBallLanded()
    {
        ballInit.ShowAmBallsEnterText(++amountBallsBack);
        amountBallsLeft--;
        if (amountBallsLeft <= 0 && amountCollectBallsLeft <= 0)
        {
            AllBallLanded();
        }
    }

    private void AllBallLanded()
    {
        Time.timeScale = 1f;
        isBreakingStuff = onBoostSpeed = false;

        EventManager.StartEvDeSpawn();

        ballInit.targetBallPosLanded = ballInit.shootBallPos = posBallFolled;

        amountBallsLeft = GameData.amountBalls;
        ballInit.nrBallINeed = AddBallUI;

        amountBallsBack = amountCollectBallsLeft = AddBallUI = 0;
        ballInit.targetBallPosLanded = ballInit.ballOr.transform.position;
        ScoreLEVEL.Instance.ResetSetting();
        Bonus.Instance.ActivateButton(true);

        allBallLanded = updateInputs = ballInit.showsABExit = true;

        ballInit.GenerateBalls();
        GameData.score_Rows++;
        IncreaseMaxScore();

        timeWaitBoostSpeed = TIMEWAITBOOSTSPEED;
        BoostSpeedButtonAnim(true);
        ballInit.ShowAmBallsExitText(GameData.amountBalls);
        //EventManager.StartEvUpdateData();
        LevelManager.Instance.GenerateMapNewGame();
    }
    
    public void UpdateUIText()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = GameData.score_Rows.ToString();
        maxScoreText.GetComponent<TextMeshProUGUI>().text = GameData.maxScore.ToString();
        Bonus.Instance.UpdateUIText();
    }

    public void OnLoseMenu()
    {
        GameData.score_Rows--;
        GameData.LoseGameSaveData();
        canvas2.SetActive(true);
        loseMenu.SetActive(true);
        statusBar.SetActive(false);
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
