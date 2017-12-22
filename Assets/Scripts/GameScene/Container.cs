using UnityEngine;

public enum BlockType
{
    none = 0,
    square = 1,
    ball = 2,
    bonus = 3,
    square_01 = 4,
}

public class Container : MonoBehaviour
{
    public BlockType blockType;
    public int visualIndex;

}
