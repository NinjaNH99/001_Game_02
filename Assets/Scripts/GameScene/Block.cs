using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block : MonoSingleton<Block>
{
    public GameObject goHpText;
    public GameObject DeathEFX;
    public RectTransform containerPos;
    public int hp;

    private TextMeshPro hpText;
    private Color image;
    private bool isDestroy;

    private void Start()
    {
        hpText = goHpText.GetComponent<TextMeshPro>();
        if (GameController.score % Random.Range(2, 4) == 0 && Random.Range(0f, 1f) > 0.3f)
            hp = GameController.score * 2;
        else
            hp = GameController.score;
        hpText.text = hp.ToString();
        isDestroy = true;
        image = GetComponent<Image>().color;
        image = GameController.Instance.ChangeColor(hp);
    }

    private void ReceiveHit()
    {
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
                LevelContainer.Instance.nrBlocksInGame--;
                LevelContainer.Instance.NrBlocksInGame();
                isDestroy = false;
            }
            return;
        }
        hpText.text = hp.ToString();
        image = GameController.Instance.ChangeColor(hp);
        GetComponent<Image>().color = image;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Player) || coll.gameObject.CompareTag(Tags.ballCopy))
            ReceiveHit();
    }
}
