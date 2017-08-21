using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRow : MonoBehaviour {

    public void Update()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

}
