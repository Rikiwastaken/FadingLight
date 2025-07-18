using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class GrappleScript : MonoBehaviour
{

    PlayerControls controls;
    public bool pressedtrigger;
    private bool pressedjump;

    private GameObject[] GrappleList;

    private Transform closestgrapple;
    private Transform closestenemy;

    public float mindist;

    public GameObject ChainPrefab;
    private List<GameObject> LastChain = new List<GameObject>();
    public Sprite CableTargetSprite;
    public float linkLength = 0.5f;
    public Sprite EnemyTargetSprite;
    public Sprite EnemyHackTargetSprite;
    private bool hacking;
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

    public Transform target;

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


        if(!global.grappling && grapplecooldown<=0 && pressedtrigger && global.worldflags[13])
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

        if ((!global.grappling && LastChain.Count>0) || (closestgrapple==null && closestenemy==null))
        {
            GetComponent<BoxCollider2D>().enabled = true;
            global.grappling = false;
            foreach(GameObject chain in LastChain)
            {
                Destroy(chain);
            }
            hacking = false;
        }
        else if(global.grappling)
        {
            target = null;
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
                Vector3 placetoputcable = Vector3.zero;
                if (target.GetComponent<EnemyHP>() != null)
                {
                    placetoputcable = (target.transform.position+(Vector3)target.GetComponent<EnemyHP>().grappleOffset - transform.position) * fraction + transform.position;
                }
                else
                {
                    placetoputcable = (target.transform.position - transform.position) * fraction + transform.position;
                }
                
                GenerateChain((Vector2)transform.position, (Vector2)placetoputcable);

            }
            else
            {
                if (Vector2.Distance((Vector2)target.transform.position, (Vector2)transform.position) <= 0.3f || (target.GetComponent<EnemyHP>()!=null && Vector2.Distance((Vector2)target.transform.position, (Vector2)transform.position) <= 3f)) 
                {
                    if (pressedjump && target != closestenemy)
                    {
                        transform.position = target.transform.position -new Vector3(0,0.29f,0);
                    }
                    else
                    {
                        foreach (GameObject chain in LastChain)
                        {
                            Destroy(chain);
                        }
                        global.grappling = false;
                        grapplecooldown = (int)(0.2f / Time.deltaTime);
                        GetComponent<BoxCollider2D>().enabled = true;
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        if (target != closestenemy)
                        {
                            GetComponent<Rigidbody2D>().AddForce(launch, ForceMode2D.Impulse);
                            GetComponent<PlayerJumpV3>().jump2 = false;
                            GetComponent<PlayerDodge>().resetairdash = true;
                        }
                        else
                        {
                            target.GetComponent<Rigidbody2D>().velocity = target.GetComponent<Rigidbody2D>().velocity / 3f;
                        }
                        return;
                    }
                    
                }
                Vector2 targetpos = (Vector2)target.transform.position;
                if (target != closestenemy)
                {
                    GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * speed;
                }
                else if (hacking)
                {
                    target.GetComponent<EnemyHP>().hacked = true;
                    target.GetComponent<EnemyHP>().enemyNRG = target.GetComponent<EnemyHP>().enemymaxNRG;
                    hacking = false;
                    foreach (GameObject chain in LastChain)
                    {
                        Destroy(chain);
                    }
                    global.grappling = false;
                    grapplecooldown = (int)(0.2f / Time.deltaTime);
                    GetComponent<BoxCollider2D>().enabled = true;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    closestenemy = null;
                    return;
                }
                else
                {
                    if(closestenemy.GetComponent<EnemyHP>().isbig && !GetComponent<AugmentsScript>().EquipedAugments[11])
                    {
                        GetComponent<Rigidbody2D>().velocity = (target.transform.position + (Vector3)target.GetComponent<EnemyHP>().grappleOffset - transform.position).normalized * speed;
                    }
                    else
                    {
                        target.GetComponent<Rigidbody2D>().velocity = (transform.position - (Vector3)target.GetComponent<EnemyHP>().grappleOffset - target.transform.position).normalized * speed;
                    }
                    targetpos += target.GetComponent<EnemyHP>().grappleOffset;
                }
                GenerateChain((Vector2)transform.position, targetpos);
                if(target.GetComponent<EnemyHP>())
                {
                    if (Mathf.Abs(Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position + target.GetComponent<EnemyHP>().grappleOffset) - lastdist) <= 0.01f)
                    {
                        if (pressedjump && target != closestenemy)
                        {

                            GetComponent<Rigidbody2D>().MovePosition(target.transform.position - new Vector3(0, 0.29f, 0));
                        }
                        else
                        {
                            foreach (GameObject chain in LastChain)
                            {
                                Destroy(chain);
                            }
                            global.grappling = false;
                            grapplecooldown = (int)(0.2f / Time.deltaTime);
                            GetComponent<BoxCollider2D>().enabled = true;
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            if (target != closestenemy)
                            {
                                GetComponent<Rigidbody2D>().AddForce(launch, ForceMode2D.Impulse);
                            }
                            else
                            {
                                target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            }
                        }

                    }
                    lastdist = Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position + target.GetComponent<EnemyHP>().grappleOffset);
                }
                else
                {
                    if (Mathf.Abs(Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position) - lastdist) <= 0.01f)
                    {
                        if (pressedjump && target != closestenemy)
                        {

                            GetComponent<Rigidbody2D>().MovePosition(target.transform.position - new Vector3(0, 0.29f, 0));
                        }
                        else
                        {
                            foreach (GameObject chain in LastChain)
                            {
                                Destroy(chain);
                            }
                            global.grappling = false;
                            grapplecooldown = (int)(0.2f / Time.deltaTime);
                            GetComponent<BoxCollider2D>().enabled = true;
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            if (target != closestenemy)
                            {
                                GetComponent<Rigidbody2D>().AddForce(launch, ForceMode2D.Impulse);
                            }
                            else
                            {
                                target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            }
                        }

                    }
                    lastdist = Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position);
                }
                
                
            }

            
            
        }

        if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll"))
        {
            closestgrapple = null;
            closestenemy = null;
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
                grappletarget.transform.localScale= Vector3.one*0.5f;
            }
            
            grappletarget.transform.position = closestgrapple.transform.position;
            if (closestgrapple.GetComponent<EnemyHP>() != null)
            {
                grappletarget.transform.position += (Vector3)closestenemy.GetComponent<EnemyHP>().grappleOffset;
            }
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
                if(closestenemy.GetComponent<EnemyHP>().ismachine && closestenemy.GetComponent<EnemyHP>().enemyNRG<=0 && global.worldflags[14])
                {
                    enemytarget.GetComponent<SpriteRenderer>().sprite = EnemyHackTargetSprite;
                }
                else
                {
                    enemytarget.GetComponent<SpriteRenderer>().sprite = EnemyTargetSprite;
                }
                
                enemytarget.GetComponent<SpriteRenderer>().sortingOrder = 2;
                enemytarget.transform.localScale = Vector3.one * 0.5f;
            }
            enemytarget.transform.position = closestenemy.transform.position + (Vector3)closestenemy.GetComponent<EnemyHP>().grappleOffset;
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
        if (global.atsavepoint || global.indialogue || global.zipping || global.grappling || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll"))
        {
            return;
        }
        if (closestgrapple != null && pressedtrigger)
        {
            grapplingenemy = false;
            previousgrapple = closestgrapple.gameObject;
            global.grappling = true;
            TimeToThrowGrapplecounter = (int)(TimeToThrowGrapple / Time.fixedDeltaTime);
        }
    }

    private void OnAttack()
    {
        if (global.atsavepoint || global.indialogue || global.zipping || !GetComponent<PlayerJumpV3>().grounded || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll"))
        {
            return;
        }
        if (closestenemy != null && pressedtrigger)
        {
            grapplingenemy = true;
            previousgrapple = closestenemy.gameObject;
            global.grappling = true;
            TimeToThrowGrapplecounter = (int)(TimeToThrowGrapple / Time.fixedDeltaTime);
        }
    }

    private void OnNorthButton()
    {
        if (global.atsavepoint || global.indialogue || global.zipping || !GetComponent<PlayerJumpV3>().grounded || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll"))
        {
            return;
        }
        if (closestenemy != null && pressedtrigger && closestenemy.GetComponent<EnemyHP>().ismachine && closestenemy.GetComponent<EnemyHP>().enemyNRG <= 0 && global.worldflags[14])
        {
            grapplingenemy = true;
            previousgrapple = closestenemy.gameObject;
            global.grappling = true;
            hacking = true;
            TimeToThrowGrapplecounter = (int)(TimeToThrowGrapple / Time.fixedDeltaTime);
        }
    }

    void GenerateChain(Vector2 Start, Vector2 End)
    {
        foreach(GameObject chain in LastChain)
        {
            Destroy (chain);
        }
        Vector2 direction = End - Start;
        float distance = direction.magnitude;
        int numberOfLinks = Mathf.FloorToInt(distance / linkLength);
        Vector2 step = direction.normalized * linkLength;


        Vector3 offset = End - Start;

        Quaternion rotation = Quaternion.LookRotation(
                               Vector3.forward, // Keep z+ pointing straight into the screen.
                               offset           // Point y+ toward the next.
                             );

        for (int i = 0; i <= numberOfLinks; i++)
        {
            Vector2 position = Start + (step * i);
            GameObject newchain =Instantiate(ChainPrefab, position, rotation, transform);
            LastChain.Add(newchain);
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
        LayerMask newlayermask = (1 << 6) | (1 << 8) | (1 << 13);
        foreach (GameObject grapple in GrappleList)
        {
            if(Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position) < distance && previousgrapple!=grapple)
            {
                RaycastHit2D hitmiddle = Physics2D.Raycast(transform.position, grapple.transform.position- transform.position, Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position)-1 , newlayermask);
                RaycastHit2D hittop = Physics2D.Raycast(transform.position + new Vector3(0f,GetComponent<BoxCollider2D>().size.y * transform.localScale.y/1.9f,0f), grapple.transform.position - transform.position, Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position) - 1, newlayermask);
                RaycastHit2D hitbottom = Physics2D.Raycast(transform.position - new Vector3(0f, GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 1.9f, 0f), grapple.transform.position - transform.position, Vector2.Distance((Vector2)grapple.transform.position, (Vector2)transform.position) - 1, newlayermask);


                if ((hitmiddle.transform==null || hitmiddle.transform.gameObject==grapple) && (hittop.transform == null || hittop.transform.gameObject == grapple) && (hitbottom.transform == null || hitbottom.transform.gameObject == grapple))
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
        else
        {
            closestgrapple = null;
        }
    }


    private void GetClosestenemy()
    {
        Transform newclosestenemy = null;
        LayerMask newlayermask = (1 << 6) | (1 << 8) | (1 << 12) | (1 << 13);
        EnemyHP[] enemylist = FindObjectsByType<EnemyHP>(FindObjectsSortMode.None);
        float distance = 2f*5f;
        foreach (EnemyHP enemy in enemylist)
        {
            if (Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position) < distance && Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position) > 2.5f && previousgrapple != enemy)
            {

                RaycastHit2D hitmiddle = Physics2D.Raycast(transform.position, enemy.transform.position - transform.position, Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position) - 1, newlayermask);
                //RaycastHit2D hittop = Physics2D.Raycast(transform.position + new Vector3(0f, GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 1.9f, 0f), enemy.transform.position - transform.position, Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position) - 1, newlayermask);
                //RaycastHit2D hitbottom = Physics2D.Raycast(transform.position - new Vector3(0f, GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 1.9f, 0f), enemy.transform.position - transform.position, Vector2.Distance((Vector2)enemy.transform.position, (Vector2)transform.position) - 1, newlayermask);

                //if ((hitmiddle.transform == null || hitmiddle.transform.gameObject == enemy.gameObject) && (hittop.transform == null || hittop.transform.gameObject == enemy.gameObject) && (hitbottom.transform == null || hitbottom.transform.gameObject == enemy.gameObject))
                if (hitmiddle.transform == null || hitmiddle.transform.gameObject == enemy.gameObject)
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
