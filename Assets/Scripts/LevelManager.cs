using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoSingleton<LevelManager>
{
    private const float DISTANCE_BETWEEN_BLOCKS = 84.0f; // 83.0
    private const float ANIMPOSY_SPEED = 250.0f;

    public GameObject rowPrefab;
    // List of rows
    protected List<GameObject> rows = new List<GameObject>();

    private void Start()
    {
        GenerateRow();
    }

    public void GenerateRow()
    {
        GameObject go_row = Instantiate(rowPrefab, this.transform) as GameObject;
        rows.Add(go_row);
    }

    /*
    public Container GetContainer(BlockType bt, int visualIndex)
    {
        Container cont = containers.Find(x => x.blockType == bt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(cont == null)
        {
            GameObject go = null;

            go = Instantiate(go);
            cont = go.GetComponent<Container>();
            containers.Add(cont);
        }
        return cont;
    }*/

}
