using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{

    PlayerControls controls;
    public bool pressedtrigger;
    private bool pressedjump;

    private GameObject[] GrappleList;

    private Transform closestgrapple;
    private Transform closestenemy;

    public float mindist;

    public Sprite CableSprite;
    public Sprite CableTargetSprite;
    public Sprite EnemyTargetSprite;
    private GameObject cable;
    private Global global;
    public float speed;

    private int grapplecooldown;

    public Vector2 launch;

    private GameObject previousgrapple;

    public float TimeToThrowGrapple;
    private int TimeToThrowGrapplecounter;

    private GameObject grappletarget;
    private GameObject enemytarget;

    public bool grapplingenemy;

    private float lastdist;

    

    private void Awake()
    {
        controls = new PlayerControls();

        controls.gameplay.LeftTrigger.performed += ctx => pressedtrigger = true;
        controls.gameplay.LeftTrigger.canceled += ctx => pressedtrigger = false;
        controls.gameplay.jump.performed += ctx => pressedjump = true;
        controls.gameplay.jump.canceled += ctx => pressedjump = false;
    }

    private void Start()
    {
        global = FindAnyObjectByType<Global>();
        GrappleList = GameObject.FindGameObjectsWithTag("GrapplePoint");
    }

    private void FixedUpdate()
    {
        if(GetComponent<PlayerJumpV3>().grounded)
        {
            previousgrapple = null;
        }


        if(global.grappling && !grapplingenemy)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }


        if(!global.grappling && grapplecooldown<=0 && pressedtrigger)
        {
            GetClosestgrapple();
            GetClosestenemy();
        }
        else if(grapplecooldown>0)
        {
            grapplecooldown--;
        }
        if(!global.grappling && !pressedtrigger)
        {
            closestgrapple = null;
            closestenemy = null;
        }

        if(closestgrapple != null)
        {
            if (Vector2.Distance(transform.position, closestgrapple.position) > mindist)
            {
                closestgrapple = null;
            }
        }

        if (closestenemy != null)
        {
            if (Vector2.Distance(transform.position, closestenemy.position) > mindist)
            {
                closestenemy = null;
            }
        }

        if ((!global.grappling && cable != null) || (closestgrapple==null && closestenemy==null))
        {
            GetComponent<BoxCollider2D>().enabled = true;
            global.grappling = false;
            Destroy(cable);
        }
        else if(global.grappling && cable!=null)
        {
            Transform target = null;
            if(closestgrapple != null && !grapplingenemy)
            {
                target=closestgrapple;
            }
            else if(closestenemy != null && grapplingenemy)
            {
                target = closestenemy;
            }
            else
            {
                ManageVisuals();
                return;
            }


            if(TimeToThrowGrapplecounter>0)
            {
                TimeToThrowGrapplecounter--;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                float fraction = (TimeToThrowGrapple / Time.fixedDeltaTime - TimeToThrowGrapplecounter) / (TimeToThrowGrapple / Time.fixedDeltaTime);
                Vector3 placetoputcable = (target.transform.position - transform.position)*fraction + transform.position;
                cable.transform.position = transform.position + (placetoputcable - transform.position) / 2f;
                Vector3 offset = placetoputcable - transform.position;

                Quaternion rotation = Quaternion.LookRotation(
                                       Vector3.forward, // Keep z+ pointing straight into the screen.
                                       offset           // Point y+ toward the next.
                                     );
                cable.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
                cable.transform.localScale = new Vector3(Vector2.Distance((Vector2)transform.position, (Vector2)placetoputcable), 0.2f, 1f);

            }
            else
            {
                if (Vector2.Distance((Vector2)target.transform.position, (Vector2)transform.position) <= 0.3f || (target.GetComponent<EnemyHP>()!=null && Vector2.Distance((Vector2)target.transform.position, (Vector2)transform.position) <= 1f)) 
                {
                    if (pressedjump && target != closestenemy)
                    {
                        transform.position = target.transform.position -new Vector3(0,0.29f,0);
                    }
                    else
                    {
                        Destroy(cable);
                        global.grappling = false;
                        grapplecooldown = (int)(0.2f / Time.deltaTime);
                        GetComponent<BoxCollider2D>().enabled = true;
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        if (target != closestenemy)
                        {
                            GetComponent<Rigidbody2D>().AddForce(launch, ForceMode2D.Impulse);
                        }
                        return;
                    }
                    
                }
                if (target != closestenemy)
                {
                    GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * speed;
                }
                else
                {
                    if(closestenemy.GetComponent<EnemyHP>().isbig)
                    {
                        GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * speed;
                    }
                    else
                    {
                        target.GetComponent<Rigidbody2D>().velocity = (transform.position - target.transform.position).normalized * speed;
                    }
                }
                
                cable.transform.position = transform.position + (target.transform.position - transform.position) / 2f;
                Vector3 offset = target.transform.position - transform.position;

                Quaternion rotation = Quaternion.LookRotation(
                                       Vector3.forward, // Keep z+ pointing straight into the screen.
                                       offset           // Point y+ toward the next.
                                     );
                cable.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
                cable.transform.localScale = new Vector3(Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position), 0.2f, 1f);
                if(Mathf.Abs(Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position) - lastdist) <=0.05f)
                {
                    if(pressedjump && target!=closestenemy)
                    {
                        
                        transform.position = target.transform.position - new Vector3(0, 0.29f, 0); ;
                    }
                    else
                    {
                        Destroy(cable);
                        global.grappling = false;
                        grapplecooldown = (int)(0.2f / Time.deltaTime);
                        GetComponent<BoxCollider2D>().enabled = true;
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        if (target != closestenemy)
                        {
                            GetComponent<Rigidbody2D>().AddForce(launch, ForceMode2D.Impulse);
                        }
                    }
                    
                }
                lastdist= Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position);
            }

            
            
        }

        ManageVisuals();

    }

    private void ManageVisuals()
    {
        if(closestgrapple != null)
        {
            if(grappletarget == null)
            {
                grappletarget = new GameObject();
                grappletarget.AddComponent<SpriteRenderer>();
                grappletarget.GetComponent<SpriteRenderer>().sprite = CableTargetSprite;
                grappletarget.GetComponent<SpriteRenderer>().color = Color.red;
                grappletarget.GetComponent<SpriteRenderer>().sortingOrder = 2;
                grappletarget.transform.localScale= Vector3.one*0.1f;
            }
            grappletarget.transform.position = closestgrapple.transform.position;
        }
        else
        {
            if(grappletarget!=null)
            {
                Destroy(grappletarget.gameObject);
            }
        }

        if (closestenemy != null)
        {
            if (enemytarget == null)
            {
                enemytarget = new GameObject();
                enemytarget.AddComponent<SpriteRenderer>();
                enemytarget.GetComponent<SpriteRenderer>().sprite = EnemyTargetSprite;
                enemytarget.GetComponent<SpriteRenderer>().sortingOrder = 2;
                enemytarget.transform.localScale = Vector3.one * 0.1f;
            }
            enemytarget.transform.position = closestenemy.transform.position;
        }
        else
        {
            if (enemytarget != null)
            {
                Destroy(enemytarget.gameObject);
            }
        }
    }

    private void OnJump()
    {
        if (global.atsavepoint || global.indialogue || global.zipping || global.grappling)
        {
            return;
        }
        if (closestgrapple != null && pressedtrigger)
        {
            grapplingenemy = false;
            previousgrapple = closestgrapple.gameObject;
            global.grappling = true;
            if (cable != null)
            {
                Destroy(cable);
            }
            TimeToThrowGrapplecounter = (int)(TimeToThrowGrapple / Time.fixedDeltaTime);
            cable = new GameObject();
            cable.AddComponent<SpriteRenderer>();
            cable.GetComponent<SpriteRenderer>().sprite = CableSprite;
            cable.transform.localScale = new Vector3(0f, 0.2f, 1f);
        }
    }

    private void OnAttack()
    {
        if (global.atsavepoint || global.indialogue || global.zipping)
        {
            return;
        }
        if (closestenemy != null && pressedtrigger)
        {
            grapplingenemy = true;
            previousgrapple = closestenemy.gameObject;
            global.grappling = true;
            if (cable != null)
            {
                Destroy(cable);
            }
            TimeToThrowGrapplecounter = (int)(TimeToThrowGrapple / Time.fixedDeltaTime);
            cable = new GameObject();
            cable.AddComponent<SpriteRenderer>();
            cable.GetComponent<SpriteRenderer>().sprite = CableSprite;
            cable.transform.localScale = new Vector3(0f, 0.2f, 1f);
        }
    }

    void OnEnable()
    {
        controls.gameplay.Enable();
    }
    void OnDisable()
    {
        controls.gameplay.Disable();
    }

    private void GetClosestgrapple()
    {
        Transform newclosestgrapple= null;
        float distance = mindist;
        foreach (GameObject grapple in GrappleList)
        {
            if(Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position) < distance && previousgrapple!=grapple)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, grapple.transform.position- transform.position, Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position)-1 ,13);
                if(hit.transform==null)
                {
                    newclosestgrapple = grapple.transform;
                    distance = Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position);
                }
                else if(hit.transform.gameObject.layer!=LayerMask.NameToLayer("wall") && hit.transform.gameObject.layer != LayerMask.NameToLayer("ground"))
                {
                    newclosestgrapple = grapple.transform;
                    distance = Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position);
                }
                
            }
        }
        if(newclosestgrapple != null && newclosestgrapple!= previousgrapple)
        {
            closestgrapple = newclosestgrapple;
        }
    }


    private void GetClosestenemy()
    {
        Transform newclosestenemy = null;
        EnemyHP[] enemylist = FindObjectsByType<EnemyHP>(FindObjectsSortMode.None);
        float distance = 1f;
        foreach (EnemyHP enemy in enemylist)
        {
            if (Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position) < distance && previousgrapple != enemy)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, enemy.transform.position - transform.position, Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position) - 1, 13);
                if (hit.transform == null)
                {
                    newclosestenemy = enemy.transform;
                    distance = Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position);
                }
                else if (hit.transform.gameObject.layer != LayerMask.NameToLayer("wall") && hit.transform.gameObject.layer != LayerMask.NameToLayer("ground"))
                {
                    newclosestenemy = enemy.transform;
                    distance = Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position);
                }

            }
        }
        if (newclosestenemy != null && newclosestenemy != previousgrapple)
        {
            closestenemy = newclosestenemy;
        }
    }

}
