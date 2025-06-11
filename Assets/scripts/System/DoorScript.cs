using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int flagID;
    private Global global;
    public bool isdoor;

    private void Start()
    {
        global = FindAnyObjectByType<Global>();
    }

    private void FixedUpdate()
    {
        if (global.worldflags[flagID])
        {
            if(isdoor)
            {
                GetComponent<Animator>().SetBool("Open", true);
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else 
        {
            if (isdoor)
            {
                GetComponent<Animator>().SetBool("Open", false);
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
