using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlType
{
    // 10 - don't add to listFC => Container
    square = 1,
    ball = 2,
    bonus = 3,
    teleportIn = 4,
    teleportOut = 5,
    square_Bonus = 6,
    square_Line = 7,
    space = 8,
    square_Boss = 9
}

public class BlockType : MonoBehaviour
{
    public BlType Bltype;

}
