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
    public bool activated;
    private BossLifeBar bossLifeBar;
    public bool bossdying;
    private int bossdeathcounter;
    private float bossscaley;


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
            bossscaley = transform.localScale.y;
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
            if(enemyNRG<=0)
            {
                enemyanim.SetBool("Stun", true);
                if (!isboss)
                {
                    cannotmove = true;

                    rb2D.velocity = new Vector2(0, 0);
                }
            }
            else
            {
                enemyanim.SetBool("Stun", false);
                execution = false;
            }
            
            
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
                bossLifeBar.numberofseparators = LifebarSegments-1;
                bossLifeBar.setupseparatorsbool = true;
                FindAnyObjectByType<Global>().inbossfight = true;
                FindAnyObjectByType<BossWall>().putupwall = true;
                FindAnyObjectByType<musicmanager>().EnterBossMusic();
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
                if(!isboss)
                {
                    enemyhp = 0;
                }
                else
                {
                    BossExecution();
                }
                
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

        if (enemyhp <= 0 && !bossdying)
        {
            if(isboss)
            {
                BossDeath();
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


        if(bossdying)
        {
            transform.localScale = new Vector3(transform.localScale.x, bossscaley*(bossdeathcounter*Time.deltaTime), transform.localScale.z);
            bossdeathcounter--;
            if (bossdeathcounter <= 0)
            {
                FindAnyObjectByType<Global>().worldflags[deathworldflag] = true;
                FindAnyObjectByType<DialogueManager>().TrytoTrigger(deathworldflag);
                FindAnyObjectByType<Global>().inbossfight = false;
                FindAnyObjectByType<BossLifeBar>().EndCombat();
                FindAnyObjectByType<BossWall>().putdownwall = true;
                FindAnyObjectByType<musicmanager>().ExitBossMusic();
                Destroy(gameObject);
            }
        }



    }


    void BossDeath()
    {
        FindAnyObjectByType<BossBeatenText>().StartTitle();
        bossdeathcounter = (int)(1/Time.deltaTime);
        bossdying = true;
    }
    void BossExecution()
    {
        for(int i = LifebarSegments-1; i>=0;i--)
        {
            if(enemyhp >enemymaxhp*i/LifebarSegments)
            {
                enemyhp = enemymaxhp * i / LifebarSegments -1;
                tempHP = enemyhp;
                enemyNRG = enemymaxNRG;
                execution = false;
                enemyanim.SetBool("Stun", false);
                return;
            }
        }
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
