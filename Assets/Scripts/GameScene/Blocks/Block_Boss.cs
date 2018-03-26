using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block_Boss : MonoBehaviour
{
    public GameObject shieldObj, shieldHPObj;
    public GameObject hpBossObj;
    public GameObject DeathEFX;
    public int hpBoss, hpShield;

    private RectTransform containerPos;
    private GameController gameContr;
    private TextMeshProUGUI hpBossText, hpShieldText;
    private Animator animBoss, animShield;
    private bool isDestroy, shieldOn;
    private int hpShieldReset;

    private void Awake()
    {
        containerPos = GetComponentInParent<Container>().gameObject.GetComponent<RectTransform>();
        //containerPos = GetComponent<RectTransform>();
        gameContr = GameController.Instance;
        LevelManager.Instance.bossObj = this;
    }

    private void Start()
    {
        hpBossText = hpBossObj.GetComponent<TextMeshProUGUI>();
        hpShieldText = shieldHPObj.GetComponent<TextMeshProUGUI>();
        hpBoss = gameContr.score_Rows * 2;
        hpShieldReset = hpShield = gameContr.score_Rows;
        hpBossText.text = hpBoss.ToString();
        hpShieldText.text = hpShield.ToString();
        isDestroy = shieldOn = true;
        GetComponent<SpriteRenderer>().color = gameContr.ChangeColor(hpBoss);
        shieldObj.GetComponent<Image>().color = GetComponent<SpriteRenderer>().color;
        animBoss = GetComponent<Animator>();
        //animShield = shieldObj.GetComponent<Animator>();
    }

    public void ReceiveHitBoss()
    {
        if (shieldOn)
        {
            ReceiveHitShield();
            return;
        }

        hpBoss--;
        if (hpBoss > 0)
            animBoss.SetTrigger("Hit");
        if (hpBoss <= 0)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;

            hpBossText.text = "1";
            GameObject go = Instantiate(DeathEFX, containerPos) as GameObject;
            //go.GetComponent<Transform>().localScale = new Vector2(33, 33);
            var main = go.GetComponent<ParticleSystem>().main;
            main.startColor = GetComponent<SpriteRenderer>().color;

            Destroy(go, 1f);
            Destroy(gameObject, 3);
            if (isDestroy)
            {
                GetComponentInParent<Row>().CheckNrConts();
                ScoreLEVEL.Instance.AddScoreLevel();
                ScoreLEVEL.Instance.ShowNrBlock(containerPos);
                Bonus.Instance.AddBonus_01();
                Bonus.Instance.AddBonus_01();
                Bonus.Instance.AddBonus_02();
                isDestroy = false;
            }
            gameObject.SetActive(false);
            return;
        }
        hpBossText.text = hpBoss.ToString();
        GetComponent<SpriteRenderer>().color = gameContr.ChangeColor(hpBoss);
    }

    public void ReceiveHitShield()
    {
        hpShield--;
        if (hpShield > 0)
            animBoss.SetTrigger("Hit");
        if (hpShield <= 0)
        {
            shieldOn = false;
            hpShieldText.text = "1";
            GameObject go = Instantiate(DeathEFX, containerPos) as GameObject;
            //go.GetComponent<Transform>().localScale = new Vector2(2, 2);
            var main = go.GetComponent<ParticleSystem>().main;
            main.startColor = GetComponent<SpriteRenderer>().color;

            Destroy(go, 1f);
            Destroy(shieldObj, 1f);

            shieldObj.SetActive(false);
            return;
        }
        hpShieldText.text = hpShield.ToString();
        shieldObj.GetComponent<Image>().color = GetComponent<SpriteRenderer>().color;
    }

    public void ResetShield()
    {
        if (!shieldOn)
            return;

        hpShield = hpShieldReset;
        hpShieldText.text = hpShield.ToString();
        shieldObj.GetComponent<Image>().color = GetComponent<SpriteRenderer>().color;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy) || coll.gameObject.CompareTag(Tags.ballSQLine) || coll.gameObject.CompareTag(Tags.LaserSq))
        {
            if (shieldOn)
                ReceiveHitShield();
            else
                ReceiveHitBoss();
        }
    }

}
