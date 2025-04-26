using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceGadget : MonoBehaviour
{

    public float rotperframe;
    public float duration;
    private int durationcounter;
    public Vector2 ForceToApply;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        durationcounter = (int)(duration/Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BulletScript>() != null)
        {
            if(collision.GetComponent<BulletScript>().damagePlayer)
            {
                Destroy(collision.gameObject);
            }
        }
        if (collision.GetComponent<EnemyHP>() != null)
        {
            Vector2 Force = ForceToApply;
            if(transform.position.x<transform.parent.position.x)
            {
                Force = new Vector2(-ForceToApply.x, ForceToApply.y);
            }
            collision.GetComponent<EnemyHP>().TakeDamage(damage, 0, Force);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.eulerAngles + new Vector3(0f, 0f, rotperframe));
        if(durationcounter > 0 )
        {
            durationcounter--;
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
