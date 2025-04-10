using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{

    private Animator anim;

    private bool grounded;

    private bool replaceennemy;
    private float gravity;

    [Header("Ground Dodge")]
    public float dodgecd;
    public int dodgecdcnt;
    

    [Header("Air Dodge")]
    private bool usedairdodge;
    public int airdodgelengthcnt;
    public float airdodgelength;
    private int airdodgedirection;
    public float airdodgespeed;
    public bool resetairdash;

    // Start is called before the first frame update
    void Start()
    {
        gravity = GetComponent<Rigidbody2D>().gravityScale;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = GetComponent<PlayerJumpV3>().grounded;
        //dodge cooldown
        if (dodgecdcnt > 0)
        {
            dodgecdcnt -= 1;
        }

        if (airdodgelengthcnt > 0)
        {
            if(Mathf.Abs(GetComponent<Rigidbody2D>().velocityX)<=0.5f)
            {
                airdodgelengthcnt = 0;
                GetComponent<Rigidbody2D>().gravityScale = gravity;
                return;
            }
            airdodgelengthcnt -= 1;
            
            GetComponent<Rigidbody2D>().gravityScale = 0;
            if(airdodgelengthcnt==0)
            {
                GetComponent<Rigidbody2D>().gravityScale = gravity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(airdodgedirection*airdodgespeed / 2, 0);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(airdodgespeed * airdodgedirection, 0);
            }
        }

        if (grounded || GetComponent<PlayerJumpV3>().stuckinwall || resetairdash)
        {
            resetairdash = false;
            usedairdodge = false;
            if(airdodgelengthcnt>0)
            {
                airdodgelengthcnt = 0;
                GetComponent<Rigidbody2D>().gravityScale = gravity;
            }
            
        }
        else
        {
            dodgecdcnt = 0;
        }
    }

    void OnDodge()
    {
        if(FindAnyObjectByType<Global>())
        {
            if(FindAnyObjectByType<Global>().closedmenu)
            {
                FindAnyObjectByType<Global>().closedmenu = false;
                return;
            }
            if (FindAnyObjectByType<Global>().atsavepoint|| FindAnyObjectByType<Global>().indialogue || FindAnyObjectByType<Global>().ininventory || FindAnyObjectByType<Global>().zipping || FindAnyObjectByType<Global>().grappling|| FindAnyObjectByType<Global>().inpause)
            {
                return;
            }

        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("roll") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && dodgecdcnt == 0 && GetComponent<PlayerJumpV3>().grounded)
        {
            anim.SetTrigger("dodge");
            dodgecdcnt = (int)(dodgecd / Time.fixedDeltaTime);
            replaceennemy = true;
        }

        if ((!anim.GetCurrentAnimatorStateInfo(0).IsName("roll")|| airdodgelengthcnt==0) && replaceennemy)
        {
            replaceennemy = false;
            Replaceenemies();
        }

        if(!grounded && !usedairdodge && airdodgelengthcnt==0)
        {
            usedairdodge = true;
            airdodgelengthcnt = (int)(airdodgelength/Time.fixedDeltaTime);
            airdodgedirection = (int)(transform.localScale.x/Mathf.Abs(transform.localScale.x));
            GetComponent<Rigidbody2D>().gravityScale = 0;
            replaceennemy = true;
        }

    }


    void Replaceenemies()
    {
        bool wallleft = false;
        bool wallright = false;
        RaycastHit2D[] allcollisions = Physics2D.BoxCastAll((Vector2)transform.position + GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size, 0f, Vector2.zero);
        foreach(RaycastHit2D collision in allcollisions)
        {
            if((collision.transform.gameObject.layer==LayerMask.NameToLayer("ground") || collision.transform.gameObject.layer == LayerMask.NameToLayer("wall")) && collision.point.y>transform.position.y- GetComponent<BoxCollider2D>().size.y)
            {
                if(collision.point.x<transform.position.x)
                {
                    wallleft = true;
                }
                if (collision.point.x > transform.position.x)
                {
                    wallright = true;
                }
            }
        }
        Collider2D[] allcolliders = Physics2D.OverlapBoxAll((Vector2)transform.position + GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size, 0f);
        foreach (Collider2D collider in allcolliders)
        {
            if (collider.GetComponent<EnemyHP>())
            {
                if (!collider.GetComponent<EnemyHP>().isboss && !!collider.GetComponent<EnemyHP>().isbig)
                {
                    int direction = 0;
                    if (collider.transform.position.x < transform.position.x && !wallleft)
                    {
                        direction = -1;
                    }
                    if(collider.transform.position.x > transform.position.x && !wallright)
                    {
                        direction = 1;
                    }
                    int max = 0;
                    while (Physics2D.OverlapBoxAll((Vector2)transform.position + GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size, 0f).Contains(collider) && max <= 9999)
                    {
                        max++;
                        if(direction == 0)
                        {
                            collider.transform.position += new Vector3(0.01f * direction, 0.01f, 0f);
                        }
                        else
                        {
                            collider.transform.position += new Vector3(0.01f * direction, 0f, 0f);
                        }
                        
                    }

                }
            }
        }
    }

}
