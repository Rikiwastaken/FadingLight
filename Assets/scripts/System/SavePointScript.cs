using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePointScript : MonoBehaviour
{
    public GameObject SaveMenu;
    private bool launchfadetoblack;
    private Image fadetoblack;

    public float fadeduration;

    public float savepointCD;
    private int savepointcdcounter;

    private int fadecounter;
    private Vector2 wheretoplaceplayer;

    // Start is called before the first frame update
    void Start()
    {
        fadetoblack = GameObject.Find("FadeToBlackImage").GetComponent<Image>();
        wheretoplaceplayer=transform.GetChild(0).transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            FindAnyObjectByType<PlayerMovement>().transform.position = wheretoplaceplayer;
        }
        else
        {
            if(savepointcdcounter>0)
            {
                savepointcdcounter--;
            }

        }

        if (launchfadetoblack)
        {
            if(fadetoblack.color.a<1)
            {
                fadecounter += 1;
                if(fadecounter / (fadeduration / Time.deltaTime)>1f)
                {
                    fadetoblack.color = new Color(1f, 1f, 1f, 1f);
                    launchfadetoblack = false;
                    
                }
                else
                {
                    fadetoblack.color = new Color(1f, 1f, 1f, fadecounter / (fadeduration / Time.deltaTime));
                }
            }
            if(fadetoblack.color.a==1)
            {
                OpenSavePointFct();
                fadecounter = -(int)(fadeduration / Time.deltaTime)/2;
                fadetoblack.color = new Color(1f, 1f, 1f, ((fadeduration / Time.deltaTime)-fadecounter) / (fadeduration / Time.deltaTime));
            }
        }
        else
        {
            if (fadetoblack.color.a >0)
            {
                fadecounter +=1;
                fadetoblack.color = new Color(1f, 1f, 1f, ((fadeduration / Time.deltaTime) - fadecounter) / (fadeduration / Time.deltaTime));
            }
            else
            {
                fadecounter = 0;
                fadetoblack.color = new Color(1f, 1f, 1f, 0);
            }

        }
    }


    void OpenSavePointFct()
    {
        if(savepointcdcounter<=0)
        {
            FindAnyObjectByType<PlayerMovement>().safezone();
            FindAnyObjectByType<PlayerMovement>().transform.position = wheretoplaceplayer;
            GameObject newmenu = Instantiate(SaveMenu, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
            newmenu.transform.SetAsFirstSibling();
            newmenu.GetComponent<RectTransform>().localPosition = Vector2.zero;
            FindAnyObjectByType<Global>().atsavepoint = true;
            savepointcdcounter = (int)(savepointCD / Time.deltaTime);
        }
        
    }

    public void InteractWithSavePoint()
    {
        if (savepointcdcounter<=0)
        {
            launchfadetoblack = true;
        }
    }
}
