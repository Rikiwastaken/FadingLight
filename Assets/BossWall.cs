using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour
{
    private float scaley;
    public bool putupwall;
    public bool putdownwall;
    private int wallcounter = -1;
    // Start is called before the first frame update
    void Start()
    {
        scaley = transform.localScale.y;
        transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((putupwall||putdownwall) && wallcounter==-1)
        {
            wallcounter = (int)(0.5f / Time.deltaTime);
        }
        if(wallcounter > 0)
        {
            if(putupwall)
            {
                float newscaley = scaley * ((0.5f / Time.deltaTime) - wallcounter) / (0.5f / Time.deltaTime);
                transform.localScale = new Vector3(transform.localScale.x, newscaley, transform.localScale.z);
                wallcounter--;
                if (wallcounter == 0)
                {
                    putupwall = false;
                    wallcounter = -1;
                }
            }
            else if(putdownwall)
            {
                float newscaley = scaley * (wallcounter) / (0.5f / Time.deltaTime);
                transform.localScale = new Vector3(transform.localScale.x, newscaley, transform.localScale.z);
                wallcounter--;
                if (wallcounter == 0)
                {
                    putdownwall = false;
                    wallcounter = -1;
                    transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                }
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
                wallcounter = -1;
            }
            
        }
    }
}
