using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public GameObject goHpText;
    public GameObject DeathEFX;
    public GameObject imageBonus = null;
    public int hp , hpx2 = 1;
    public bool isBonus = false;

    private RectTransform containerPos;
    private GameController gameContr;
    private TextMeshProUGUI hpText;
    private Animator anim;
    private bool isDestroy, isApplBonus;
    private int visualIndexCont, rowHP, rowID;

    private void Awake()
    {
        containerPos = GetComponentInParent<Container>().gameObject.GetComponent<RectTransform>();
        gameContr = GameController.Instance;
        GetComponent<RectTransform>().localScale = new Vector2(75, 75);
        anim = GetComponent<Animator>();
        visualIndexCont = GetComponentInParent<Container>().visualIndex;
    }

    private void Start()
    {
        rowHP = GetComponentInParent<Row>().rowHP - 1;
        rowID = GetComponentInParent<Row>().rowID - 1;
        hpText = goHpText.GetComponent<TextMeshProUGUI>();
        EventManager.EvUpdateDataSavedM += UpdateHP;
        LoadHP();
        
        isDestroy = isApplBonus = true;
        if (isBonus)
        {
            GetComponent<Image>().color = gameContr.ChangeColor(hp);
            anim.SetBool("IsBonus",true);
        }
        else
            GetComponent<SpriteRenderer>().color = gameContr.ChangeColor(hp);
        UpdateHP();
    }

    private void LoadHP()
    {
        int newHP = GetComponentInParent<Container>().LoadData().hp;
        if (newHP <= 0)
            this.hp = (rowHP + 1) * hpx2;
        else
            this.hp = newHP;
        hpText.text = this.hp.ToString();
    }

    private void UpdateHP()
    {
        if (isBonus)
            GetComponentInParent<Container>().UpdateData(6, this.hp);
        else
            GetComponentInParent<Container>().UpdateData(1, this.hp);
    }

    public void ReceiveHit(bool hitBlBon)
    {
        if (hitBlBon)
            this.hp = 0;
        else
            this.hp--;
        if (this.hp > 0)
        {
            if (isBonus)
                anim.SetBool("IsBonus", true);
            else
                anim.SetTrigger("Hit");
        }
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (isBonus && isApplBonus)
            {
                isApplBonus = false;
                GetComponentInParent<Container>().ApplySquare_Bonus();
            }
            hpText.text = "1";
            GameObject go = Instantiate(DeathEFX, containerPos) as GameObject;

            var main = go.GetComponent<ParticleSystem>().main;
            if (isBonus)
                main.startColor = GetComponent<Image>().color;
            else
                main.startColor = GetComponent<SpriteRenderer>().color;

            if (!isBonus)
                GetComponentInParent<Container>().AddInListFreeConts();
            EventManager.EvUpdateDataSavedM -= UpdateHP;
            Destroy(go, 1f);
            Destroy(gameObject, 1f);
            if (isDestroy)
            {
                ScoreLEVEL.Instance.AddScoreLevel();
                ScoreLEVEL.Instance.ShowNrBlock(containerPos);
                ScoreLEVEL.Instance.BGShake(Random.Range(0, 2));
                isDestroy = false;
            }
                        
            gameObject.SetActive(false);
            return;
        }
        hpText.text = hp.ToString();
        if (!isBonus)
            GetComponent<SpriteRenderer>().color = gameContr.ChangeColor(hp);
    }

    public void ReciveHitByBonus()
    {
        LevelManager.Instance.ApplyBallBonus(visualIndexCont, rowID - 1);
        LevelManager.Instance.ApplyBallBonus(visualIndexCont, rowID);
        LevelManager.Instance.ApplyBallBonus(visualIndexCont, rowID + 1);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy) || coll.gameObject.CompareTag(Tags.ballSQLine) || coll.gameObject.CompareTag(Tags.LaserSq))
            ReceiveHit(false);
        if (coll.gameObject.CompareTag(Tags.BallBomb))
        {
            ReciveHitByBonus();
        }
    }

}
