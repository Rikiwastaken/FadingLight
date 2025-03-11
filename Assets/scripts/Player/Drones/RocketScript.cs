using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public Transform target;
    public float speed;
    public int damage;
    public int Energydamage;
    public float basedirection;
    public float detectionrange;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target != null)
        {
            GetComponent<Rigidbody2D>().velocity = (target.position - transform.position).normalized * speed;
            Vector3 offset = target.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the target.
                                 );
            transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * basedirection, transform.localScale.y, transform.localScale.z);
            GetComponent<Rigidbody2D>().velocityX = speed * basedirection;

            Collider2D[] listcollider = Physics2D.OverlapCircleAll(transform.position, detectionrange);
            float lowestdist = detectionrange + 1;
            target = null;
            foreach (Collider2D collider in listcollider)
            {
                if ((collider.transform.tag == "enemy" || collider.transform.tag == "Boss") && Vector2.Distance(collider.transform.position, transform.position) < lowestdist)
                {
                    
                    target = collider.transform;

                    lowestdist = Vector2.Distance(collider.transform.position, transform.position);
                }
            }
        }
        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHP>())
        {
            collision.GetComponent<EnemyHP>().TakeDamage(damage, Energydamage);
            Destroy(gameObject);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            Destroy(gameObject);
        }
        
    }
}
