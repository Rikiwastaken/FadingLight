using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public Transform target;
    public float speed;
    public int damage;

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = (target.position-transform.position).normalized*speed;
        transform.forward = (target.position - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="ennemy")
        {
            if(collision.GetComponent<EnemyHP>())
            {
                collision.GetComponent<EnemyHP>().TakeDamage(damage);
            }
        }
    }
}
