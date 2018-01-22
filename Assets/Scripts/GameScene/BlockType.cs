using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlType
{
    square = 1,
    ball = 2,
    bonus = 3,
    square_01 = 4,
    square_Bonus = 5,
    space = 6
}

public class BlockType : MonoBehaviour
{
    public BlType Bltype;

}
