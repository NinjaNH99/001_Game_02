using UnityEngine;

public class Container : MonoBehaviour
{
    public int visualIndex;
    public int rowID;

    public GameObject[] blockTypes = new GameObject[8];

    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = LevelManager.Instance;
    }

    public void SpawnType(BlType type , bool applayBonus = false, bool addInListFC = true)
    {
        if (type == BlType.space && addInListFC)
        {
            AddInListFreeConts();
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
                }
                else if (applayBonus && type == BlType.bonus)
                {
                    go.GetComponent<CollectBonus>().isByBonus = true;
                }
            }
        }
        
    }

    public void AddInListFreeConts()
    {
        GetComponentInParent<Row>().rowMap[visualIndex] = 8;
        levelManager.listFreeConts.Add(this);
    }

    public void RemoveContInLFC()
    {
        //if (levelManager.listFreeConts.Contains(this))
            levelManager.listFreeConts.Remove(this);
    }

    public void DeSpawnBlock()
    {
        RemoveContInLFC();
        GetComponentInParent<Row>().evDeSpawnContainer -= DeSpawnBlock;
        if (visualIndex == 8)
        {
            EventManager.StartEvSpawn();
        }
    }

    public void ApplySquare_Bonus()
    {
        BlType blType = BlType.ball;

        int r = Random.Range(0, 100);
        if (r >= 60)
        {
            blType = BlType.ball;
            GetComponentInParent<Row>().rowMap[visualIndex] = 2;
        }
        else if (r >= 20)
        {
            blType = BlType.bonus;
            GetComponentInParent<Row>().rowMap[visualIndex] = 3;
        }
        else if (r >= 10)
        {
            blType = BlType.square;
            GetComponentInParent<Row>().rowMap[visualIndex] = 1;
        }

        RemoveContInLFC();
        SpawnType(blType, true);
    }

}
