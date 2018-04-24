using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square_01Teleport : MonoBehaviour
{
    public Material portalMaterial;
    public GameObject teleportImage;

    private Transform rectTransform;

    private void Start()
    {
        rectTransform = teleportImage.GetComponent<Transform>();
        portalMaterial.color = GameController.Instance.ChangeColor(GameData.score_Rows);
    }

    private void Update()
    {
        rectTransform.Rotate(new Vector3(0,0,8) * Time.deltaTime * 20f);
    }

}
