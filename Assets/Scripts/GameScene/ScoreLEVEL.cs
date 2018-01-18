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

    private Image timerImg, timerBGimg;
    private bool checkTimer, addTimer;
    private float x, spidT;

    private void Start()
    {
        addTimer = checkTimer = false;
        timerImg = GetComponent<Image>();
        timerBGimg = timerBG.GetComponent<Image>();
        i = spidT = 0;
        nrBlDestroy = 1;
        timerImg.fillAmount = i;
        ChangeColor();
        ResetSetting();
    }


    private void Update()
    {
        if (addTimer)
        {
            spidT += Time.deltaTime / 1.5f;
            timerImg.fillAmount = spidT;
            if (spidT >= i)
            {
                timerImg.fillAmount = spidT;
                addTimer = false;
            }
        }
        else if(!addTimer && checkTimer)
        {
            spidT -= Time.deltaTime / 10;
            timerImg.fillAmount = spidT;
            if (spidT <= 0)
            {
                spidT = 0;
                checkTimer = false;
            }
        }
    }

    public void AddScoreLevel()
    {
        nrBlDestroy++;
        i = x * nrBlDestroy;
        //timerImg.fillAmount += i;
        CheckStar(timerImg.fillAmount);
        addTimer = true;
    }

    private void CheckStar(float  i)
    {
        if (i >= 0.2f)
        {
            stars[0].GetComponent<ShowStar>().Staroption();
        }
        else if(i >= 0.49f)
        {
            stars[1].GetComponent<ShowStar>().Staroption();
        }
        else if(i >= 0.74f)
        {
            stars[2].GetComponent<ShowStar>().Staroption();
        }
        else if(i >= 0.95f)
        {
            stars[3].GetComponent<ShowStar>().Staroption();
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
        x = (1000f / maxScore) / 100f;
    }

    private void ChangeColor()
    {
        var colorScore = GameController.score_Rows;

        timerImg.color = GameController.Instance.ChangeColor(colorScore);
        timerImg.SetTransparency(0.4f);

        timerBGimg.color = timerImg.color;
        timerBGimg.SetTransparency(0.08f);
    }

}
