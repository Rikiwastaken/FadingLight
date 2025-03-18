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

    private int gadgetCDcounter;

    private PlayerHP PlayerHP;

    public float projectileoffset;

    private float CDmultiplier;

    private AugmentsScript augmentsScript;
    private PlayerJumpV3 playerjump;

    private void Start()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        PlayerHP = GetComponent<PlayerHP>();
        augmentsScript = GetComponent<AugmentsScript>();
        playerjump = GetComponent<PlayerJumpV3>();
    }
    private void FixedUpdate()
    {
        CDmultiplier = 1.0f;
        if (augmentsScript.EquipedAugments[8])
        {
            CDmultiplier = 0.8f;
        }

        if (GadgetList[ActiveGadgetID].cooldown!=0)
        {
            healthbar.SetMaxGadget(GadgetList[ActiveGadgetID].cooldown / Time.deltaTime);
            healthbar.SetGadgetCD(gadgetCDcounter);
            //Debug.Log(gadgetCDcounter + "   " + GadgetList[ActiveGadgetID].cooldown / Time.deltaTime);
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

            gadgetCDcounter = (int)(CDmultiplier*ActiveGadget.cooldown / Time.deltaTime);
            healthbar.SetMaxGadget(ActiveGadget.cooldown / Time.deltaTime);

            if (ActiveGadget.PrefabtoSpawn!=null)
            {
                Spawnprefab(ActiveGadget);
            }

        }
    }

    private void Spawnprefab(Gadget gadget)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + new Vector2(transform.localScale.x / Mathf.Abs(transform.localScale.x) * (0.1f + GetComponent<BoxCollider2D>().size.x / 1.5f), 0f), 0.15f);
        bool wallinfront = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("wall") || collider.gameObject.layer == LayerMask.NameToLayer("ground"))
            {
                wallinfront = true;
            }
        }
        
        
        Vector3 offset = new Vector3(transform.localScale.x / Mathf.Abs(transform.localScale.x), 0f,0f)*projectileoffset;

        if (wallinfront && gadget.PrefabtoSpawn.GetComponent<GrenadeScript>())
        {
            offset = new Vector3(0f, 0.5f, 0f);
        }
        if(gadget.PrefabtoSpawn.GetComponent<ShockwaveScript>())
        {
            offset=Vector3.zero;
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
        GameObject projectile = Instantiate(gadget.PrefabtoSpawn, transform.position + offset, Quaternion.identity);
        BulletScript bulletScript = projectile.GetComponent<BulletScript>();
        RocketScript rocketScript = projectile.GetComponent<RocketScript>();
        GrenadeScript GrenadeScript = projectile.GetComponent<GrenadeScript>();
        if (bulletScript!=null)
        {
            bulletScript.damage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage * gadget.DamageMultiplier);
            bulletScript.EnergyDamage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.NRJDamage * gadget.DamageMultiplier);
            bulletScript.direction = (int)(transform.localScale.x/Mathf.Abs(transform.localScale.x));
            bulletScript.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        else if (rocketScript != null)
        {
            rocketScript.damage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.Damage * gadget.DamageMultiplier);
            rocketScript.Energydamage = (int)Mathf.Round(GetComponent<AugmentsScript>().EquipedStats.NRJDamage * gadget.DamageMultiplier);
            rocketScript.basedirection = (int)(transform.localScale.x / Mathf.Abs(transform.localScale.x));
            rocketScript.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
            Vector3 forcetoapply = new Vector3(transform.localScale.x / Mathf.Abs(transform.localScale.x) * 3f, 1f, 0f);
            if (wallinfront)
            {
                forcetoapply = new Vector3(0f, 0f, 0f);
            }
            Debug.Log(forcetoapply);
            GrenadeScript.GetComponent<Rigidbody2D>().AddForce(forcetoapply,ForceMode2D.Impulse);
            GrenadeScript.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere((Vector2)transform.position + new Vector2(transform.localScale.x / Mathf.Abs(transform.localScale.x) * (0.1f + GetComponent<BoxCollider2D>().size.x / 1.5f), 0f), 0.15f);
    }

}
