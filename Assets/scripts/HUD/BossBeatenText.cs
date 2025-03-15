using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBeatenText : MonoBehaviour
{
    public Transform bottomhalf;
    public Transform tophalf;

    private int posx;
    public int offset;
    private bool gotomiddle;
    private bool gotosides;
    public float titleduration;
    private int titledurationcnt;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        posx= (int)bottomhalf.position.x;
        bottomhalf.position += new Vector3(offset, 0f, 0f);
        tophalf.position -= new Vector3(offset, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {



        if(gotomiddle)
        {
            GotoMiddleFct();
        }
        if(gotosides)
        {
            GotoSidesFct();
        }

        if(!gotomiddle && titledurationcnt > 0)
        {
            titledurationcnt--;
            if(titledurationcnt==0)
            {
                gotosides = true;
            }
        }
    }
    void GotoMiddleFct()
    {
        if (bottomhalf.transform.position.x > posx)
        {
            bottomhalf.transform.position -= new Vector3(speed, 0f, 0f);
        }
        else
        {
            bottomhalf.transform.position = new Vector3(posx, bottomhalf.transform.position.y, bottomhalf.transform.position.z);
        }

        if (tophalf.transform.position.x < posx)
        {
            tophalf.transform.position += new Vector3(speed, 0f, 0f);
        }
        else
        {
            tophalf.transform.position = new Vector3(posx, bottomhalf.transform.position.y, bottomhalf.transform.position.z);
        }

        if (tophalf.transform.position.x == posx && bottomhalf.transform.position.x == posx)
        {
            gotomiddle = false;
        }
    }

    void GotoSidesFct()
    {
        if (bottomhalf.transform.position.x < posx+offset)
        {
            bottomhalf.transform.position += new Vector3(speed*1.5f, 0f, 0f);
        }
        else
        {
            bottomhalf.transform.position = new Vector3(posx+offset, bottomhalf.transform.position.y, bottomhalf.transform.position.z);
        }

        if (tophalf.transform.position.x > posx - offset)
        {
            tophalf.transform.position -= new Vector3(speed*1.5f, 0f, 0f);
        }
        else
        {
            tophalf.transform.position = new Vector3(posx - offset, bottomhalf.transform.position.y, bottomhalf.transform.position.z);
        }

        if (tophalf.transform.position.x == posx-offset && bottomhalf.transform.position.x == posx + offset)
        {
            gotosides = false;
        }
    }

    public void StartTitle()
    {
        titledurationcnt = (int)(titleduration/Time.deltaTime);
        gotomiddle=true;
    }
}


