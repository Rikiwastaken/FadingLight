using System.Collections.Generic;
using UnityEngine;

public class ContaminatedAI : MonoBehaviour
{
    public float damage;
    public Vector3 PushForce;
    private GameObject player;
    private float mindist;

    private int lasthp;
    public float timeunabletomove;
    public int timeunabletomovecounter;
    public GameObject Exclamationmark;
    public float movespeed;

    private float destination;
    private List<GameObject> hitlist = new List<GameObject>();

    private int attackCDcounter;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerHP>() != null)
        {
            if(((transform.position.x>collision.transform.position.x && GetComponent<SpriteRenderer>().flipX)|| (transform.position.x < collision.transform.position.x && !GetComponent<SpriteRenderer>().flipX)) && !hitlist.Contains(collision.gameObject))
            {

                Vector2 forcetoapply = PushForce;
                if (collision.transform.position.x < transform.position.x)
                {
                    forcetoapply.x = -PushForce.x;
                }
                hitlist.Add(collision.gameObject);
                collision.GetComponent<PlayerHP>().TakeDamage(damage, Vector2.zero, forcetoapply);
            }
            
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
        mindist = transform.localScale.x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ContaminatedAttack"))
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
            Exclamationmark.SetActive(true);
        }
        else
        {
            attackCDcounter--;
            hitlist = new List<GameObject>();
            GetComponent<CapsuleCollider2D>().enabled = false;
            Exclamationmark.SetActive(false);
            if (GetComponent<EnemyHP>().enemyNRG > 0)
            {
                ManageMovement();
            }    
        }

        if (GetComponent<EnemyHP>().enemyNRG > 0)
        {
            ManageAttack();

            managehitstun();
        }



    }

    private void ManageAttack()
    {
        if(Mathf.Abs(player.transform.position.x-transform.position.x)<=mindist && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ContaminatedAttack") && attackCDcounter<=0)
        {
            if(player.transform.position.x<transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            GetComponent<Animator>().SetTrigger("Attack");
            attackCDcounter = (int)(0.5f/Time.fixedDeltaTime);
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

        if(destination!=0 && timeunabletomovecounter > 0)
        {
            GetComponent<Rigidbody2D>().velocityX=(destination-transform.position.x)/Mathf.Abs(destination - transform.position.x)*movespeed;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocityX = GetComponent<Rigidbody2D>().velocityX * 0.9991f;
        }
        if(Mathf.Abs(destination - transform.position.x)<=1 || destination==0)
        {
            if(Random.Range(0,60)==25)
            {
                destination = transform.position.x+Random.Range(-5f,5f);
            }
            else
            {
                destination = 0;
            }
        }
        if(GetComponent<Rigidbody2D>().velocityX<-0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(GetComponent<Rigidbody2D>().velocityX > 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocityX));
    }
}
