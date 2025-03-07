using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int direction; // 1 right, -1 left
    public float speed;
    public int damage;
    public int EnergyDamage;

    private void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x*direction, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocityX = speed * direction;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHP>())
        {
            collision.GetComponent<EnemyHP>().TakeDamage(damage, EnergyDamage);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            Destroy(gameObject);
        }

    }
}
