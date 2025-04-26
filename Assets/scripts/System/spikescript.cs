using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikescript : MonoBehaviour
{

    public int damage;
    public Vector2 pushstr;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="Player")
        {
            collision.transform.GetComponent<PlayerHP>().TakeDamage(damage,Vector2.zero,pushstr);
        }
        if(collision.GetComponent<EnemyHP>())
        {
            collision.transform.GetComponent<EnemyHP>().TakeDamage(damage,0, pushstr);
        }
    }
}
