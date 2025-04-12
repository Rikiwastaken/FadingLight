using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static GadgetScript;

public class MovingSentryAI : MonoBehaviour
{

    public GameObject LaserSight;
    public GameObject SentryCanon;
    public GameObject target;
    public List<Vector2> targetpos = new List<Vector2>();
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

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        target = FindAnyObjectByType<PlayerHP>().gameObject;
        canonanimspeed = SentryCanon.GetComponentInChildren<Animator>().speed;
        mainanimspeed = GetComponent<Animator>().speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(target.transform.position,transform.position) <= mindist)
        {
            targetting = true;
        }
        else
        {
            targetting = false;
        }

        ManageTargetting();

        ManageMovement();

        ManageAttack();
        
    }

    private void ManageAttack()
    {
        if(targetting)
        {
            timebeforeattackcounter++;
            if (timebeforeattackcounter == (int)(timebeforeattack / Time.fixedDeltaTime) + activesalvo * (int)(timebeforenextsalvoe / Time.fixedDeltaTime))
            {
                GameObject bullet =Instantiate(projectileprefab, transform.position,Quaternion.identity);
                BulletScript bulletScript  = bullet.GetComponent<BulletScript>();
                bulletScript.damage = (int)damage;
                bulletScript.speed = bulletScript.speed / 2f;
                bulletScript.damagePlayer = true;
                bulletScript.directionvector = target.transform.position - SentryCanon.transform.position ;
                bulletScript.transform.localScale = Vector3.one * 0.5f;
                activesalvo += 1;
                if (activesalvo == numberofsalvoes)
                {
                    activesalvo = 0;
                    timebeforeattackcounter = 0;
                }
            }
        }
        else
        {
            timebeforeattackcounter = 0;
            activesalvo = 0;
        }
    }
    private void ManageMovement()
    {
        if(targetting)
        {

            float direction = (target.transform.position.x - transform.position.x)/Mathf.Abs(target.transform.position.x - transform.position.x);

            GetComponent<Rigidbody2D>().velocity = new Vector2(direction * movespeed, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (GetComponent<Rigidbody2D>().velocityX > 0.1f)
        {
            GetComponent<Animator>().speed = mainanimspeed;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(GetComponent<Rigidbody2D>().velocityX < -0.1f)
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
            LaserSight.transform.localScale = new Vector2(Vector3.Distance(wheretoaim, SentryCanon.transform.position) / 3f, 0.01f);
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
