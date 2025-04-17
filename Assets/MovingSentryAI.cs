using System.Collections.Generic;
using UnityEngine;

public class MovingSentryAI : MonoBehaviour
{

    public GameObject LaserSight;
    public GameObject SentryCanon;
    public GameObject target;
    public List<Vector2> targetpos = new List<Vector2>();
    public GameObject Exclamationmark;
    public float mindist;
    private bool targetting;
    private float canonanimspeed;
    private float mainanimspeed;
    public float movespeed;

    public float timebeforeattack;
    private int timebeforeattackcounter;
    public float timebeforenextsalvoe;
    public int numberofsalvoes;
    public GameObject projectileprefab;
    private int activesalvo;
    public Transform wheretospawnbullets;
    private bool hacked;
    private GameObject player;

    private int lasthp;
    public float timeunabletomove;
    public int timeunabletomovecounter;


    public float damage;

    private EnemyHP[] allenemies;

    // Start is called before the first frame update
    void Start()
    {
        canonanimspeed = SentryCanon.GetComponentInChildren<Animator>().speed;
        mainanimspeed = GetComponent<Animator>().speed;
        allenemies = FindObjectsByType<EnemyHP>(FindObjectsSortMode.None);
        player = FindAnyObjectByType<PlayerHP>().gameObject;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }
        if (collision.transform.GetComponent<PlayerHP>() != null && (GetComponent<BoxCollider2D>().bounds.center.y- GetComponent<BoxCollider2D>().bounds.size.y/2f <= collision.transform.GetComponent<BoxCollider2D>().bounds.center.y - collision.transform.GetComponent<BoxCollider2D>().bounds.size.y / 2f))
        {
            int direction = (int)((collision.transform.position.x - transform.position.x)/Mathf.Abs(collision.transform.position.x-transform.position.x));
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction*5,3), ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            return;
        }
        hacked = GetComponent<EnemyHP>().hacked;
        if (hacked)
        {
            float distance  = 100000;
            allenemies = FindObjectsByType<EnemyHP>(FindObjectsSortMode.None);
            foreach (EnemyHP enemy in allenemies)
            {
                if(enemy != GetComponent<EnemyHP>() && !enemy.hacked && Vector2.Distance(enemy.transform.position,transform.position)<distance && !enemy.isboss)
                {
                    target = enemy.gameObject;
                    distance = Vector2.Distance(enemy.transform.position, transform.position);
                }
            }
        }
        else
        {
            target = player;
        }


        if(Vector2.Distance(target.transform.position,transform.position) <= mindist && FindAnyObjectByType<GadgetScript>().invisibilityFrames == 0)
        {
            targetting = true;
        }
        else
        {
            targetting = false;
        }

        if (GetComponent<EnemyHP>().enemyNRG > 0)
        {
            ManageTargetting();

            ManageMovement();

            ManageAttack();

            managehitstun();
        }

        
        
    }

    private void managehitstun()
    {

        if(timeunabletomovecounter > 0)
        {
            timeunabletomovecounter--;
        }

        if(lasthp> GetComponent<EnemyHP>().enemyhp && timeunabletomovecounter == 0)
        {
            timeunabletomovecounter= (int)(timeunabletomove/Time.deltaTime);
        }


        lasthp = GetComponent<EnemyHP>().enemyhp;
    }

    private void ManageAttack()
    {
        if(targetting)
        {
            if (timeunabletomovecounter > 0)
            {
                return;
            }
            if(timebeforeattackcounter >= (int)(timebeforeattack / Time.fixedDeltaTime)*2/3)
            {
                Exclamationmark.SetActive(true);
            }
            timebeforeattackcounter++;
            if (timebeforeattackcounter == (int)(timebeforeattack / Time.fixedDeltaTime) + activesalvo * (int)(timebeforenextsalvoe / Time.fixedDeltaTime))
            {
                GameObject bullet =Instantiate(projectileprefab, wheretospawnbullets.transform.position,Quaternion.identity);
                BulletScript bulletScript  = bullet.GetComponent<BulletScript>();
                bulletScript.damage = (int)damage;
                bulletScript.sender = gameObject;
                bulletScript.speed = bulletScript.speed / 2f;
                if(!hacked)
                {
                    bulletScript.damagePlayer = true;
                }
                bulletScript.directionvector = target.transform.position - SentryCanon.transform.position ;
                bulletScript.transform.localScale = Vector3.one * 0.5f;
                activesalvo += 1;
                if (activesalvo == numberofsalvoes)
                {
                    activesalvo = 0;
                    timebeforeattackcounter = 0;
                    Exclamationmark.SetActive(false);
                }
            }
        }
        else
        {
            timebeforeattackcounter = 0;
            activesalvo = 0;
            Exclamationmark.SetActive(false);
        }
    }
    private void ManageMovement()
    {
        if (timeunabletomovecounter > 0 || FindAnyObjectByType<GrappleScript>().target==transform)
        {
            GetComponent<Animator>().speed = 0f;
            return;
        }
        if (targetting && Mathf.Abs(target.transform.position.x-transform.position.x)>2.5f)
        {

            float direction = (target.transform.position.x - transform.position.x)/Mathf.Abs(target.transform.position.x - transform.position.x);

            GetComponent<Rigidbody2D>().velocity = new Vector2(direction * movespeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if(hacked && Mathf.Abs(target.transform.position.x - transform.position.x) > 5f)
        {
            float direction = (player.transform.position.x - transform.position.x) / Mathf.Abs(player.transform.position.x - transform.position.x);

            GetComponent<Rigidbody2D>().velocity = new Vector2(direction * movespeed/2f, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (GetComponent<Rigidbody2D>().velocityX > 0.5f)
        {
            GetComponent<Animator>().speed = mainanimspeed;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(GetComponent<Rigidbody2D>().velocityX < -0.5f)
        {
            GetComponent<Animator>().speed = mainanimspeed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<Animator>().speed = 0f;
        }
    }

    private void ManageTargetting()
    {
        if (targetting)
        {
            Vector3 wheretoaim = managewheretoaim();
            Vector3 offset = wheretoaim - transform.position;

            Quaternion rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the target.
                                 );
            SentryCanon.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            LaserSight.transform.position = Vector3.Lerp(wheretoaim, SentryCanon.transform.position, 0.5f); ;
            LaserSight.transform.localScale = new Vector2(Vector3.Distance(wheretoaim, SentryCanon.transform.position) / 3f, 0.015f);
            SentryCanon.GetComponentInChildren<Animator>().speed = canonanimspeed;
        }
        else
        {
            LaserSight.transform.localScale = Vector3.zero;
            SentryCanon.GetComponentInChildren<Animator>().speed = 0f;
        }
    }

    private Vector2 managewheretoaim()
    {
        if (targetpos.Count > (int)(1f / (Time.fixedDeltaTime * 5f)))
        {
            for(int i = (int)(1f / (Time.fixedDeltaTime * 5f));i<targetpos.Count;i++)
            {
                targetpos.RemoveAt(i);
            }
        }
        else
        {
            targetpos.Add(target.transform.position);
        }
        for (int i = 0; i < targetpos.Count - 1; i++)
        {
            targetpos[i + 1] = targetpos[i];
        }
        targetpos[0] = target.transform.position;
        return targetpos[targetpos.Count - 1];
    }
}
