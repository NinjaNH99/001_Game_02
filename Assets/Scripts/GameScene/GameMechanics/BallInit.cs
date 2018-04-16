using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallInit : MonoSingleton<BallInit>
{
    private const float BALLSPEED = 4f;

    public float ballSpeedGet { get { return BALLSPEED; } }

    public RectTransform ballContainer;
    public GameObject gameControllerObj, ballCopyPr, bonus_02Pr, amountBallsTextPr, ballOr;
    public Transform Space2D, ballLaser;
    public LineRenderer lineRend;

    public Color ballColor;
    public Color ballCopyColor;
    public Vector2 targetBallPosLanded = new Vector2(GameData.posXBall, -1.5f);
    public Vector2 shootDir;

    public int nrBallINeed = GameData.amountBalls - 1;
    public bool createBallBomb = false, showsABExit = true, ballBambReady = false;
    public float ballOrgYPos;

    private TextMeshProUGUI amountBallsText;
    //private GameController gameController;
    public List<GameObject> BallsList = new List<GameObject>();

    private Vector2 sd;

    private void Awake()
    {
        //gameController = gameControllerObj.GetComponent<GameController>();
        BallsList = new List<GameObject>();
        ballOr.GetComponent<RectTransform>().position = new Vector2(GameData.posXBall, -1.5f);
        ballOrgYPos = ballOr.transform.position.y;

        amountBallsText = amountBallsTextPr.GetComponentInChildren<TextMeshProUGUI>();
        sd = MobileInputs.Instance.swipeDelta;
        sd.Set(-sd.x, -sd.y);
        shootDir = sd.normalized;

        showsABExit = true;
        lineRend.enabled = true;
        BallsList.Add(ballOr);
        ballLaser.gameObject.SetActive(false);
        createBallBomb = ballBambReady = false;

        nrBallINeed = GameData.amountBalls - 1;

        GenerateBalls();
        AlignAmBalTextPr();
    }

    private void Start()
    {
        ballColor = Ball.Instance.GetComponent<Image>().color;
        ballCopyColor = ballColor;
        targetBallPosLanded = ballOr.GetComponent<RectTransform>().position;
        amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);

        ShowAmBallsExitText(GameData.amountBalls);
    }

    public void PoolInput()
    {
        sd = MobileInputs.Instance.swipeDelta;
        sd.Set(-sd.x, -sd.y);
        if (sd != Vector2.zero)
        {
            if (sd.y < 3.5f)
            {
                lineRend.enabled = false;
                ballLaser.gameObject.SetActive(false);
            }
            else
            {
                ballLaser.up = sd.normalized;
                ballLaser.gameObject.SetActive(true);
                lineRend.enabled = true;
                if (MobileInputs.Instance.release)
                {
                    GameController.Instance.tutorialContainer.SetActive(false);
                    GameController.Instance.isBreakingStuff = GameController.Instance.onBoostSpeed = true;
                    GameController.Instance.updateInputs = false;
                    MobileInputs.Instance.Reset();
                    //if (bonus_02IsReady)
                        //GenerateBalls(true);
                    lineRend.enabled = false;
                    ballLaser.gameObject.SetActive(false);
                    shootDir = sd.normalized;
                    StartCoroutine(FireBalls());
                    Bonus.Instance.ActivateButton(false);
                }
            }
        }
    }

    public void GenerateBalls()
    {
        if (!createBallBomb)
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
            GameObject go = Instantiate(bonus_02Pr, Space2D) as GameObject;
            go.SetActive(false);
            BallsList.Add(go);
            createBallBomb = false;
            ballBambReady = true;
        }
        nrBallINeed = 0;
    }

    public IEnumerator FireBalls()
    {
        int AmountBalls = BallsList.Count - 1;
        //shootDir = sd;
        //Vector2 posIn = targetBallPosLanded;

        for (int i = BallsList.Count - 1; i > 0; i--)
        {
            if(ballBambReady)
            {
                //Ball_Bonus_02 ballCopy = BallsList[i].GetComponent<Ball_Bonus_02>();
                //ballCopy.ballPos = posIn;
                //ballCopy.speed = BALLSPEED;
                BallsList[i].SetActive(true);
                //ballCopy.SendBallInDirection(sd);
                AmountBalls--;
                BallsList.RemoveAt(i);

                ballBambReady = false;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                //BallCopy ballCopy = BallsList[i].GetComponent<BallCopy>();
                //ballCopy.ballPos = posIn;
                //ballCopy.speed = BALLSPEED;
                BallsList[i].SetActive(true);
                //ballCopy.SendBallInDirection(sd);
                AmountBalls--;
                yield return new WaitForSeconds(0.1f);
            }

            ShowAmBallsExitText(AmountBalls);
        }

        BallsList[0].GetComponent<BallOrg>().speed = ballSpeedGet;
        BallsList[0].GetComponent<BallOrg>().SendBallInDirection(shootDir);

        ShowAmBallsExitText(AmountBalls);
        if (AmountBalls <= 0)
        {
            showsABExit = false;
        }
        yield return new WaitForSeconds(0.1f);
    }
    
    public void ShowAmBallsExitText(int amountBallsShowExit)
    {
        amountBallsText.text = 'x' + amountBallsShowExit.ToString();

        if (amountBallsShowExit <= 0)
            amountBallsTextPr.GetComponentInChildren<Animator>().SetTrigger("Exit");
    }

    public void ShowAmBallsEnterText(int amountBallsShowEnter)
    {
        if (!showsABExit)
        {
            amountBallsText.text = 'x' + amountBallsShowEnter.ToString();

            if (amountBallsShowEnter >= GameData.amountBalls)
                amountBallsTextPr.GetComponentInChildren<Animator>().SetTrigger("Enter");
            AlignAmBalTextPr();
        }
    }

    private void AlignAmBalTextPr()
    {
        amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);
        if (amountBallsTextPr.GetComponent<RectTransform>().position.x >= 0.9f)
            amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(-0.2f, 0.1f);
        else if (amountBallsTextPr.GetComponent<RectTransform>().position.x <= -0.7f)
            amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.3f, 0.1f);
    }
}
