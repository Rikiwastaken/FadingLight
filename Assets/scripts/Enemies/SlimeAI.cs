using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    [Header("movement")]
    public float detectdist;
    public float xspeed;
    private Transform target;
    private Rigidbody2D rb2D;

    [Header("HP & energy")]
    public int enemyHP;
    private int tempenemyhp;
    public int hitdelay;
    public float energystunduration;
    public int delaycounter;

    [Header("Attack")]
    public float attackrange;
    public float abandonrange;
    public bool cannotmoveatk;
    private bool initiateattack;
    private float distplayer;
    private int attackcounter;
    public float timebeforejump;
    public float jumpforcex;
    public float jumpforcey;
    private float atkcdcounter;
    public float atkcd;
    public int attackdmg;

    private Vector2 targetpos;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2D = transform.GetComponent<Rigidbody2D>();
        enemyHP = this.GetComponent<EnemyHP>().enemyhp;
        tempenemyhp = enemyHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }
        if (atkcdcounter!=0 && Mathf.Abs(rb2D.velocityX)>0.05 && collision.transform.tag=="Player")
        {
            collision.transform.GetComponent<PlayerHP>().TakeDamage(attackdmg, new Vector2(collision.transform.GetComponent<Rigidbody2D>().velocityX, collision.transform.GetComponent<PlayerHP>().hitjumpforce), Vector2.zero);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }

        if (GetComponent<EnemyHP>().targetted && distplayer>= abandonrange || enemyHP <= 0)
        {
            GetComponent<EnemyHP>().targetted = false;
            GameObject.Find("music").GetComponent<musicmanager>().playcbt = false;
        }

        if (GetComponent<EnemyHP>().targetted)
        {
            GameObject.Find("music").GetComponent<musicmanager>().playcbt = true;
        }

        if (atkcdcounter != 0)
        {
            atkcdcounter -= 1;
        }
        else
        {


            Managedirection();
            

            if (distplayer <= detectdist || GetComponent<EnemyHP>().targetted)
            {
                GetComponent<EnemyHP>().targetted = true;
                if (delaycounter == 0 && !GetComponent<EnemyHP>().cannotmove && !cannotmoveatk && Mathf.Abs(target.position.x - transform.position.x)>0.25f)
                {
                    if (target.position.x < transform.position.x)
                    {
                        if (rb2D.velocity.x > -xspeed)
                        {
                            rb2D.velocity = new Vector2(rb2D.velocity.x - xspeed * 0.08f, rb2D.velocity.y);
                        }
                    }
                    else
                    {
                        if (rb2D.velocity.x < xspeed)
                        {
                            rb2D.velocity = new Vector2(rb2D.velocity.x + xspeed * 0.08f, rb2D.velocity.y);
                        }
                    }
                }

            }


        }
        enemyHP = GetComponent<EnemyHP>().enemyhp;
        distplayer = Vector2.Distance(target.position, transform.position);


        if (tempenemyhp != enemyHP)
        {
            delaycounter = hitdelay;
            tempenemyhp = enemyHP;
        }

        if (delaycounter!=0)
        {
            delaycounter -= 1;
        }

        

        if (GetComponent<EnemyHP>().targetted && distplayer < attackrange && !initiateattack && atkcdcounter==0 && GetComponent<EnemyHP>().enemyNRG>0)
        {
            cannotmoveatk = true;
            attackcounter = (int)(timebeforejump/Time.deltaTime);
            initiateattack = true;
            targetpos = target.position;

        }

        if (initiateattack && attackcounter!=0)
        {
            attackcounter -= 1;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (initiateattack && attackcounter == 0)
        {
            if (targetpos.x < transform.position.x)
            {
                rb2D.AddForce(new Vector2(-jumpforcex, jumpforcey),ForceMode2D.Impulse);
            }
            if (targetpos.x > transform.position.x)
            {
                rb2D.AddForce(new Vector2(jumpforcex, jumpforcey), ForceMode2D.Impulse);
            }

            initiateattack = false;
            cannotmoveatk=false;
            atkcdcounter = atkcd;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectdist);
    }

    void Managedirection()
    {
        if (target != null)
        {
            if (target.transform.position.x <= transform.position.x)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                transform.GetChild(1).localScale = new Vector2(Mathf.Abs(transform.GetChild(1).localScale.x), transform.GetChild(1).localScale.y);

            }
            else
            {
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                transform.GetChild(1).localScale = new Vector2(-Mathf.Abs(transform.GetChild(1).localScale.x), transform.GetChild(1).localScale.y);
            }
        }
    }
    
}

