using UnityEngine;
using UnityEngine.UI;

public class ScoreLEVEL : MonoSingleton<ScoreLEVEL>
{
    public GameObject timerBG;
    public GameObject[] stars;

    public Transform BackgroundPr;
    public GameObject AddScoreUIPr;

    public int maxScore = 2000;

    [Range(0f, 1f), HideInInspector]
    public float i;
    [HideInInspector]
    public int nrBlDestroy;

    private GameController gameContr;
    private Image timerImg, timerBGimg;
    private bool checkTimer, addTimer;
    private float x, spidT, minPosT;

    private void Start()
    {
        gameContr = GameController.Instance;
        x = (1000f / maxScore) / 100f;
        addTimer = checkTimer = false;
        timerImg = GetComponent<Image>();
        timerBGimg = timerBG.GetComponent<Image>();
        i = 0;
        minPosT = 0;
        spidT = minPosT;
        nrBlDestroy = 0;
        timerImg.fillAmount = i;
        ChangeColor();
        ResetSetting();
    }


    private void Update()
    {
        if (addTimer)
        {
            spidT += Time.deltaTime / 5f;
            timerImg.fillAmount = spidT;
            if (spidT >= i)
            {
                timerImg.fillAmount = spidT;
                CheckStar(timerImg.fillAmount);
                addTimer = false;
            }
        }
        else if(!addTimer && checkTimer)
        {
            spidT -= Time.deltaTime / 5f;
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
        minPosT += 0.001f;
        addTimer = true;
    }

    private void CheckStar(float  i)
    {
        if (i >= 0.95f)
        {
            stars[3].GetComponent<ShowStar>().Staroption(timerImg.color);
        }
        else if(i >= 0.74f)
        {
            stars[2].GetComponent<ShowStar>().Staroption(timerImg.color);
        }
        else if(i >= 0.49f)
        {
            stars[1].GetComponent<ShowStar>().Staroption(timerImg.color);
        }
        else if(i >= 0.2f)
        {
            stars[0].GetComponent<ShowStar>().Staroption(timerImg.color);
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
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].GetComponent<ShowStar>().ChangeColor(colorScore, timerImg.color);
        }
    }

}
