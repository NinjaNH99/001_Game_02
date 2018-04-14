using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallInit : MonoSingleton<BallInit>
{
    public const float BALLSPEED = 4f;

    public RectTransform ballContainer;
    public GameObject ballCopyPr, bonus_02Pr, amountBallsTextPr, ballOr;
    public Transform Space2D, ballLaser;
    public LineRenderer lineRend;

    public Color ballColor;
    public Color ballCopyColor;
    public Vector2 targetBallPosLanded;

    public bool bonus_02IsReady, showsABExit = true;
    public float ballOrgYPos;

    private TextMeshProUGUI amountBallsText;
    private GameController gameController;
    private List<GameObject> BallsList;

    private Vector2 sd;

    private void Awake()
    {
        gameController = GetComponent<GameController>();
        Debug.Log("Awake BallInit");
        BallsList = new List<GameObject>();
        //ballOrgYPos = ballOr.transform.position.y;
        //targetBallPosLanded = ballOr.GetComponent<RectTransform>().position;

        amountBallsText = amountBallsTextPr.GetComponentInChildren<TextMeshProUGUI>();
        //amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);
        sd = MobileInputs.Instance.swipeDelta;
        sd.Set(-sd.x, -sd.y);

        showsABExit = true;
        
        BallsList.Add(ballOr);
        ballLaser.gameObject.SetActive(false);
        bonus_02IsReady = lineRend.enabled = false;
    }

    private void Start()
    {
        ballColor = Ball.Instance.GetComponent<Image>().color;
        ballCopyColor = ballColor;
        ballOrgYPos = ballOr.transform.position.y;
        targetBallPosLanded = ballOr.GetComponent<RectTransform>().position;
        amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);
        //GenerateBalls(amountBalls - 1, false);
        //ShowAmBallsExitText(GameData.amountBalls);
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
                    gameController.tutorialContainer.SetActive(false);
                    gameController.isBreakingStuff = gameController.onBoostSpeed = true;
                    gameController.updateInputs = false;
                    MobileInputs.Instance.Reset();
                    if (bonus_02IsReady)
                    {
                        GenerateBalls(1, true);
                    }
                    lineRend.enabled = false;
                    ballLaser.gameObject.SetActive(false);
                    StartCoroutine(FireBalls(sd.normalized));
                    Bonus.Instance.ActivateButton(false);
                }
            }
        }
    }

    public void GenerateBalls(int nrBallINeed, bool isBonBall)
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

    public IEnumerator FireBalls(Vector2 sd)
    {
        int AmountBalls = BallsList.Count - 1;

        Vector2 posIn = targetBallPosLanded;
        //Vector2 posIn = new Vector2(GameData.posXBall, ballOrgYPos);

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

            amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.12f, 0.12f);
            if (amountBallsTextPr.GetComponent<RectTransform>().position.x >= 0.9f)
                amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(-0.2f, 0.1f);
            else if (amountBallsTextPr.GetComponent<RectTransform>().position.x <= -0.7f)
                amountBallsTextPr.GetComponent<RectTransform>().position = targetBallPosLanded + new Vector2(0.3f, 0.1f);
        }
    }
    
}
