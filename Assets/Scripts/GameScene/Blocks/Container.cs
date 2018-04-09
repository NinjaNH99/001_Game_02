using UnityEngine;

public class Container : MonoBehaviour
{
    public int visualIndex;

    public GameObject[] blockTypes = new GameObject[6];

    private void Start()
    {
        GetComponentInParent<Row>().evDeSpawnContainer += DeSpawnBlock;
    }

    public void SpawnType(BlType type , bool applayBonus = false, bool addInListFC = true)
    {
        if (type == BlType.space && addInListFC)
        {
            LevelManager.Instance.listFreeConts.Add(this.gameObject);
            return;
        }

        for (int i = 0; i < blockTypes.Length; i++)
        {
            if (blockTypes[i].GetComponent<BlockType>().Bltype == type && type != BlType.space)
            {
                GameObject go = Instantiate(blockTypes[i], transform) as GameObject;

                if (applayBonus && type == BlType.ball)
                {
                    go.GetComponent<CollectBall>().isByBonus = true;
                    LevelManager.Instance.listFreeConts.Remove(this.gameObject);
                }
                else if (applayBonus && type == BlType.bonus)
                {
                    go.GetComponent<CollectBonus>().isByBonus = true;
                    LevelManager.Instance.listFreeConts.Remove(this.gameObject);
                }
            }
        }
        
        //DeSpawnBlock();
    }
    /*
    public bool DeSpawnBlock()
    {
        var nrBlockType = GetComponentsInChildren<BlockType>().Length;
        if (nrBlockType <= 0)
        {
            //GetComponentInParent<Row>().nrBlock--;
            gameObject.SetActive(false);
            LevelManager.Instance.listFreeConts.Remove(this.gameObject);
        }
        return true;
    }*/

    public void DeSpawnBlock()
    {
        LevelManager.Instance.listFreeConts.Remove(this.gameObject);
    }

    public void ApplySquare_Bonus()
    {
        BlType blType = BlType.ball;

        int r = Random.Range(0, 100);
        if (r >= 60)
            blType = BlType.ball;
        else if (r >= 20)
            blType = BlType.bonus;
        else if (r >= 10)
            blType = BlType.square;

        SpawnType(blType, true);
    }

    public void EndLevel(GameObject obj)
    {
        if (obj.tag == Tags.Square || obj.tag == Tags.Block_Boss)
        {
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        else if (obj.tag == Tags.Square_01)
        {
            Destroy(obj);
        }
    }
}
