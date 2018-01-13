using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block : MonoSingleton<Block>
{
    public GameObject goHpText;
    public GameObject DeathEFX;
    public RectTransform containerPos;
    public int hp , hpx2 = 1;

    private TextMeshProUGUI hpText;
    private Animator anim;
    private bool isDestroy;

    private void Awake()
    {
        GetComponent<RectTransform>().localScale = new Vector2(75, 75);
    }

    private void Start()
    {
        hpText = goHpText.GetComponent<TextMeshProUGUI>();
        hp = GameController.score * hpx2;
        hpText.text = hp.ToString();
        isDestroy = true;
        GetComponent<Image>().color = GameController.Instance.ChangeColor(hp);
        anim = GetComponent<Animator>();
    }

    private void ReceiveHit()
    {
        anim.SetTrigger("Hit");
        hp--;
        if (hp <= 0)
        {
            hpText.text = "1";
            GameObject go = Instantiate(DeathEFX, containerPos) as GameObject;
            gameObject.SetActive(false);

            Destroy(go, 1f);
            Destroy(gameObject, 1f);
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (isDestroy)
            {
                GetComponentInParent<Row>().CheckNrConts();
                isDestroy = false;
            }
            return;
        }
        hpText.text = hp.ToString();
        GetComponent<Image>().color = GameController.Instance.ChangeColor(hp);
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
