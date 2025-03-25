using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float timebeforeexplosion;
    public int damage;
    public int energydamage;
    public int explosioncounter;
    private List<Transform> damagedobject = new List<Transform>();
    public float explosionduration;
    public int explosiondurationcounter=-1;
    public Vector2 ForcetoApply;
    private bool initialized;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(explosioncounter == 0)
        {
            if(collision.GetComponent<EnemyHP>() != null && !damagedobject.Contains(collision.transform))
            {
                damagedobject.Add(collision.transform);
                int direction = 0;
                if(transform.position.x>collision.transform.position.x)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }
                Vector2 realforce = new Vector2(ForcetoApply.x* direction, ForcetoApply.y);
                collision.GetComponent<EnemyHP>().TakeDamage(damage,energydamage, realforce);
            }
            //if(collision.GetComponent<PlayerHP>() != null && !damagedobject.Contains(collision.transform))
            //{
            //    damagedobject.Add(collision.transform);
            //    int direction = 0;
            //    if (transform.position.x > collision.transform.position.x)
            //    {
            //        direction = -1;
            //    }
            //    else
            //    {
            //        direction = 1;
            //    }
            //    Vector2 realforce = new Vector2(ForcetoApply.x * direction, ForcetoApply.y);
            //    collision.GetComponent<PlayerHP>().TakeDamage(damage,Vector2.zero, realforce, energydamage);
            //}
        }
    }

    private void FixedUpdate()
    {

        if(!initialized)
        {
            explosioncounter = (int)(timebeforeexplosion / Time.deltaTime);
            initialized = true;
        }

        if(explosiondurationcounter==0 && explosioncounter==0)
        {
            Destroy(gameObject);
        }

        if(explosioncounter>0)
        {
            explosioncounter--;
            
        }
        if(explosioncounter==0 && explosiondurationcounter==-1)
        {
            explosiondurationcounter = (int)(explosionduration / Time.deltaTime);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            
        }
        
        if(explosiondurationcounter>0)
        {
            explosiondurationcounter--;
        }

    }

}
