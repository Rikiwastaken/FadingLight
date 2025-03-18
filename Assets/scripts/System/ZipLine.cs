using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZipLine : MonoBehaviour
{

    public bool start;
    public bool end;
    public ZipLine next;
    public ZipLine previous;
    public Sprite ConnectorSprite;
    public Vector2 Offset;
    private Global global;
    public Transform player;
    public float speed;
    public bool reversed;
    private bool pressingtrigger;
    private GrappleScript GrappleScript;

    private void Awake()
    {
        if (!end && !start)
        {
            transform.tag = "Untagged";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        global = FindAnyObjectByType<Global>();
        GrappleScript=FindAnyObjectByType<GrappleScript>();
        

        if (next != null)
        {
            GameObject connector = new GameObject();
            connector.AddComponent<SpriteRenderer>();
            connector.GetComponent<SpriteRenderer>().sortingOrder=0;
            connector.GetComponent<SpriteRenderer>().sprite = ConnectorSprite;
            connector.transform.position = transform.position + (next.transform.position - transform.position) / 2f;
            Vector3 offset = next.transform.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the next.
                                 );
            connector.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            connector.transform.localScale = new Vector3(Vector2.Distance((Vector2)transform.position, (Vector2)next.transform.position), 0.2f, 1f);
            connector.transform.parent = transform;
        }
    }

    private void FixedUpdate()
    {
        pressingtrigger = GrappleScript.pressedtrigger;

        if(!global.zipping)
        {
            player = null;
        }
        if(player != null)
        {
            if(!reversed)
            {
                player.GetComponent<Rigidbody2D>().velocity = (next.transform.position - player.transform.position - (Vector3)Offset).normalized * speed;
            }
            else
            {
                player.GetComponent<Rigidbody2D>().velocity = (previous.transform.position - player.transform.position - (Vector3)Offset).normalized * speed;
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(pressingtrigger && !global.zipping && collision.transform.GetComponent<PlayerHP>()!=null)
        {
            if(start)
            {
                spreadunreversedtonext(this);
                global.zipping = true;
                collision.transform.position = transform.position - (Vector3)Offset;
                player = collision.transform;
            }
            if(end)
            {
                spreadreversedtoprevious(this);
                global.zipping = true;
                collision.transform.position = transform.position - (Vector3)Offset;
                player = collision.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.GetComponent<PlayerHP>()!=null)
        {
            if (start)
            {
                if(!global.zipping)
                {
                    if(pressingtrigger)
                    {
                        spreadunreversedtonext(this);
                        global.zipping = true;
                        collision.transform.position = transform.position - (Vector3)Offset;
                        player = collision.transform;
                    }
                }
                else
                {
                    next.player = null;
                    global.zipping = false;
                }
                
            }
            else if(end)
            {
                if(!global.zipping)
                {
                    if(pressingtrigger)
                    {
                        spreadreversedtoprevious(this);
                        global.zipping = true;
                        collision.transform.position = transform.position - (Vector3)Offset;
                        player = collision.transform;
                    }
                    
                }
                else
                {
                    previous.player = null;
                    global.zipping = false;
                }
                
            }
            else if(previous.player == collision.transform && !reversed)
            {
                player = collision.transform;
                previous.player = null;
            }
            else if(next.player == collision.transform && reversed)
            {
                player = collision.transform;
                next.player = null;
            }
        }
    }
    void spreadreversedtoprevious(ZipLine currentzip)
    {
        currentzip.reversed = true;
        if(currentzip.previous!=null)
        {
            spreadreversedtoprevious(currentzip.previous);
        }
    }

    void spreadunreversedtonext(ZipLine currentzip)
    {
        currentzip.reversed = false;
        if (currentzip.next != null)
        {
            spreadunreversedtonext(currentzip.next);
        }
    }
}
