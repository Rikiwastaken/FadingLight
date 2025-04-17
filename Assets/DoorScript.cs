using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int flagID;
    private Global global;

    private void Start()
    {
        global = FindAnyObjectByType<Global>();
    }

    private void FixedUpdate()
    {
        if (global.worldflags[flagID])
        {
            GetComponent<Animator>().SetBool("Open",true);
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else 
        {
            GetComponent<Animator>().SetBool("Open", false);
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
