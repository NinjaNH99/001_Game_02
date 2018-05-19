using UnityEngine;

public class Square_01Teleport : MonoBehaviour
{
    public const float speed = 20f;

    public Material portalMaterial;
    public GameObject teleportImage;

    private Transform rectTransform;

    private int rotateDir = 5;

    private void Awake()
    {
        rectTransform = teleportImage.GetComponent<Transform>();
        portalMaterial.color = GameController.Instance.ChangeColor(GameData.score_Rows);
        if (GetComponentInParent<Square_01>().type == TeleportType.Out)
            rotateDir = -5;
        else
            rotateDir = 5;
    }

    private void Update()
    {
        rectTransform.Rotate(new Vector3(0, 0, rotateDir) * Time.deltaTime * 20f);
    }

}