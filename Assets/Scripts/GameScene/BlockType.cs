using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlType
{
    square = 1,
    ball = 2,
    bonus = 3,
    square_01 = 4,
}

public class BlockType : MonoBehaviour
{
    public BlType Bltype;

    public void Option(bool spawn)
    {
        if(spawn)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

}
