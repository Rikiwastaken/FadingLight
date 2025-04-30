using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingRejectAI : MonoBehaviour
{

    private bool touchingwall;

    public float range;

    private Transform player;

    public float speed;

    public float damage;

    public Vector2 pushstr;

    public float gravitymultiplier;

    private Vector3 Destination;

    private int jumpCD;

    public float JumpCDTime;

    private Vector3 LastDestination;

    private int temphp;

    private void OnCollisionStay2D(Collision2D collision)
    {

        if ((collision.gameObject.layer == LayerMask.NameToLayer("wall") || collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("roof")) && collision.gameObject.transform.tag!="passthroughplatform")
        {
            touchingwall = true;
        }
        if (collision.transform.GetComponent<PlayerHP>() != null && (transform.position.y - GetComponent<CircleCollider2D>().radius * transform.localScale.y / 2f >= collision.transform.position.y + GetComponent<CircleCollider2D>().radius * collision.transform.transform.localScale.y / 2f))
        {
            int direction = (int)((collision.transform.position.x - transform.position.x) / Mathf.Abs(collision.transform.position.x - transform.position.x));
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 5, 3), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("wall") || collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("roof")) && collision.gameObject.transform.tag!="passthroughplatform")
        {
            touchingwall = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("wall") || collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("roof")) && collision.gameObject.transform.tag!="passthroughplatform")
        {
            touchingwall = true;
            if(Destination != Vector3.zero)
            {
                Destination = Vector3.zero;
                jumpCD = (int)(JumpCDTime / Time.fixedDeltaTime);

            }
        }

        if(collision.transform.GetComponent<PlayerHP>() != null && Destination!=Vector3.zero)
        {
            Vector2 howtopush = Vector2.zero;
            howtopush.y = pushstr.y;
            howtopush.x = (collision.transform.position.x- transform.position.x)/Mathf.Abs(collision.transform.position.x - transform.position.x)*pushstr.x;
            collision.transform.GetComponent<PlayerHP>().TakeDamage(damage, Vector2.zero, howtopush);
            Destination = -Destination;
        }

        if (collision.transform.GetComponent<EnemyHP>() != null && Destination != Vector3.zero)
        {
            Destination = -Destination;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerHP>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(touchingwall)
        {
            if (FindAnyObjectByType<Global>().grappling && GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {
                return;
            }
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            if(Vector3.Distance(player.position,transform.position) <= range && jumpCD<=0)
            {
                Destination = (player.position - transform.position).normalized;
            }
            else
            {
                Destination = Vector3.zero;
                jumpCD--;
            }
        }
        else
        {
            if (Destination == Vector3.zero)
            {
                GetComponent<Rigidbody2D>().AddForce(Physics2D.gravity * gravitymultiplier, ForceMode2D.Force);

            }
        }

        if(Destination != Vector3.zero)
        {
            if (!(FindAnyObjectByType<Global>().grappling && GetComponent<Rigidbody2D>().velocity != (Vector2)Destination * speed))
            {
                GetComponent<Rigidbody2D>().velocity = Destination * speed;
            }
            else
            {
                Destination = Vector3.zero;
            }
            
        }

        if(LastDestination!=Destination && Destination ==Vector3.zero)
        {
            jumpCD = (int)(JumpCDTime / Time.fixedDeltaTime);
        }
        LastDestination = Destination;

        if(temphp!=GetComponent<EnemyHP>().enemyhp && Destination != Vector3.zero)
        {
            Destination=-Destination;
        }
        temphp = GetComponent<EnemyHP>().enemyhp;


        if(Destination == Vector3.zero)
        {
            GetComponent<EyeScript>().activateeyes = false;
        }
        else
        {
            GetComponent<EyeScript>().activateeyes = true;
        }

    }

    

}
