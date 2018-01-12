using UnityEngine;
using UnityEngine.UI;

public class TimerGravity : MonoSingleton<TimerGravity>
{
    private const float TIMESPEED = 10.0f; // 10

    public GameObject timerBG;
    public float nrBalls;
    public bool checkTime;
    [Range(0f, 1f)]
    public float i;

    private Image timerImg, timerBGimg;

    private void Start()
    {
        timerImg = GetComponent<Image>();
        timerBGimg = timerBG.GetComponent<Image>();
        nrBalls = 0f;
        i = 1;
        checkTime = false;
        ChangeColor();
    }

    private void Update()
    {
        if (GameController.startTimerGravity)
        {
            checkTime = true;
            i -= Time.deltaTime / (TIMESPEED + nrBalls);
            timerImg.fillAmount = i;
            if (i < -0.03f)
            {
                i = 0;
                timerImg.fillAmount = 0;
                Ball.Instance.startFall = true;
                Ball.Instance.startFall = true;
            }
        }
        else if (!GameController.startTimerGravity && checkTime)
        {
            i += Time.deltaTime / 1.5f;
            timerImg.fillAmount = i;
            if (i > 1)
            {
                i = 1;
                Ball.Instance.startFall = false;
                Ball.Instance.startFall = false;
                checkTime = false;
                GameController.Instance.IsAllBallLanded();
                ChangeColor();
                return;
            }
        }
    }

    private void ChangeColor()
    {
        var colorScore = GameController.score;

        timerImg.color = GameController.Instance.ChangeColor(colorScore);
        timerImg.SetTransparency(0.4f);

        timerBGimg.color = timerImg.color;
        timerBGimg.SetTransparency(0.08f);
    }

}
