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
        Vector3 offset = target.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(
                               Vector3.forward, // Keep z+ pointing straight into the screen.
                               offset           // Point y+ toward the target.
                             );
        transform.rotation = rotation * Quaternion.Euler(0, 0, 90);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHP>())
        {
            collision.GetComponent<EnemyHP>().TakeDamage(damage);
            Destroy(gameObject);
        }
        
    }
}
