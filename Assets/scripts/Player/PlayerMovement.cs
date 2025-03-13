using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]



public class PlayerMovement : MonoBehaviour
{

    PlayerControls controls;

    //safezone variables
    private float startx;
    private float starty;
    public float safedetectrange;
    [SerializeField] private LayerMask whatissafe;
    [SerializeField] private Transform groundcheck;
    public GameObject[] deadenemy;
    public bool issafe;

    //necessary for anim and physics
    private Rigidbody2D rb2D;
    private Animator myanimator;

    public bool facingRight = true;

    //modifiable variables 
    public float speed = 2.0f;
    public float maxspeed;
    public float slowdownspeed; //counterforcewhen no input
    private float valueright;
    private float valueleft;
    public float horizontal; // 1,-1,0
    public float vertical;
    private bool jumppressed;

    public bool rolling;
    public float rollingspeed;

    private PlayerJumpV3 playerjump;


    private void Awake()
    {
        controls = new PlayerControls();

        controls.gameplay.moveleft.performed += ctx => valueleft = 1;
        controls.gameplay.moveright.performed += ctx => valueright = 1;
        controls.gameplay.moveleft.canceled += ctx => valueleft = 0;
        controls.gameplay.moveright.canceled += ctx => valueright = 0;
        controls.gameplay.down.performed += ctx => vertical = 1;
        controls.gameplay.down.canceled += ctx => vertical = 0;
        controls.gameplay.jump.performed += ctx => jumppressed = true;
        controls.gameplay.jump.canceled += ctx => jumppressed = false;

    }

    private void Start()
    {
        //Define the gamobjects found on the player
        rb2D = GetComponent<Rigidbody2D>();
        myanimator = GetComponent<Animator>();
        playerjump = GetComponent<PlayerJumpV3>();
    }

    //Handles running of the physics
    private void FixedUpdate()
    {
        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            myanimator.SetBool("atsavepoint", true);
            myanimator.SetLayerWeight(1, 0);
            return;
        }
        else
        {
            myanimator.SetBool("atsavepoint", false);
        }
        if (FindAnyObjectByType<Global>().indialogue)
        {
            if(Mathf.Abs(rb2D.velocity.x)>0.05)
            {
                Flip(rb2D.velocity.x);
                myanimator.SetFloat("speed", Mathf.Abs(rb2D.velocity.x));
            }
            else
            {
                myanimator.SetFloat("speed", 0);
            }
            
            return;
        }

        if (!playerjump.stuckinwall)
        {
            //check if key pressed
            if (valueleft == 1 & valueright == 0)
            {
                horizontal = -1;
            }
            if (valueleft == 0 & valueright == 1)
            {
                horizontal = 1;
            }
            if (valueleft == 0 & valueright == 0)
            {
                horizontal = 0;
            }
            if (valueleft == 1 & valueright == 1)
            {
                horizontal = 0;
            }
        }


        //move player
        if (!playerjump.stuckinwall)
        {
            if (Mathf.Abs(rb2D.velocity.x) < maxspeed)
            {
                float newspeed = rb2D.velocityX + horizontal * speed;
                if (Mathf.Abs(newspeed) > maxspeed)
                {
                    newspeed = horizontal * maxspeed;
                }
                rb2D.velocityX = newspeed;
            }
            if (horizontal == 0)
            {
                if (Mathf.Abs(rb2D.velocity.x) <= 0.01)
                {
                    rb2D.velocityX = 0f;
                }
                else
                {
                    if (rb2D.velocity.x > 0)
                    {
                        rb2D.velocityX -= slowdownspeed;
                    }
                    else
                    {
                        rb2D.velocityX += slowdownspeed;
                    }
                }
            }
            if ((horizontal > 0 && rb2D.velocity.x < 0) || (horizontal < 0 && rb2D.velocity.x > 0))
            {
                if (rb2D.velocity.x > 0)
                {
                    rb2D.velocityX -= slowdownspeed;
                }
                else
                {
                    rb2D.velocityX += slowdownspeed;
                }
            }


            if (rolling)
            {
                rb2D.velocityX = (transform.localScale.x / Mathf.Abs(transform.localScale.x)) * rollingspeed;
            }

            Flip(horizontal);
            myanimator.SetFloat("speed", Mathf.Abs(horizontal));
        }
        else
        {
            if (!playerjump.grounded && horizontal != 0 && !GetComponent<PlayerJumpV3>().stuckinwall)
            {
                Flip(horizontal);
            }
        }
        issafe = Physics2D.OverlapCircle(groundcheck.position, safedetectrange, whatissafe);
        if (playerjump.grounded && Physics2D.OverlapCircle(transform.position, 0.1f, whatissafe) && (vertical == 1||jumppressed))
        {
            SavePointScript[] savepointlist =FindObjectsByType<SavePointScript>(FindObjectsSortMode.InstanceID);
            SavePointScript closest = null;
            float Distance = 1000000;
            foreach (SavePointScript save in savepointlist)
            {
                if(Vector2.Distance(save.transform.position,transform.position)<Distance)
                {
                    closest = save;
                    Distance = Vector2.Distance(save.transform.position, transform.position);
                }
            }
            if(closest != null)
            {
                closest.InteractWithSavePoint();
            }
        }

    }
    //flipping function
    private void Flip(float horizontal)
    {
        if (horizontal < 0 && facingRight || horizontal>0 && !facingRight)
        {
            facingRight = !facingRight;

            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;

        }
    }
    public void safezone()
    {
        if(!facingRight)
        {
            Flip(1);
        }
        deadenemy = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in deadenemy)
        {
            if (enemy.tag == "enemy")
            {
                enemy.GetComponent<EnemyHP>().enemyhp = enemy.GetComponent<EnemyHP>().enemymaxhp;
                enemy.GetComponent<EnemyHP>().enemyNRG = enemy.GetComponent<EnemyHP>().enemymaxNRG;
                enemy.GetComponentInChildren<Canvas>().enabled = true;
                enemy.GetComponent<Collider2D>().enabled = true;
                enemy.GetComponent<SpriteRenderer>().enabled = true;
                enemy.GetComponent<EnemyHP>().enabled = true;
                enemy.GetComponent<EnemyHP>().rez = true;
                enemy.GetComponent<EnemyAI>().targetted = false;
                enemy.GetComponent<EnemyHP>().execution = false;
                enemy.GetComponent<Animator>().SetBool("Stun", false);
            }
        }
        GetComponent<PlayerHP>().Eldonhp = GetComponent<PlayerHP>().Eldonmaxhp;
    }
    void OnEnable()
    {
        controls.gameplay.Enable();
    }
    void OnDisable()
    {
        controls.gameplay.Disable();
    }
}
