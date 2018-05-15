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
        gameContr = GameController.Instance;
        LevelManager.Instance.bossObj = this;
        shieldObj.SetActive(false);
    }

    private void Start()
    {
        hpBossText = hpBossObj.GetComponent<TextMeshProUGUI>();
        
        LoadHP();

        isDestroy = true;
        GetComponent<SpriteRenderer>().color = gameContr.ChangeColor(hpBoss);

        if(shieldOn)
        {
            shieldObj.SetActive(true);
            hpShieldText = shieldHPObj.GetComponent<TextMeshProUGUI>();
            shieldObj.GetComponent<Image>().color = GetComponent<SpriteRenderer>().color;
            hpShieldText.text = hpShield.ToString();
        }

        animBoss = GetComponent<Animator>();
        EventManager.EvMoveDownM += UpdateStats;
    }

    private void LoadHP()
    {
        int newHP = GetComponentInParent<Container>().LoadData().hp;
        int shieldON1 = GetComponentInParent<Container>().LoadData().shieldON;

        if (newHP <= 0)
            hpBoss = GetComponentInParent<Row>().rowHP * 2;
        else
            hpBoss = newHP;

        if (shieldON1 == 0)
            //hpShield = hpShieldReset = GetComponentInParent<Row>().rowID;
            shieldOn = false;
        else if (shieldON1 == 1)
        {
            hpShield = hpShieldReset = GetComponentInParent<Row>().rowHP;
            //hpShield = hpShieldReset = newHPShiel;
            shieldOn = true;
        }
        else if (shieldON1 == 2)
        {
            //int newHPShiel = GetComponentInParent<Container>().LoadData().shield;
            hpShield = hpShieldReset = GetComponentInParent<Container>().LoadData().shield;
            shieldOn = true;
        }
        hpBossText.text = hpBoss.ToString();
        //hpShieldText.text = hpShield.ToString();
    }

    private void UpdateStats()
    {
        if(shieldOn)
            GetComponentInParent<Container>().UpdateData(9, hpBoss, hpShield, 2);
        else
            GetComponentInParent<Container>().UpdateData(9, hpBoss, hpShield, 0);
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
            var main = go.GetComponent<ParticleSystem>().main;
            main.startColor = GetComponent<SpriteRenderer>().color;


            GetComponentInParent<Container>().AddInListFreeConts();

            EventManager.EvMoveDownM -= UpdateStats;

            Destroy(go, 1f);
            Destroy(gameObject, 3);
            if (isDestroy)
            {
                ScoreLEVEL.Instance.AddScoreLevel();
                ScoreLEVEL.Instance.ShowNrBlock(containerPos);
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
        UpdateStats();
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
