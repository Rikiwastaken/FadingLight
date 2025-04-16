using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipmentScript;

public class GadgetScript : MonoBehaviour
{
    [System.Serializable]
    public class Gadget
    {
        public string name;
        public string description;
        public float DamageMultiplier;
        public float AbsorbMultiplier;
        public int Energycost;
        public float cooldown;
        public GameObject PrefabtoSpawn;
        public int ID;
        public Sprite image;
        public bool locked;
    }

    public List<Gadget> GadgetList;

    public int ActiveGadgetID;

    private Healthbar healthbar;

    public int gadgetCDcounter;

    private PlayerHP PlayerHP;

    public float projectileoffset;

    private float CDmultiplier;

    private AugmentsScript augmentsScript;
    private PlayerJumpV3 playerjump;

    public int invisibilityFrames;

    private void Start()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        PlayerHP = GetComponent<PlayerHP>();
        augmentsScript = GetComponent<AugmentsScript>();
        playerjump = GetComponent<PlayerJumpV3>();
    }
    private void FixedUpdate()
    {
        if((FindAnyObjectByType<Global>().grappling|| FindAnyObjectByType<Global>().atsavepoint || FindAnyObjectByType<Global>().zipping || FindAnyObjectByType<Global>().indialogue) && invisibilityFrames>0)
        {
            invisibilityFrames = 1;
        }
        if(invisibilityFrames>0)
        {
            invisibilityFrames--;
            if(invisibilityFrames == 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.75f);
            }
            
        }

        CDmultiplier = 1.0f;
        if (augmentsScript.EquipedAugments[8])
        {
            CDmultiplier = 0.8f;
        }

        if (GadgetList[ActiveGadgetID].cooldown!=0)
        {
            healthbar.SetMaxGadget(GadgetList[ActiveGadgetID].cooldown * 60);
            healthbar.SetGadgetCD(gadgetCDcounter);
        }
        else
        {
            healthbar.SetMaxGadget(1f);
            healthbar.SetGadgetCD(0f);
        }
        


        if (gadgetCDcounter > 0)
        {
            gadgetCDcounter--;
        }

        healthbar.gadgetusable = PlayerHP.EldonNRG >= GadgetList[ActiveGadgetID].Energycost;

    }

    void OnLeftShoulder()
    {
        if (FindAnyObjectByType<Global>().atsavepoint || FindAnyObjectByType<Global>().indialogue || FindAnyObjectByType<Global>().ininventory || gadgetCDcounter > 0 || FindAnyObjectByType<Global>().grappling || FindAnyObjectByType<Global>().zipping|| FindAnyObjectByType<Global>().inpause || playerjump.stuckinwall)
        {
            return;
        }
        UseGadget();
    }

    private void UseGadget()
    {
        Gadget ActiveGadget = GadgetList[ActiveGadgetID];

        if(PlayerHP.EldonNRG>= ActiveGadget.Energycost && gadgetCDcounter==0)
        {
            PlayerHP.EldonNRG -= ActiveGadget.Energycost;

            gadgetCDcounter = (int)(CDmultiplier*ActiveGadget.cooldown*60);
            healthbar.SetMaxGadget(ActiveGadget.cooldown*60);

            if (ActiveGadget.PrefabtoSpawn!=null)
            {
                Spawnprefab(ActiveGadget);
            }

            if(ActiveGadgetID==7)
            {
                invisibilityFrames=(int)(5/Time.fixedDeltaTime);
            }

        }
    }

    private void Spawnprefab(Gadget gadget)
    {

        int direction = 0;
        if(GetComponent<SpriteRenderer>().flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + new Vector2(direction * (0.1f + GetComponent<BoxCollider2D>().size.x / 1.5f), 0f), 0.15f);
        bool wallinfront = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("wall") || collider.gameObject.layer == LayerMask.NameToLayer("ground"))
            {
                wallinfront = true;
            }
        }
        
        
        Vector3 offset = new Vector3(direction, 0f,0f)*projectileoffset;

        if (wallinfront && gadget.PrefabtoSpawn.GetComponent<GrenadeScript>())
        {
            offset = new Vector3(0f, 2.5f, 0f);
        }
        if(gadget.PrefabtoSpawn.GetComponent<ShockwaveScript>())
        {
            offset=Vector3.zero;
        }
        if(gadget.PrefabtoSpawn.transform.childCount>0)
        {
            if(gadget.PrefabtoSpawn.transform.GetChild(0).GetComponent<MaceGadget>())
            {
                offset = Vector3.zero;
            }
        }
        if(wallinfront && gadget.PrefabtoSpawn.GetComponent<RocketScript>())
        {
            gadgetCDcounter = 0;
            PlayerHP.EldonNRG += gadget.Energycost;
            if(PlayerHP.EldonNRG>PlayerHP.EldonmaxNRG)
            {
                PlayerHP.EldonNRG = PlayerHP.EldonmaxNRG;
            }
            return;
        }
        //GameObject projectile = Instantiate(gadget.PrefabtoSpawn, transform.position + offset, Quaternion.identity);
        GameObject projectile = Instantiate(gadget.PrefabtoSpawn, GetComponent<Rigidbody2D>().position, Quaternion.identity);
        BulletScript bulletScript = projectile.GetComponent<BulletScript>();
        RocketScript rocketScript = projectile.GetComponent<RocketScript>();
        GrenadeScript GrenadeScript = projectile.GetComponent<GrenadeScript>();
        TurretScript turretScript = projectile.GetComponent<TurretScript>();
        MaceGadget macegadget = null;
        if (projectile.transform.childCount > 0)
        {
            if (projectile.transform.GetChild(0).GetComponent<MaceGadget>())
            {
                macegadget = projectile.transform.GetChild(0).GetComponent<MaceGadget>();
            }
        }
        if (bulletScript!=null)
        {
            bulletScript.damage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage * gadget.DamageMultiplier);
            bulletScript.EnergyDamage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.NRJDamage * gadget.DamageMultiplier);
            bulletScript.direction = direction;
            bulletScript.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (rocketScript != null)
        {
            rocketScript.damage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage * gadget.DamageMultiplier);
            rocketScript.Energydamage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.NRJDamage * gadget.DamageMultiplier);
            rocketScript.basedirection = direction;
            rocketScript.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (GrenadeScript != null)
        {
            if(gadget.ID ==4)
            {
                GrenadeScript.damage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage * gadget.DamageMultiplier);
            }
            else if(gadget.ID ==3)
            {
                GrenadeScript.energydamage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.NRJDamage * gadget.DamageMultiplier);
            }
            Vector3 forcetoapply = new Vector3(direction * 5f, 1f, 0f)*10f;
            if (wallinfront)
            {
                forcetoapply = new Vector3(0f, 0f, 0f);
            }
            GrenadeScript.GetComponent<Rigidbody2D>().AddForce(forcetoapply,ForceMode2D.Impulse);
            GrenadeScript.transform.localScale =new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (turretScript != null)
        {
            Vector3 forcetoapply = new Vector3(direction * 2f, 2f, 0f)*10f;
            if (wallinfront)
            {
                forcetoapply = new Vector3(0f, 0f, 0f);
            }
            turretScript.transform.localScale = new Vector3(direction * 1.75f, 1.75f, 1.75f);
            turretScript.damage= (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage * gadget.DamageMultiplier);
            turretScript.energydamage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.NRJDamage * gadget.DamageMultiplier);
            turretScript.GetComponent<Rigidbody2D>().AddForce(forcetoapply, ForceMode2D.Impulse);
        }
        else if(macegadget != null)
        {
            
            macegadget.transform.parent.SetParent(transform);
            macegadget.transform.parent.localPosition = GetComponent<Collider2D>().offset;
            macegadget.damage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage * gadget.DamageMultiplier);

        }
    }

}
