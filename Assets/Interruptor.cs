using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptor : MonoBehaviour
{
    public Sprite InterruptorActivated;
    public Sprite InterruptorNotActivated;
    public int worldflagconcerned;
    public bool canonlybeactivatedOnce;
    private Global global;

    private void Start()
    {
        global = FindAnyObjectByType<Global>();
    }
    private void FixedUpdate()
    {
        if (global.worldflags[worldflagconcerned])
        {
            GetComponent<SpriteRenderer>().sprite = InterruptorActivated;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = InterruptorNotActivated;
        }
    }

    public void InterractWithInterruptor()
    {
        if(!global.worldflags[worldflagconcerned])
        {
            global.worldflags[worldflagconcerned] = true;
        }
        else if(!canonlybeactivatedOnce)
        {
            global.worldflags[worldflagconcerned] =false;
        }
    }
}
