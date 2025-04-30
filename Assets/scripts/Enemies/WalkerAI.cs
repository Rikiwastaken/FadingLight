using System.Collections.Generic;
using UnityEngine;

public class WalkerAI : MonoBehaviour
{
    private GameObject player;
    public float timeunabletomove;
    public int timeunabletomovecounter;
    public GameObject Exclamationmark;
    public float movespeed;

    private float destination;

    private int lasthp;

    public float mindist;
    public float damage;

    public Vector2 pushforce;

    public float AttackCD;
    private int AttackCDCounter;

    private bool targetting;

    public float mindistforATK;
    private bool hittingleft;

    private List<GameObject> hits = new List<GameObject>();

    private void OnTriggerStay2D(Collider2D collision)
    {

        bool rightdirection = (transform.position.x < collision.transform.position.x && !GetComponent<SpriteRenderer>().flipX) || (transform.position.x > collision.transform.position.x && !GetComponent<SpriteRenderer>().flipX);

        if (((hittingleft && transform.position.x<collision.transform.position.x)|| (!hittingleft && transform.position.x > collision.transform.position.x) || hits.Contains(collision.gameObject))&& rightdirection)
        {
            return;
        }
        if(collision.gameObject.GetComponent<PlayerHP>() != null)
        {
            
            Vector2 force = pushforce;
            float newdest = transform.position.x - 3f;
            if (collision.gameObject.transform.position.x<transform.position.x)
            {
                force.x *= -1;
                newdest = transform.position.x + 3f;
            }
            collision.gameObject.GetComponent<PlayerHP>().TakeDamage(damage,Vector2.zero,force);
            destination = newdest;
            hits.Add(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }
        if (collision.transform.GetComponent<PlayerHP>() != null && (transform.position.y - GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2f >= collision.transform.position.y + collision.transform.GetComponent<BoxCollider2D>().size.y * collision.transform.transform.localScale.y / 2f))
        {
            int direction = (int)((collision.transform.position.x - transform.position.x) / Mathf.Abs(collision.transform.position.x - transform.position.x));
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 5, 3), ForceMode2D.Impulse);
        }
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerHP>().gameObject;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }

        if(Vector2.Distance(transform.position,player.transform.position)<=mindist)
        {
            targetting=true;
        }
        else
        {
            targetting = false;
        }
        GetComponent<EyeScript>().activateeyes = targetting;

        Exclamationmark.SetActive(false);
        if (GetComponent<EnemyHP>().enemyNRG > 0)
        {
            ManageMovement();
            managehitstun();
            manageAttack();
        }



    }


    private void manageAttack()
    {

        if(AttackCDCounter==0 && Vector2.Distance(transform.position, player.transform.position) <= mindistforATK && timeunabletomovecounter == 0 || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("WalkerAttack"))
        {
            Exclamationmark.SetActive(true);
        }
        else
        {
            Exclamationmark.SetActive(false);
        }

        if(AttackCDCounter>0)
        {
            AttackCDCounter--;
        }
        else
        {

            bool facingrightdirection = (GetComponent<SpriteRenderer>().flipX && player.transform.position.x < transform.position.x) || (!GetComponent<SpriteRenderer>().flipX && player.transform.position.x > transform.position.x);
            if (targetting && Vector2.Distance(transform.position,player.transform.position)<=mindistforATK && timeunabletomovecounter==0 && facingrightdirection)
            {
                AttackCDCounter = (int)(AttackCD/Time.fixedDeltaTime);
                hits = new List<GameObject>();
                GetComponent<Animator>().SetTrigger("Attack");
                hittingleft = GetComponent<SpriteRenderer>().flipX;
            }
        }
    }

    private void managehitstun()
    {

        if (timeunabletomovecounter > 0)
        {
            timeunabletomovecounter--;
        }

        if (lasthp > GetComponent<EnemyHP>().enemyhp && timeunabletomovecounter == 0)
        {
            timeunabletomovecounter = (int)(timeunabletomove / Time.deltaTime);
        }


        lasthp = GetComponent<EnemyHP>().enemyhp;
    }

    private void ManageMovement()
    {
        if(timeunabletomovecounter > 0)
        {
            return;
        }
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("WalkerAttack"))
        {
            GetComponent<Rigidbody2D>().velocityX =0;
            return;
        }
        if (destination != 0 && timeunabletomovecounter == 0)
        {
            GetComponent<Rigidbody2D>().velocityX = (destination - transform.position.x) / Mathf.Abs(destination - transform.position.x) * movespeed;
            if (targetting)
            {
                GetComponent<Rigidbody2D>().velocityX *= 2;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocityX = GetComponent<Rigidbody2D>().velocityX * 0.9991f;
        }
        if(targetting && AttackCDCounter==0)
        {
            destination = player.transform.position.x;
        }
        if (Mathf.Abs(destination - transform.position.x) <= 1 || destination == 0)
        {
            if (Random.Range(0, 60) == 25)
            {
                destination = transform.position.x + Random.Range(-5f, 5f);
            }
            else
            {
                destination = 0;
            }
        }
        if (GetComponent<Rigidbody2D>().velocityX < -0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetComponent<Rigidbody2D>().velocityX > 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocityX));
    }
}
