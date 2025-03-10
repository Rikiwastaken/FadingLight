using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{



    //HP variables
    [Header("HP variables")]
    public int enemyhp;
    public int enemymaxhp;
    private int tempHP;
    public bool rez = false;

    //Energy variables
    [Header("Energy variables")]
    public int enemyNRG;
    public int enemymaxNRG;
    private int NRGcounter;
    public float NRGdelay;
    private int NRGrecharge;
    public int NRGrechargerate;
    private bool stopenergyregen;
    public bool execution;

    //stun variables
    public bool cannotmove;
    public int hitstundelay;
    private int hitstuncounter;
    private Animator enemyanim;

    [Header("Boss variables")]
    public bool isboss;
    public int worldflagtospawn;
    public int deathworldflag;
    public int LifebarSegments;
    private bool activated;
    private BossLifeBar bossLifeBar;



    //Healthbar
    public Healthbar healthbar;

    private Rigidbody2D rb2D;



    void Start()
    {
        //setting enemy's max heatlth and energy
        enemyhp = enemymaxhp;
        enemyNRG = enemymaxNRG;
        if(!isboss)
        {
            healthbar.SetMaxhealth(enemymaxhp);
            healthbar.SetMaxEnergy(enemymaxNRG);
            cannotmove = GetComponent<EnemyAI>().cannotmove;
        }
        else
        {
            if (FindAnyObjectByType<Global>().worldflags[deathworldflag])
            {
                Destroy(gameObject);
            }
        }
        enemyanim = GetComponent<Animator>();
        execution = false;
        enemyanim.SetBool("Stun", false);
        rb2D = transform.GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {

        if (execution)
        {
            cannotmove = true;
            enemyanim.SetBool("Stun", true);
            rb2D.velocity=new Vector2(0,0);
        }

        if(!isboss)
        {
            GetComponent<EnemyAI>().cannotmove = cannotmove;
        }
        else //boss activation
        {
            if (FindAnyObjectByType<Global>().worldflags[worldflagtospawn] && !activated)
            {
                bossLifeBar = FindAnyObjectByType<BossLifeBar>();
                bossLifeBar.InitiateCombat(this);
                FindAnyObjectByType<Global>().inbossfight = true;
                activated = true;
            }
        }
        


        //Enemy's energy regen when not hit for some time
        if (tempHP == enemyhp & enemyNRG < enemymaxNRG)
        {
            NRGcounter += 1;
            if (NRGcounter >= (int)(NRGdelay/Time.deltaTime) & !stopenergyregen)
            {
                NRGrecharge += 1;
                if (NRGrecharge == NRGrechargerate)
                {
                    enemyanim.SetBool("Stun", false);
                    NRGrecharge = 0;
                    enemyNRG += 1;
                    execution = false;
                }
            }
        }

        //stun enemy if hit by an attack

        if (tempHP!=enemyhp & !cannotmove & hitstuncounter==0)
        {
            hitstuncounter = hitstundelay;
            cannotmove = true;
        }

        if (hitstuncounter>0)
        {
            hitstuncounter -= 1;
            cannotmove = true;
            
        }
        if (hitstuncounter <= 0 & !execution)
        {
            hitstuncounter = 0;
            cannotmove = false;
        }

        //Reinitialization of NRGcounter
        if (enemyNRG == enemymaxNRG | tempHP != enemyhp)
        {
            NRGcounter = 0;
        }

        //temporary execution

        if (enemyNRG<=0)
        {
            
            if (execution && enemyhp < tempHP)
            {
                enemyhp=0;
            }

            if (enemyhp<tempHP)
            {
                execution = true;
            }
            

        }

        //update of the healthbar
        if(!isboss)
        {
            healthbar.SetHealth(enemyhp);
            healthbar.SetEnergy(enemyNRG);
        }

        //death of the enemy

        if (enemyhp <= 0)
        {
            if(isboss)
            {
                FindAnyObjectByType<Global>().worldflags[deathworldflag] = true;
            }
            else
            {
                GetComponentInChildren<Canvas>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                transform.position = new Vector3(-100, -100);
            }
        }
        if (rez)
        {

            transform.position = new Vector2(GetComponent<EnemyAI>().startx, GetComponent<EnemyAI>().starty);
            GetComponent<Rigidbody2D>().velocity=new Vector2(0,0);
            rez = false;
        }
        tempHP = enemyhp;
    }

    public void TakeDamage(int damage)
    {
        enemyhp-=damage;
    }

    public void TakeDamage(int damage, int energydamage)
    {
        enemyhp -= damage;
        enemyNRG-=energydamage;
    }
}
