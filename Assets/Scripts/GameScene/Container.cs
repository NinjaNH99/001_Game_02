using UnityEngine;

public class Container : Row
{
    public int visualIndex;

    private BlockType[] blockTypes = new BlockType[5];

    private void Awake()
    {
        blockTypes = GetComponentsInChildren<BlockType>();
    }

    public void SpawnType(int type)
    {
        switch (type)
        {
            case 1:
                {
                    for (int i = 0; i < blockTypes.Length; i++)
                    {
                        if (blockTypes[i].Bltype == BlType.square_01)
                        {
                            blockTypes[i].Option(true);
                        }
                        else
                            blockTypes[i].Option(false);
                    }
                    break;
                }
            case 2:
                {
                    for (int i = 0; i < blockTypes.Length; i++)
                    {
                        if (blockTypes[i].Bltype == BlType.ball)
                        {
                            blockTypes[i].Option(true);
                        }
                        else
                            blockTypes[i].Option(false);
                    }
                    break;
                }
            case 3:
                {
                    for (int i = 0; i < blockTypes.Length; i++)
                    {
                        if (blockTypes[i].Bltype == BlType.bonus)
                        {
                            blockTypes[i].Option(true);
                        }
                        else
                            blockTypes[i].Option(false);
                    }
                    break;
                }
            case 4:
                {
                    for (int i = 0; i < blockTypes.Length; i++)
                    {
                        if (blockTypes[i].Bltype == BlType.square)
                        {
                            blockTypes[i].Option(true);
                        }
                        else
                            blockTypes[i].Option(false);
                    }
                    break;
                }
            case 5:
                {
                    this.gameObject.SetActive(false);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void DeSpawnBlock()
    {
        blockTypes = GetComponentsInChildren<BlockType>();
        if (blockTypes.Length == 0)
        {
            Debug.Log("Container is NULL");
            this.gameObject.SetActive(false);
        }
    }
}
