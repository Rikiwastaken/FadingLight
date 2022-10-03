using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectdist;
    public float xspeed;
    private Transform target;
    private Rigidbody2D rb2D;
    public int enemyHP;
    private int tempenemyhp;
    public int hitdelay;
    public float energystunduration;
    public int delaycounter;
    public bool cannotmove;
    public bool targetted;
    private bool walkingright = false;
    private bool lookingright = false;

    //starting position for respawn
    public float startx;
    public float starty;




    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2D = transform.GetComponent<Rigidbody2D>();
        enemyHP = this.GetComponent<EnemyHP>().enemyhp;
        tempenemyhp = enemyHP;
        startx = GetComponent<Rigidbody2D>().position.x;
        starty = GetComponent<Rigidbody2D>().position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (walkingright && !lookingright)
        {
            GetComponent<SpriteRenderer>().flipX=true;
            lookingright = true;
        }
        if (!walkingright && lookingright)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            lookingright = false;
        }

        enemyHP = GetComponent<EnemyHP>().enemyhp;
        float distance = Vector2.Distance(target.position, transform.position);


        if (tempenemyhp != enemyHP)
        {
            delaycounter = hitdelay;
            tempenemyhp = enemyHP;
        }

        if (delaycounter!=0)
        {
            delaycounter -= 1;
        }

        if (distance<=detectdist || targetted)
        {
            targetted = true;
            if (delaycounter == 0 & !cannotmove)
            {
                if (target.position.x < transform.position.x)
                {
                    if (rb2D.velocity.x> -xspeed)
                    {
                        rb2D.velocity = new Vector2(rb2D.velocity.x - xspeed * 0.08f, 0);
                    }
                    walkingright = false;
                }
                else
                {
                    if (rb2D.velocity.x < xspeed)
                    {
                        rb2D.velocity = new Vector2(rb2D.velocity.x + xspeed * 0.08f, 0);
                    }
                    walkingright = true;
                }
            }
            
        }
        


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectdist);
    }
}

