using UnityEngine;

public class Container : MonoBehaviour
{
    public int visualIndex;

    public GameObject[] blockTypes = new GameObject[5];

    public void SpawnType(BlType type , bool applayBonus = false)
    {
        for (int i = 0; i < blockTypes.Length; i++)
        {
            if (blockTypes[i].GetComponent<BlockType>().Bltype == type && type != BlType.space)
            {
                GameObject go = Instantiate(blockTypes[i], transform) as GameObject;

                if (applayBonus && type == BlType.ball)
                    go.GetComponent<CollectBall>().isByBonus = true;
                else if(applayBonus && type == BlType.bonus)
                    go.GetComponent<CollectBonus>().isByBonus = true;
                else if(applayBonus && type == BlType.square_01)
                    LevelManager.Instance.LSQ1MAX--;

                GetComponentInParent<Row>().nrBlock++;
            }
        }
        DeSpawnBlock();
    }

    public bool DeSpawnBlock()
    {
        var nrBlockType = GetComponentsInChildren<BlockType>().Length;
        if (nrBlockType <= 0)
        {
            GetComponentInParent<Row>().nrBlock--;
            gameObject.SetActive(false);
        }
        return true;
    }

    public void ApplySquare_Bonus()
    {
        BlType blType = BlType.ball;

        var r = Random.Range(0, 100);
        if (r >= 60)
            blType = BlType.ball;
        else if (r >= 20)
            blType = BlType.bonus;
        else if(r >= 10)
            blType = BlType.square_01;

        SpawnType(blType, true);
    }

    public void EndLevel(GameObject obj)
    {
        if (obj.tag == Tags.Square)
        {
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        else if (obj.tag == Tags.Square_01)
        {
            Destroy(obj);
        }
    }
}
