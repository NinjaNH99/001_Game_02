using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreLEVEL : MonoSingleton<ScoreLEVEL>
{
    public const int MAXSCore = 2000;

    public GameObject timerBG;
    public GameObject[] points;
    public GameObject[] stars;

    public Transform BackgroundPr;
    public GameObject AddScoreUIPr;

    public TextMeshProUGUI PointsScore;

    [Range(0f, 1f), HideInInspector]
    public float i;
    [HideInInspector]
    public int nrBlDestroy, showPoints;

    private GameController gameContr;
    private Image timerImg, timerBGimg;
    private bool checkTimer, addTimer, ckStar1, ckStar2, ckStar3;
    private float x, spidT, minPosT;

    private void Awake()
    {
        ckStar1 = ckStar2 = ckStar3 = true;
        x = (1000f / MAXSCore) / 100f;
        addTimer = checkTimer = false;
        timerImg = GetComponent<Image>();
        timerBGimg = timerBG.GetComponent<Image>();
        i = 0;
        minPosT = 0;
        showPoints = MAXSCore;
        spidT = minPosT;
        nrBlDestroy = 0;
        timerImg.fillAmount = i;
        PointsScore.text = showPoints.ToString();
    }

    private void Start()
    {
        gameContr = GameController.Instance;
        ResetSetting();
        ChangeColor();
    }


    private void Update()
    {
        if (addTimer)
        {
            spidT += Time.deltaTime / 5f;
            timerImg.fillAmount = spidT;
            CheckStar(spidT);
            if (spidT >= i)
            {
                timerImg.fillAmount = spidT;
                addTimer = false;
            }
        }
        else if (!addTimer && checkTimer)
        {
            spidT -= Time.deltaTime / 2f;
            timerImg.fillAmount = spidT;
            if (spidT <= minPosT)
            {
                spidT = minPosT;
                ChangeColor();
                checkTimer = false;
            }
        }
    }

    public void AddScoreLevel()
    {
        nrBlDestroy++;
        i += x * nrBlDestroy;
        minPosT += 0.0015f;
        if (showPoints >= 0)
            showPoints -= nrBlDestroy;
        else
            showPoints = 0;
        PointsScore.text = showPoints.ToString();
        addTimer = true;
    }

    private void CheckStar(float  i)
    {
        if(i >= 0.98f && ckStar3)
        {
            points[2].GetComponent<ShowPoint>().Staroption(timerImg.color);
            stars[2].GetComponent<ShowStars>().ShowStar();
            ckStar3 = false;
            gameContr.OnEndMenu();
        }
        else if(i >= 0.64f && ckStar2)
        {
            points[1].GetComponent<ShowPoint>().Staroption(timerImg.color);
            stars[1].GetComponent<ShowStars>().ShowStar();
            ckStar2 = false;
        }
        else if(i >= 0.35f && ckStar1)
        {
            points[0].GetComponent<ShowPoint>().Staroption(timerImg.color);
            stars[0].GetComponent<ShowStars>().ShowStar();
            ckStar1 = false;
        }
    }

    public void ShowNrBlock(Transform objTr)
    {
        GameObject go = Instantiate(AddScoreUIPr, objTr) as GameObject;
        go.GetComponentInChildren<AddScoreLevelUI>().Show(nrBlDestroy);
        Destroy(go, 1.3f);
    }

    public void ResetSetting()
    {
        checkTimer = true;
        nrBlDestroy = 1;
        i = 0;
        timerImg.fillAmount = i;
    }

    private void ChangeColor()
    {
        var colorScore = gameContr.score_Rows;

        timerImg.color = gameContr.ChangeColor(colorScore);
        timerImg.SetTransparency(0.4f);

        timerBGimg.color = timerImg.color;
        timerBGimg.SetTransparency(0.08f);
        for (int i = 0; i < points.Length; i++)
        {
            points[i].GetComponent<ShowPoint>().ChangeColor(colorScore, timerImg.color);
        }
    }

}
