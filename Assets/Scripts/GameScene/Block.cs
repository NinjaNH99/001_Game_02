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
    private bool isDestroy;

    private void Awake()
    {
        containerPos = GetComponentInParent<Container>().gameObject.GetComponent<RectTransform>();
        gameContr = GameController.Instance;
        GetComponent<RectTransform>().localScale = new Vector2(75, 75);
    }

    private void Start()
    {
        hpText = goHpText.GetComponent<TextMeshProUGUI>();
        hp = gameContr.score_Rows * hpx2;
        hpText.text = hp.ToString();
        isDestroy = true;
        GetComponent<Image>().color = gameContr.ChangeColor(hp);
        if (imageBonus != null)
            imageBonus.GetComponent<Image>().color = GetComponent<Image>().color;
        anim = GetComponent<Animator>();
    }

    private void ReceiveHit()
    {
        hp--;
        if (hp > 0)
            anim.SetTrigger("Hit");
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (isBonus)
                GetComponentInParent<Container>().ApplySquare_Bonus();
            hpText.text = "1";
            GameObject go = Instantiate(DeathEFX, containerPos) as GameObject;

            var main = go.GetComponent<ParticleSystem>().main;
            main.startColor = GetComponent<Image>().color;

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
        if(!isBonus)
            GetComponent<Image>().color = gameContr.ChangeColor(hp);
    }

    public void ReciveHitByBonus()
    {
        hp = 1;
        ReceiveHit();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
            ReceiveHit();
        if (coll.gameObject.CompareTag(Tags.Bonus_02))
        {
            ReciveHitByBonus();
            //GetComponentInParent<Row>().RunBonus_02();
        }
    }
}
