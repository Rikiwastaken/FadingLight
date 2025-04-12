using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int direction; // 1 right, -1 left
    public float speed;
    public int damage;
    public int EnergyDamage;
    public bool damagePlayer;
    public Vector2 directionvector;
    private void Start()
    {
        if (directionvector != Vector2.zero)
        {
            Vector3 offset = directionvector;

            Quaternion rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the target.
                                 );
            transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            GetComponent<Rigidbody2D>().velocity = speed * directionvector;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.z);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(directionvector!=Vector2.zero)
        {
            GetComponent<Rigidbody2D>().velocity = speed*directionvector.normalized;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocityX = speed * direction;
        }
        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHP>() && !damagePlayer)
        {
            collision.GetComponent<EnemyHP>().TakeDamage(damage, EnergyDamage);
            Destroy(gameObject);
        }
        if (collision.GetComponent<PlayerHP>() && damagePlayer)
        {
            collision.GetComponent<PlayerHP>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            Destroy(gameObject);
        }

    }
}
