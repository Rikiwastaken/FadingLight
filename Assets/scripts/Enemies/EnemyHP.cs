using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{



    //HP variables
    [Header("HP variables")]
    public int enemyhp;
    public int enemymaxhp;
    private int tempHP;
    public bool rez = false;


    [Header("EnemyType variables")]
    public GameObject Itemdrop;
    public int quantity;

    //Energy variables
    [Header("Energy variables")]
    public int enemyNRG; // current energy
    public int enemymaxNRG; // max nrg
    private int NRGcounter; 
    public float NRGdelay; //Time Before energy starts recharging
    private int NRGrecharge; // Frames between rehcarge (counter)
    public int NRGrechargerate; // Frames between rehcarge
    private bool stopenergyregen;
    public bool execution;
    public bool isbig;

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

    //HP variables
    [Header("EnemyType variables")]
    public bool ismachine;
    public bool iscontaminated;
    public bool isflying;
    public bool hacked;


    //Healthbar
    private Healthbar healthbar;

    private Rigidbody2D rb2D;

    private Global global;

    private BossLifeBar BossLifeBar;

    private musicmanager Musicmanager;

    private BossWall BossWall;

    private Vector2 start;
    public bool targetted;

    private GadgetScript gadgetScript;
    private int hackinghpdraincounter;
    public Vector2 grappleOffset;
    private bool ded;

    public bool cantbemoved;
    void Start()
    {
        start = transform.position;
        global = FindAnyObjectByType<Global>();
        BossLifeBar = FindAnyObjectByType<BossLifeBar>();
        Musicmanager = FindAnyObjectByType<musicmanager>();
        BossWall = FindAnyObjectByType<BossWall>();
        gadgetScript = FindAnyObjectByType<GadgetScript>();
        //setting enemy's max heatlth and energy
        enemyhp = enemymaxhp;
        enemyNRG = enemymaxNRG;
        if(!isboss)
        {
            healthbar = GetComponentInChildren<Healthbar>();
            healthbar.SetMaxhealth(enemymaxhp);
            healthbar.SetMaxEnergy(enemymaxNRG);
        }
        else
        {
            bossscaley = transform.localScale.y;
            if (global.worldflags[deathworldflag])
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

        if((global.inbossfight || global.indialogue) && !isboss && enemyhp>0)
        {
            transform.position = start;
        }
        if(gadgetScript.invisibilityFrames>0 && !isboss)
        {
            targetted = false;
        }

        if(ismachine)
        {
            if (hacked)
            {
                enemyanim.SetBool("Hacked", true);
                if(hackinghpdraincounter==0)
                {
                    enemyhp -=(int) (enemymaxhp * 0.03f);
                    hackinghpdraincounter = (int)(1/Time.fixedDeltaTime);
                }
                else
                {
                    hackinghpdraincounter--;
                }
            }
            else
            {
                enemyanim.SetBool("Hacked", false);
            }
        }
        

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

        if(isboss) //boss activation
        {
            
            if (global.worldflags[worldflagtospawn] && !activated)
            {
                BossLifeBar.InitiateCombat(this);
                BossLifeBar.numberofseparators = LifebarSegments-1;
                BossLifeBar.setupseparatorsbool = true;
                global.inbossfight = true;
                BossWall.putupwall = true;
                Musicmanager.EnterBossMusic();
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
        if (enemyNRG == enemymaxNRG)
        {
            NRGcounter = 0;
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
            else if(!ded)
            {
                ded = true;
                GetComponentInChildren<Canvas>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                ItemDropfct();
                transform.position = new Vector3(-100, -100);
            }
        }
        if (rez)
        {
            ded = false;
            transform.position = start;
            hacked = false;
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
                global.worldflags[deathworldflag] = true;
                FindAnyObjectByType<DialogueManager>().TrytoTrigger(deathworldflag);
                global.inbossfight = false;
                BossLifeBar.EndCombat();
                BossWall.putdownwall = true;
                Musicmanager.ExitBossMusic();
                Destroy(gameObject);
            }
        }



    }

    void ItemDropfct()
    {
        GameObject Item = Instantiate(Itemdrop, transform.position, quaternion.identity);
        Item.GetComponent<ItemDrop>().quantity=quantity;
        if(ismachine)
        {
            Item.GetComponent<ItemDrop>().type = 6;
        }
        else if (iscontaminated)
        {
            Item.GetComponent<ItemDrop>().type = 8;
        }
        else
        {
            Item.GetComponent<ItemDrop>().type = 7;
        }
    }

    void BossDeath()
    {
        FindAnyObjectByType<BossBeatenText>().StartTitle();
        bossdeathcounter = (int)(6/Time.deltaTime);
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

                if (FindAnyObjectByType<AugmentsScript>().EquipedAugments[16])
                {
                    FindAnyObjectByType<PlayerHP>().Eldonhp = FindAnyObjectByType<PlayerHP>().Eldonmaxhp;
                }

                return;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(damage<0)
        {
            damage = 0;
        }
        enemyhp-=damage;
        NRGcounter = 0;
        if (enemyNRG <= 0)
        {

            if (execution && enemyhp < tempHP)
            {
                if (ismachine)
                {
                    
                }
                else if(isboss)
                {
                    BossExecution();
                }
                else
                {
                    if (FindAnyObjectByType<AugmentsScript>().EquipedAugments[16])
                    {
                        FindAnyObjectByType<PlayerHP>().Eldonhp = FindAnyObjectByType<PlayerHP>().Eldonmaxhp;
                    }
                    enemyhp = 0;
                }

            }

            if (!execution)
            {
                execution = true;
            }


        }

    }

    public void TakeDamage(int damage, int energydamage)
    {
        if (damage < 0)
        {
            damage = 0;
        }
        if (energydamage < 0)
        {
            energydamage = 0;
        }
        enemyhp -= damage;
        enemyNRG -= energydamage;
        if (enemyNRG < 0)
        {
            enemyNRG = 0;
        }
        if (isboss && !global.inbossfight)
        {
            return;
        }
        if (enemyNRG <= 0)
        {

            if (execution && enemyhp < tempHP)
            {
                if (ismachine)
                {

                }
                else if (isboss)
                {
                    BossExecution();
                }
                else
                {
                    if (FindAnyObjectByType<AugmentsScript>().EquipedAugments[16])
                    {
                        FindAnyObjectByType<PlayerHP>().Eldonhp = FindAnyObjectByType<PlayerHP>().Eldonmaxhp;
                    }
                    enemyhp = 0;
                }

            }

            if (!execution)
            {
                execution = true;
            }


        }
        
        
        NRGcounter = 0;
        
    }

    public void TakeDamage(int damage, int energydamage, Vector2 Force)
    {
        if (damage < 0)
        {
            damage = 0;
        }
        if (energydamage < 0)
        {
            energydamage = 0;
        }
        enemyhp -= damage;
        enemyNRG -= energydamage;
        if (enemyNRG < 0)
        {
            enemyNRG = 0;
        }
        if (isboss && !global.inbossfight)
        {
            return;
        }
        if (enemyNRG <= 0)
        {

            if (execution && enemyhp < tempHP)
            {
                if (ismachine)
                {

                }
                else if (isboss)
                {
                    BossExecution();
                }
                else
                {
                    if (FindAnyObjectByType<AugmentsScript>().EquipedAugments[16])
                    {
                        FindAnyObjectByType<PlayerHP>().Eldonhp = FindAnyObjectByType<PlayerHP>().Eldonmaxhp;
                    }
                    enemyhp = 0;
                }

            }

            if (!execution)
            {
                execution = true;
            }


        }
        if (!isboss)
        {
            if(isflying)
            {
                GetComponent<Rigidbody2D>().AddForce(Force/50000f, ForceMode2D.Impulse);
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(Force, ForceMode2D.Impulse);
            }
            
        }
        NRGcounter = 0;
        
        
    }
}
