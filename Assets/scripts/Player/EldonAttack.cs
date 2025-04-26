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
        if (FindAnyObjectByType<Global>().inmenu_inv_shop || FindAnyObjectByType<Global>().zipping || FindAnyObjectByType<Global>().grappling )
        {
            return;
        }
        Modeswitch();
    }

    void OnAttack()
    {
        if (FindAnyObjectByType<Global>().inmenu_inv_shop || FindAnyObjectByType<Global>().zipping || FindAnyObjectByType<Global>().grappling || GetComponent<GrappleScript>().pressedtrigger ||  GetComponent<PlayerDodge>().airdodgelengthcnt > 0)
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
        int direction = 0;
        if (GetComponent<SpriteRenderer>().flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        attackpoint.transform.localPosition = new Vector2(direction * Mathf.Abs(attackpoint.transform.localPosition.x), attackpoint.transform.localPosition.y);

    }


    public void Modeswitch()
    {
        if(!GetComponent<ChainSawMode>().chainsawmode)
        {
            if (slayermode)
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
    }

    public void Modeswitch(bool modetoactivate)
    {
        if (modetoactivate)
        {
            slayermode = true;
            GameObject.Find("slayertext").GetComponent<Image>().enabled = true;
            GameObject.Find("eatertext").GetComponent<Image>().enabled = false;
        }
        else
        {
            slayermode = false;
            GameObject.Find("slayertext").GetComponent<Image>().enabled = false;
            GameObject.Find("eatertext").GetComponent<Image>().enabled = true;
        }
    }


    void fctAttack()
    {
        if(FindAnyObjectByType<GadgetScript>().invisibilityFrames >0)
        {
            FindAnyObjectByType<GadgetScript>().invisibilityFrames = 1;
        }
        

        if (!grounded && !playerjump.onennemi)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 12.5f);
        }
        
        //attack animation
        eldonanim.SetTrigger("attack");

        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.position,range);

        int damage = hpdamage;
        float energydamage =nrgdamage;
        float absorbrate = 3f / 10f;
        float damagewithchainsaw = hpdamage;
        float energydamagewithchainsaw = nrgdamage;
        if (GetComponent<ChainSawMode>().chainsawmode)
        {
            damagewithchainsaw *= GetComponent<ChainSawMode>().chainsawdamageMultiplier;
            energydamagewithchainsaw *= GetComponent<ChainSawMode>().chainsawdamageMultiplier;
            absorbrate /= GetComponent<ChainSawMode>().chainsawdamageMultiplier;
            if (GetComponent<AugmentsScript>().EquipedAugments[15])
            {
                damagewithchainsaw *= GetComponent<AugmentsScript>().Augmentlist[15].valueincr;
                energydamagewithchainsaw *= GetComponent<AugmentsScript>().Augmentlist[15].valueincr;
                absorbrate /= GetComponent<AugmentsScript>().Augmentlist[15].valueincr;
            }
        }
        if (equipmentScript.equipedChainIndex!=-1)
        {
            damage = (int)(damagewithchainsaw * equipmentScript.Chainslist[equipmentScript.equipedChainIndex].DamageMultiplier);
            energydamage = (energydamagewithchainsaw * equipmentScript.Chainslist[equipmentScript.equipedChainIndex].DamageMultiplier);
            absorbrate = absorbrate * equipmentScript.Chainslist[equipmentScript.equipedChainIndex].AbsorbMultiplier;
            if(GetComponent<AugmentsScript>().EquipedAugments[6])
            {
                absorbrate*= GetComponent<AugmentsScript>().Augmentlist[6].valueincr;
            }
        }

        foreach (Collider2D enemy in hitenemies)
        {
            
            if (enemy.tag == "enemy")
            {
                Debug.Log(enemy.name);
                EnemyHP enemyHP = enemy.GetComponent<EnemyHP>();
                enemyrb = enemy.GetComponent<Rigidbody2D>();
                playerx = GetComponent<Rigidbody2D>().position.x;

                if (slayermode)
                {
                    enemyHP.TakeDamage(damage, (int)(energydamage * 0.1f));
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate ;
                    if(GetComponent<PlayerHP>().EldonNRG> GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                    if (GetComponent<AugmentsScript>().EquipedAugments[12])
                    {
                        GetComponent<PlayerHP>().Eldonhp += (int)(damage * GetComponent<AugmentsScript>().Augmentlist[12].valueincr);
                        if(GetComponent<PlayerHP>().Eldonhp> GetComponent<PlayerHP>().Eldonmaxhp)
                        {
                            GetComponent<PlayerHP>().Eldonhp = GetComponent<PlayerHP>().Eldonmaxhp;
                        }
                    }
                }
                else
                {
                    enemyHP.TakeDamage((int)(damage * 0.1f), (int)energydamage);
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate*2;
                    if (GetComponent<PlayerHP>().EldonNRG > GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                    if (GetComponent<AugmentsScript>().EquipedAugments[12])
                    {
                        GetComponent<PlayerHP>().Eldonhp += (int)(damage * 0.1f* GetComponent<AugmentsScript>().Augmentlist[12].valueincr);
                        if (GetComponent<PlayerHP>().Eldonhp > GetComponent<PlayerHP>().Eldonmaxhp)
                        {
                            GetComponent<PlayerHP>().Eldonhp = GetComponent<PlayerHP>().Eldonmaxhp;
                        }
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
                    enemyHP.TakeDamage(damage, (int)(energydamage * 0.1f));
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate;
                    if (GetComponent<PlayerHP>().EldonNRG > GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                    if (GetComponent<AugmentsScript>().EquipedAugments[12])
                    {
                        GetComponent<PlayerHP>().Eldonhp += (int)(damage * 0.01f);
                        if (GetComponent<PlayerHP>().Eldonhp > GetComponent<PlayerHP>().Eldonmaxhp)
                        {
                            GetComponent<PlayerHP>().Eldonhp = GetComponent<PlayerHP>().Eldonmaxhp;
                        }
                    }
                }
                else
                {
                    enemyHP.TakeDamage((int)(damage * 0.1f), (int)energydamage);
                    GetComponent<PlayerHP>().EldonNRG += energydamage * absorbrate * 2;
                    if (GetComponent<PlayerHP>().EldonNRG > GetComponent<PlayerHP>().EldonmaxNRG)
                    {
                        GetComponent<PlayerHP>().EldonNRG = GetComponent<PlayerHP>().EldonmaxNRG;
                    }
                }
                if (GetComponent<AugmentsScript>().EquipedAugments[12])
                {
                    GetComponent<PlayerHP>().Eldonhp += (int)(damage * 0.1f * 0.01f);
                    if (GetComponent<PlayerHP>().Eldonhp > GetComponent<PlayerHP>().Eldonmaxhp)
                    {
                        GetComponent<PlayerHP>().Eldonhp = GetComponent<PlayerHP>().Eldonmaxhp;
                    }
                }
            }
            else if(enemy.GetComponent<Interruptor>())
            {
                enemy.GetComponent<Interruptor>().InterractWithInterruptor();
            }


        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackpoint.position, range);
    }
}
