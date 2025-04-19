using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static GadgetScript;

public class PlayerDodge : MonoBehaviour
{

    private Animator anim;

    private bool grounded;

    private float gravity;
    public GameObject Grenade;
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
            if (FindAnyObjectByType<Global>().inmenu_inv_shop || FindAnyObjectByType<Global>().zipping || FindAnyObjectByType<Global>().grappling)
            {
                return;
            }

        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("roll") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && dodgecdcnt == 0 && GetComponent<PlayerJumpV3>().grounded)
        {
            if (GetComponent<AugmentsScript>().EquipedAugments[10])
            {
                GameObject grenade = Instantiate(Grenade, transform.position, Quaternion.identity);
                GrenadeScript GrenadeScript = grenade.GetComponent<GrenadeScript>();
                GrenadeScript.damage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage / 3f);
                GrenadeScript.energydamage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.NRJDamage / 3f);
                GrenadeScript.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            }
                anim.SetTrigger("dodge");
            dodgecdcnt = (int)(dodgecd / Time.fixedDeltaTime);
        }

        

        if(!grounded && !usedairdodge && airdodgelengthcnt==0)
        {
            usedairdodge = true;
            airdodgelengthcnt = (int)(airdodgelength/Time.fixedDeltaTime);
            if(GetComponent<SpriteRenderer>().flipX)
            {
                airdodgedirection = -1;
            }
            else
            {
                airdodgedirection = 1;

            }
            
            GetComponent<Rigidbody2D>().velocity = new Vector2(airdodgedirection * airdodgespeed / 2, 0);
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }

    }


    public void Replaceenemies()
    {


        int safetyCounter = 0;
        const int maxIterations = 30;

        while (safetyCounter < maxIterations)
        {
            bool anyStillOverlapping = false;
            Collider2D[] allcolliders = Physics2D.OverlapBoxAll(
                GetComponent<BoxCollider2D>().bounds.center,
                GetComponent<BoxCollider2D>().bounds.size * 1.1f,
                0f);

            foreach (Collider2D collider in allcolliders)
            {
                if (collider != null && collider.GetComponent<EnemyHP>() && !collider.GetComponent<EnemyHP>().isboss)
                {
                    anyStillOverlapping = true;
                    int direction = 0;
                    if (collider.transform.position.x<transform.position.x) direction = -1;
                    if (collider.transform.position.x > transform.position.x) direction = 1;
                    if (direction == 0)
                    {
                        // If no wall direction is detected, just push enemy slightly upward
                        collider.transform.position += new Vector3(0f, 0.05f, 0f);
                    }
                    else
                    {
                        collider.transform.position += new Vector3(
                            0.5f * direction,
                            0f,
                            0f);
                    }
                    Physics2D.SyncTransforms();
                }
            }

            if (!anyStillOverlapping)
            {
                break;
            }

            safetyCounter++;
        }

        if (safetyCounter == maxIterations)
        {
            Debug.LogWarning("Replaceenemies: Max iterations reached � some enemies may still be inside the collider.");
        }
    }

}
