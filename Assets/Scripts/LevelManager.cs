using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoSingleton<LevelManager>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 84.0f; // 83.0
    private const float ANIMPOSY_SPEED = 250.0f;

    // List of containers
    public List<Container> squares = new List<Container>();
    public List<Container> balls = new List<Container>();
    public List<Container> bonus = new List<Container>();
    public List<Container> square_01s = new List<Container>();
    public List<Container> containers = new List<Container>();

    // List of rows
    public List<Row> rows = new List<Row>();

    public bool startSpawn = false;


    private void Start()
    {
        GenerateRow();
    }

    private void GenerateRow()
    {
        SpawnRow();
    }

    private void SpawnRow()
    {

    }

    public Container GetContainer(BlockType bt, int visualIndex)
    {
        Container cont = containers.Find(x => x.blockType == bt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(cont == null)
        {
            GameObject go = null;

            if (bt == BlockType.square)
                go = squares[visualIndex].gameObject;
            else if (bt == BlockType.ball)
                go = balls[visualIndex].gameObject;
            else if (bt == BlockType.bonus)
                go = bonus[visualIndex].gameObject;
            else if (bt == BlockType.square_01)
                go = square_01s[visualIndex].gameObject;

            go = Instantiate(go);
            cont = go.GetComponent<Container>();
            containers.Add(cont);
        }

        return cont;
    }

}
