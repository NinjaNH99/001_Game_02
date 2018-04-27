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
        GetComponentInParent<Row>().evDeSpawnContainer += DeSpawnBlock;
        //rowID = GetComponentInParent<Row>().rowID;
    }

    public void SpawnType(BlType type , bool applayBonus = false, bool addInListFC = true)
    {
        rowID = GetComponentInParent<Row>().rowID;
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
        levelManager.listFreeConts.Add(this);
    }

    public void RemoveContInLFC()
    {
        if(levelManager.listFreeConts.Contains(this))
            levelManager.listFreeConts.Remove(this);
    }

    public void DeSpawnBlock()
    {
        RemoveContInLFC();
        GetComponentInParent<Row>().evDeSpawnContainer -= DeSpawnBlock;
        if (visualIndex == 8)
        {
            //Debug.Log("DeSpawn last block");
            EventManager.StartEvSpawn();
        }
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

        RemoveContInLFC();
        SpawnType(blType, true);
    }

}
