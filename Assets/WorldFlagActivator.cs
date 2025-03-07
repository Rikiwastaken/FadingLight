using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFlagActivator : MonoBehaviour
{

    private Global Global;

    public int worldflagid;

    // Start is called before the first frame update
    void Start()
    {
        Global = FindAnyObjectByType<Global>();
        if(Global.worldflags[worldflagid])
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerHP>())
        {
            Global.worldflags[worldflagid] = true;
            Destroy(gameObject);
        }
    }
}
