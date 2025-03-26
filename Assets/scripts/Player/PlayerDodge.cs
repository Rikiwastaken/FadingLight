using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{

    private Animator anim;

    public float dodgecd;
    public int dodgecdcnt;
    private bool replaceennemy;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //dodge cooldown
        if (dodgecdcnt > 0)
        {
            dodgecdcnt -= 1;
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

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("roll") && replaceennemy)
        {
            replaceennemy = false;
            Replaceenemies();
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
                    Debug.Log(max);

                }
            }
        }
    }

}
