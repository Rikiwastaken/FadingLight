using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class EldonAttack : MonoBehaviour
{
    PlayerControls controls;

    private Animator eldonanim;

    //size of the attack 
    [Header("hitbox of the attack")]
    public Transform attackpoint;
    public float range;


    //attack variables
    [Header("attack variables")]
    public int nrgregen;
    public int hpdamage;
    public int nrgdamage;
    public bool slayermode; //true:slayer false:eater
    public float attackdelay; //time between attacks
    public int delaycounter;
    


    [Header("recoil variables")]
    private int size;
    private Rigidbody2D enemyrb;
    private float playerx;
    public float smallrecoil;
    public float flyingrecoil;

    public bool grounded;

    private PlayerJumpV3 playerjump;
    private EquipmentScript equipmentScript;


    // Start is called before the first frame update
    void Start()
    {
        eldonanim = GetComponent<Animator>();
        slayermode = true;
        GameObject.Find("slayertext").GetComponent<Image>().enabled = true;
        GameObject.Find("eatertext").GetComponent<Image>().enabled = false;
        playerjump = GetComponent<PlayerJumpV3>();
        equipmentScript = GetComponent<EquipmentScript>();
    }

    //Slayer and Eater modes

    void OnRightShoulder()
    {
        if (FindAnyObjectByType<Global>().atsavepoint|| FindAnyObjectByType<Global>().indialogue|| FindAnyObjectByType<Global>().zipping || FindAnyObjectByType<Global>().grappling || FindAnyObjectByType<Global>().inpause )
        {
            return;
        }
        Modeswitch();
    }

    void OnAttack()
    {
        if (FindAnyObjectByType<Global>().atsavepoint|| FindAnyObjectByType<Global>().indialogue|| FindAnyObjectByType<Global>().zipping || FindAnyObjectByType<Global>().grappling || GetComponent<GrappleScript>().pressedtrigger || FindAnyObjectByType<Global>().inpause || GetComponent<PlayerDodge>().airdodgelengthcnt > 0)
        {
            return;
        }
        if (delaycounter == 0 && !playerjump.stuckinwall)
            {
                fctAttack();
                delaycounter = (int)(attackdelay / Time.fixedDeltaTime);
            }
        
        
    }

    void FixedUpdate()
    {

        grounded = playerjump.grounded;
        //attack cooldown
        if (delaycounter>0)
        {
            delaycounter -= 1;
        }


    }


    void Modeswitch()
    {
        if(slayermode)
        {
            slayermode = false;
            GameObject.Find("slayertext").GetComponent<Image>().enabled = false;
            GameObject.Find("eatertext").GetComponent<Image>().enabled = true;
        }
        else
        {
            slayermode = true;
            GameObject.Find("slayertext").GetComponent<Image>().enabled = true;
            GameObject.Find("eatertext").GetComponent<Image>().enabled = false;
        }
    }


    void fctAttack()
    {
        if (!grounded && !playerjump.onennemi)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 12.5f);
        }
        
        //attack animation
        eldonanim.SetTrigger("attack");

        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.position,range);

        int damage = hpdamage;
        int energydamage =nrgdamage;
        float absorbrate = 3f / 10f;
        if (equipmentScript.equipedChainIndex!=-1)
        {
            damage = (int)(damage * equipmentScript.Chainslist[equipmentScript.equipedChainIndex].DamageMultiplier);
            energydamage = (int)(energydamage * equipmentScript.Chainslist[equipmentScript.equipedChainIndex].DamageMultiplier);
            absorbrate = absorbrate * equipmentScript.Chainslist[equipmentScript.equipedChainIndex].AbsorbMultiplier;
        }

        foreach (Collider2D enemy in hitenemies)
        {
            
            if (enemy.tag == "enemy")
            {
                EnemyHP enemyHP = enemy.GetComponent<EnemyHP>();
                enemyrb = enemy.GetComponent<Rigidbody2D>();
                playerx = GetComponent<Rigidbody2D>().position.x;

                if (slayermode)
                {
                    enemyHP.TakeDamage(damage, (energydamage * 1 / 10));
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate ;
                    if(GetComponent<PlayerHP>().EldonNRG> GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                }
                else
                {
                    enemyHP.TakeDamage(damage*1/10, energydamage);
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate*2;
                    if (GetComponent<PlayerHP>().EldonNRG > GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                }
                if (enemyHP.enemyhp > 0)
                {
                    int direction = (int)((enemyrb.position.x - playerx) / Mathf.Abs(enemyrb.position.x - playerx));
                    if(enemyHP.isflying)
                    {
                        enemyrb.AddForce(new Vector2(direction * flyingrecoil, 0), ForceMode2D.Impulse);
                    }
                    else
                    {
                        enemyrb.AddForce(new Vector2(direction * smallrecoil, 0), ForceMode2D.Impulse);
                    }
                   
                    
                }
            }

            else if (enemy.tag == "Boss")
            {
                EnemyHP enemyHP = enemy.GetComponent<EnemyHP>();

                if (slayermode)
                {
                    enemyHP.TakeDamage(damage, (energydamage * 1 / 10));
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate;
                    if (GetComponent<PlayerHP>().EldonNRG > GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                }
                else
                {
                    enemyHP.TakeDamage(damage * 1 / 10, energydamage);
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate * 2;
                    if (GetComponent<PlayerHP>().EldonNRG > GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                }
            }


        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackpoint.position, range);
    }
}
