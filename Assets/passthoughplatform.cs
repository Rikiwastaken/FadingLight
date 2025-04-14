using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        while(collision.collider == playerRB.GetComponent<CircleCollider2D>() && collision.contacts[0].point.y>= playerRB.transform.position.x+playerRB.GetComponent<BoxCollider2D>().size.y / 2f)
        {
            playerRB.transform.position += new Vector3(0, 0.1f,0);
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

        if (playerRB.velocity.y <= 0 && getdowncounter==0) // Si le joueur tombe (ou descend)
        {
            // Activer les collisions entre le joueur et la plateforme
            Physics2D.IgnoreLayerCollision(9, 12, false); // Collision activée
        }
        else // Si le joueur ne descend pas (ou saute par exemple)
        {
            // Exclure les collisions entre le joueur et la plateforme
            Physics2D.IgnoreLayerCollision(9, 12, true); // Collision désactivée
        }
    }
}
