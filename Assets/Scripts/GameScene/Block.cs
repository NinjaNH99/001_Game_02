using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block : MonoSingleton<Block>
{
    public GameObject goHpText;
    public GameObject DeathEFX;
    public RectTransform containerPos;
    public int hp;

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
        /*if (GetComponentInParent<Row>().nrBlock2HP > 0)
        {
            hp = GameController.score * 2;
            GetComponentInParent<Row>().nrBlock2HP--;
        }
        else
            hp = GameController.score;*/
        hp = GameController.score;
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

            //Debug.Log(posInBlock);

            Destroy(go, 1f);
            Destroy(gameObject, 1f);
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (isDestroy)
            {
                GetComponentInParent<Row>().CheckNrConts();
                //LevelContainer.Instance.nrBlocksInGame--;
                //LevelContainer.Instance.NrBlocksInGame();
                isDestroy = false;
            }
            return;
        }
        hpText.text = hp.ToString();
        GetComponent<Image>().color = GameController.Instance.ChangeColor(hp);
    }

    public void ReciveHitByBonus(int nr)
    {
        switch(nr)
        {
            case 1:
                {
                    hp /= 2;
                    hp++;
                    ReceiveHit();
                    break;
                }
            case 0:
                {
                    hp = 1;
                    ReceiveHit();
                    break;
                }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
            ReceiveHit();
        if (coll.gameObject.CompareTag(Tags.Bonus_02))
        {
            //GetComponentInParent<Container>().RunBonus_02();
            ReciveHitByBonus(0);
        }
    }
}
