using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block : MonoSingleton<Block>
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

    private void Awake()
    {
        containerPos = GetComponentInParent<Container>().gameObject.GetComponent<RectTransform>();
        gameContr = GameController.Instance;
        GetComponent<RectTransform>().localScale = new Vector2(75, 75);
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //float rtime = Random.Range(2f, 5f);
        hpText = goHpText.GetComponent<TextMeshProUGUI>();
        hp = gameContr.score_Rows * hpx2;
        hpText.text = hp.ToString();
        isDestroy = isApplBonus = true;
        if (isBonus)
        {
            GetComponent<Image>().color = gameContr.ChangeColor(hp);
            anim.SetBool("IsBonus",true);
        }
        else
            GetComponent<SpriteRenderer>().color = gameContr.ChangeColor(hp);

        //InvokeRepeating("AnimState", 0, rtime);
    }

    private void AnimState()
    {
        anim.SetTrigger("Flash");
    }

    public void ReceiveHit()
    {
        hp--;
        if (hp > 0)
            anim.SetTrigger("Hit");
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
            if(isBonus)
                main.startColor = GetComponent<Image>().color;
            else
                main.startColor = GetComponent<SpriteRenderer>().color;

            Destroy(go, 1f);
            Destroy(gameObject, 1f);
            if (isDestroy)
            {
                GetComponentInParent<Row>().CheckNrConts();
                ScoreLEVEL.Instance.AddScoreLevel();
                ScoreLEVEL.Instance.ShowNrBlock(containerPos);
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
        hp = 1;
        ReceiveHit();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy) || coll.gameObject.CompareTag(Tags.ballSQLine) || coll.gameObject.CompareTag(Tags.LaserSq))
            ReceiveHit();
        if (coll.gameObject.CompareTag(Tags.Bonus_02))
        {
            ReciveHitByBonus();
            //GetComponentInParent<Row>().RunBonus_02();
        }
    }

}
