using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{

    public bool deactivatevisuals;

    // Start is called before the first frame update
    void Start()
    {
        if(deactivatevisuals)
        {
            foreach (SpriteRenderer SR in GetComponentsInChildren<SpriteRenderer>())
            {
                SR.enabled = false;
            }
        }
    }
}
