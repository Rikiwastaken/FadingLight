using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{

    PlayerControls controls;
    public bool pressedtrigger;

    private GameObject[] GrappleList;

    public Transform closestgrapple;

    public float mindist;

    public Sprite CableSprite;
    private GameObject cable;
    private Global global;
    public float speed;

    private int grapplecooldown;

    public Vector2 launch;

    public GameObject previousgrapple;
    private void Awake()
    {
        controls = new PlayerControls();

        controls.gameplay.LeftTrigger.performed += ctx => pressedtrigger = true;
        controls.gameplay.LeftTrigger.canceled += ctx => pressedtrigger = false;
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


        if(global.grappling)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }


        if(!global.grappling && grapplecooldown<=0)
        {
            GetClosestgrapple();
        }
        else if(grapplecooldown>0)
        {
            grapplecooldown--;
        }

        if(closestgrapple != null)
        {
            if (Vector2.Distance(transform.position, closestgrapple.position) > mindist)
            {
                closestgrapple = null;
            }
        }
        
        if((!global.grappling && cable != null) || closestgrapple==null)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            global.grappling = false;
            Destroy(cable);
        }
        else if(global.grappling && cable!=null && closestgrapple!=null)
        {
            if (Vector2.Distance((Vector2)closestgrapple.transform.position, (Vector2)transform.position) <= 0.3f)
            {
                Destroy(cable);
                global.grappling = false;
                grapplecooldown = (int)(0.2f / Time.deltaTime);
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<Rigidbody2D>().velocity=Vector2.zero;
                GetComponent<Rigidbody2D>().AddForce(launch, ForceMode2D.Impulse);
                return;
            }
            GetComponent<Rigidbody2D>().velocity = (closestgrapple.transform.position - transform.position).normalized * speed;
            cable.transform.position = transform.position + (closestgrapple.transform.position - transform.position) / 2f;
            Vector3 offset = closestgrapple.transform.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the next.
                                 );
            cable.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            cable.transform.localScale = new Vector3(Vector2.Distance((Vector2)transform.position, (Vector2)closestgrapple.transform.position), 0.2f, 1f);
            
        }

        

    }

    private void OnLeftTrigger()
    {
        if(closestgrapple != null)
        {
            previousgrapple = closestgrapple.gameObject;
            global.grappling = true;
            if(cable !=null)
            {
                Destroy(cable);
            }
            cable = new GameObject();
            cable.AddComponent<SpriteRenderer>();
            cable.GetComponent<SpriteRenderer>().sprite = CableSprite;
            cable.transform.position = transform.position + (closestgrapple.transform.position - transform.position) / 2f;
            Vector3 offset = closestgrapple.transform.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the next.
                                 );
            cable.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            cable.transform.localScale = new Vector3(Vector2.Distance((Vector2)transform.position, (Vector2)closestgrapple.transform.position), 0.2f, 1f);
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
            grapple.transform.GetChild(0).gameObject.SetActive(false);
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
            newclosestgrapple.GetChild(0).gameObject.SetActive(true);
            closestgrapple = newclosestgrapple;
        }
    }

}
