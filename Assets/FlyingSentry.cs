using System.Collections.Generic;
using UnityEngine;

public class FlyingSentry : MonoBehaviour
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if(FindAnyObjectByType<Global>().atsavepoint)
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


        if(Vector2.Distance(target.transform.position,transform.position) <= mindist)
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

        }

        
        
    }
    private void ManageAttack()
    {
        if(targetting)
        {
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
        if (targetting && Vector2.Distance(target.transform.position,transform.position)>7f)
        {
            GetComponent<Rigidbody2D>().velocity = (target.transform.position- transform.position).normalized*movespeed;
        }
        else if(hacked && Vector2.Distance(player.transform.position, transform.position) > 10f)
        {

            GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * movespeed/2f;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity *= 0.9f;
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
            LaserSight.transform.localScale = new Vector2(Vector3.Distance(wheretoaim, SentryCanon.transform.position) / 3f, 0.03f);
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
