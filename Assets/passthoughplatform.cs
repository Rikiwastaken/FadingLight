using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class passthoughplatform : MonoBehaviour
{

    private int getdowncounter;
    
    private Rigidbody2D playerRB;

    private void FixedUpdate()
    {

        

        if(playerRB == null)
        {
            playerRB = FindAnyObjectByType<PlayerHP>().GetComponent<Rigidbody2D>();

            ChangeLayer();
        }
        else
        {
            ChangeLayer();
        }
       
    }

    private void ChangeLayer()
    {
        if (playerRB.GetComponent<PlayerMovement>().vertical == 1)
        {
            getdowncounter = (int)(0.5f / Time.fixedDeltaTime);
        }
        else if (getdowncounter>0)
        {
            getdowncounter--;
        }

        if (playerRB.transform.GetChild(0).position.y >= transform.position.y+GetComponent<BoxCollider2D>().size.y*transform.localScale.y/2f && getdowncounter==0)
        {
            gameObject.layer = LayerMask.NameToLayer("ground");
        }
        else // Si le joueur ne descend pas (ou saute par exemple)
        {
            gameObject.layer = LayerMask.NameToLayer("passthroughplatform");
        }
    }
}
