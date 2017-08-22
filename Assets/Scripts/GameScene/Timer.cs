using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public const float TIMESPEED = 8.5f;

    private Image timer;

    public float scoreTime;
    [Range(0f, 1f)]
    public float i;

    private void Start()
    {
        timer = GetComponent<Image>();
        scoreTime = 0f;
        i = 1;
    }

    private void Update()
    {
        if (!GameController.isBreakingStuff)
        {
            i -= Time.deltaTime / TimeSpeed(scoreTime);
            timer.fillAmount = i;
            if (i < -0.03f)
            {
                i = 1;
                timer.fillAmount = 1;
                LevelContainer.Instance.GenerateNewRow();
            }
        }
        else
        {
            i += Time.deltaTime / 5f;
            timer.fillAmount = i;
            if (i > 1)
            {
                i = 1;
                GameController.Instance.IsAllBallLanded(false);
                return;
            }
        }
    }

    private float TimeSpeed(float scoreTime)
    {
        float rez = 0f;
        if (scoreTime >= TIMESPEED)
            rez = TIMESPEED;
        else
            rez = TIMESPEED - scoreTime;
        return rez;
    }

}
