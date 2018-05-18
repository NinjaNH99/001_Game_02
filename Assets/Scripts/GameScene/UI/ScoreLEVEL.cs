using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreLEVEL : MonoSingleton<ScoreLEVEL>
{
    public const int MAXSCore = 2000;

    public GameObject timerBG;
    public Animator anim;

    public Transform BackgroundPr;
    public GameObject AddScoreUIPr;

    public Material bgEFX;
    public Material timerMat0, timerMat1;

    [Range(0f, 1f), HideInInspector]
    public float i = 0;
    [HideInInspector]
    public int nrBlDestroy = 0;

    private GameController gameContr;
    private Image timerImg, timerBGimg;
    private bool checkTimer = false, addTimer = false;
    private float x = (1000f / MAXSCore) / 50f, spidT = 0;

    private void Awake()
    {
        x = (1000f / MAXSCore) / 50f;
        addTimer = checkTimer = false;
        timerImg = GetComponent<Image>();
        timerBGimg = timerBG.GetComponent<Image>();
        i = nrBlDestroy = 0;
        spidT = 0;
        timerImg.fillAmount = i;
        gameContr = GameController.Instance;
    }

    private void Start()
    {
        ResetSetting();
        ChangeColor();
    }
    
    private void Update()
    {
        if (addTimer)
        {
            spidT += Time.deltaTime / (6f - (nrBlDestroy / 5f));
            timerImg.fillAmount = spidT;
            if (spidT >= 1)
                CheckStar();
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
            if (spidT <= 0)
            {
                spidT = 0;
                ChangeColor();
                checkTimer = false;
            }
        }
    }

    public void BGShake(int op )
    {
        if(op == 0)
            anim.SetTrigger("Shake0");
        else if(op == 1)
            anim.SetTrigger("Shake1");
        else
            anim.SetTrigger("Shake2");
    }

    public void AddScoreLevel()
    {
        nrBlDestroy++;
        i += x * nrBlDestroy;
        //minPosT += 0.0015f;
        addTimer = true;
    }

    private void CheckStar()
    {
        spidT = i = 0;
        Bonus.Instance.AddBonus_01();
        Bonus.Instance.AddBonus_01();
        Bonus.Instance.AddBonus_02();
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
        var colorScore = GameData.score_Rows;

        timerImg.color = gameContr.ChangeColor(colorScore);
        timerImg.SetTransparency(0.4f);
        timerMat0.color = timerImg.color;

        timerBGimg.color = timerMat0.color;
        timerBGimg.SetTransparency(0.08f);
        timerMat1.color = timerBGimg.color;

        bgEFX.color = timerMat0.color;
    }

}
