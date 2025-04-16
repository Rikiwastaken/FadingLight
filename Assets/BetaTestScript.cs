using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BetaTestScript : MonoBehaviour
{

    private bool Npressed;
    private bool Bpressed;

    private int activeindex;

    public GameObject[] EnemyList;
    public Sprite[] SpriteList;


    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameObject.Find("TestImageToSpawn"))
        {
            GameObject.Find("TestImageToSpawn").GetComponent<Image>().sprite = SpriteList[activeindex];
        }

        if (Input.GetKeyDown(KeyCode.N) && !Npressed)
        {
            int direction = 1;
            if(GetComponent<SpriteRenderer>().flipX)
            {
                direction = -1;
            }
            Npressed = true;
            Instantiate(EnemyList[activeindex], transform.position + new Vector3(5f,0f,0f)*direction,Quaternion.identity);
        }

        if(!Input.GetKeyDown(KeyCode.N))
        {
            Npressed=false;
        }

        if (Input.GetKeyDown(KeyCode.B) && !Bpressed)
        {
            Bpressed = true;
            if(activeindex<EnemyList.Length-1)
            {
                activeindex++;
            }
            else
            {
                activeindex = 0;
            }
        }

        if (!Input.GetKeyDown(KeyCode.B))
        {
            Bpressed = false;
        }
    }
}
