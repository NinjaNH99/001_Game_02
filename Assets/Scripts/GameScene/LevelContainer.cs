using UnityEngine;
using TMPro;

public class LevelContainer : MonoSingleton<LevelContainer>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 83.0f; // 0.35
    private const float ANIMPOSY_SPEED = 250.0f;

    public GameObject rowPrefab;
    public GameObject containerPrefab;
    public RectTransform rowContainer;

    private float currentSpawnY;
    private Vector2 rowContainerStartingPosition;
    private Vector2 desiredPosition;

    private float lastBallSpawn;
    public int nrBlocksInGame;

    private int e;
    private bool animPosY;

    private void Awake()
    {
        animPosY = true;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200.0f);
        rowContainerStartingPosition = rowContainer.anchoredPosition;
        desiredPosition = rowContainerStartingPosition;
        lastBallSpawn = 1f;
        nrBlocksInGame = 0;
    }

    private void Start()
    {
        e = 0;
        GenerateNewRow();
        NrBlocksInGame();
    }

    private void Update()
    {
        if (animPosY)
        {
            if (rowContainer.anchoredPosition != desiredPosition)
                rowContainer.anchoredPosition = Vector2.MoveTowards(rowContainer.anchoredPosition, desiredPosition + new Vector2(0, DISTANCE_BETWEEN_BLOCKS), Time.deltaTime * ANIMPOSY_SPEED);
            if (rowContainer.anchoredPosition == desiredPosition + new Vector2(0, DISTANCE_BETWEEN_BLOCKS))
                animPosY = false;
        }
    }

    public void GenerateNewRow()
    {
        animPosY = true;
        bool doNotSpawn = false;
        //bonus_01Text.gameObject.SetActive(false);
        GameObject go = Instantiate(rowPrefab, rowContainer) as GameObject;
        go = GenerateRowBlocks(go);

        go.GetComponent<RectTransform>().localPosition = Vector2.down * currentSpawnY;

        currentSpawnY -= DISTANCE_BETWEEN_BLOCKS;

        //desiredPosition = rowContainerStartingPosition + (Vector2.up * currentSpawnY);
        desiredPosition = rowContainerStartingPosition + Vector2.up * currentSpawnY;

        Container[] blockArray = go.GetComponentsInChildren<Container>();

        int ballSpawnIndex = SpawnBall(blockArray.Length , true);
        int ballSpawnIndex2 = -1;

        if(e == 3)
        {
            ballSpawnIndex2 = SpawnBall(blockArray.Length , false);
            e = 0;
        }
        e++;

        for (int i = 0; i < blockArray.Length; i++)
        {
            if (ballSpawnIndex == i || (ballSpawnIndex2 == i && ballSpawnIndex != ballSpawnIndex2))
            {
                blockArray[i].SpawnBall();
            }
            else if (ballSpawnIndex != i && !doNotSpawn)
            {
                if (GameController.score % Random.Range(4, 6) == 0)
                {
                    blockArray[i].SpawnSquare_01();
                    doNotSpawn = true;
                }
            }
            else if (GameController.score % 2 == 0)
            {
                blockArray[i].SpawnBonus();
                break;
            }
        }
        lastBallSpawn += 0.25f;   
    }

    private int SpawnBall(int blockArray , bool oneBool)
    {
        int ballSpawnIndex = -1;
        if (oneBool)
        {
            if (lastBallSpawn * Random.Range(1.0f, 3.0f) > 1.3f)
            {
                // Force spawn a ball
                ballSpawnIndex = Random.Range(0, blockArray);
                lastBallSpawn = 1f;
            }
        }
        else if(!oneBool)
        {
            ballSpawnIndex = Random.Range(0, blockArray);
            lastBallSpawn = 1f;
        }
        return ballSpawnIndex;
    }

    private GameObject GenerateRowBlocks(GameObject row)
    {
        int[] posArray = { 5, 5, 5, 5, 5, 5, 5 };
        int pos;
        bool posIsClean = false;
        int nrBlocks = Random.Range(4, 7);
        for (int i = 0; i < nrBlocks; i++)
        {
            GameObject go = Instantiate(containerPrefab, row.transform) as GameObject;
            do
            {
                pos = Random.Range(-3, 4);
                for (int e = 0; e < posArray.Length; e++)
                {
                    if (pos != posArray[e])
                    {
                        posIsClean = true;
                    }
                    else
                    {
                        posIsClean = false;
                        break;
                    }
                }
            } while (!posIsClean);
            posArray[i] = pos;
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos * DISTANCE_BETWEEN_BLOCKS, row.GetComponent<RectTransform>().anchoredPosition.y);
        }
        nrBlocksInGame += nrBlocks;
        return row;
    }

    public void NrBlocksInGame()
    {
        if (nrBlocksInGame == 0)
        {
            Bonus.bonus_02++;
            Bonus.Instance.AddBonus_02();
        }
    }
}
