using UnityEngine;

public class Container : Row
{
    public int visualIndex;
    public bool goNext;

    private BlockType[] blockTypes;

    private void Awake()
    {
        goNext = false;
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
                            goNext = true;
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
                            goNext = true;
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
                            goNext = true;
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
                            goNext = true;
                        }
                        else
                            blockTypes[i].Option(false);
                    }
                    break;
                }
            default:
                {
                    this.gameObject.SetActive(false);
                    break;
                }
        }
    }

    private void DeSpawnBlock()
    {

    }
}
