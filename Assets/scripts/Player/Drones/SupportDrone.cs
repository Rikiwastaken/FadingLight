using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class SupportDrone : MonoBehaviour
{

    [System.Serializable]
    public class HealerDrone
    {
        public string name;
        public int ID;
        public int RequiredEnergy;
        public float Effect; //also counts as damage
        public string Description;
        public float cooldown;
        public bool Medical;
        public GameObject Summon;
        public Sprite Sprite;
        public bool locked;
    }

    public int ActiveDroneID; //-1 : No Drone, 0 : EnergyConverter Drone, 1 : Energy Regen Drone, 2 Healer Drone, 3 Rocket Drone, 4 Leech Drone

    private PlayerHP playerhp;
    private AugmentsScript augmentsscript;
    private EquipmentScript EquipmentScript;

    public int dronecd;

    public List<HealerDrone> drones;

    public RuntimeAnimatorController MedController;
    public RuntimeAnimatorController GunController;

    // Start is called before the first frame update
    void Start()
    {
        playerhp = GameObject.FindAnyObjectByType<PlayerHP>();
        augmentsscript = FindAnyObjectByType<AugmentsScript>();
        EquipmentScript = FindAnyObjectByType<EquipmentScript>();
        ChangeSprite();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChangeSprite();
        if(dronecd>0)
        {
            dronecd--;
        }
        else
        {
            if (ActiveDroneID > -1)
            {
                if (playerhp.EldonNRG >= drones[ActiveDroneID].RequiredEnergy)
                {

                    switch(ActiveDroneID)
                    {
                        case 0:
                            EnergyConverterDroneeffect();
                            break;
                        case 1:
                            EnergyRegenDroneeffect();
                            break;
                        case 2:
                            HealerDroneeffect();
                            break;
                        case 3:
                            RocketDroneEffect();
                            break;
                        case 4:
                            LeechDroneEffect();
                            break;
                        case 5:
                            DrainDroneEffect();
                            break;
                        case 6:
                            ReanimatorDroneeffect();
                            break;
                        case 7:
                            EnergyRegenDroneeffect();
                            break;
                    }

                }
            }
        }
        
    }

    void ChangeSprite()
    {
        if(ActiveDroneID !=-1)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Animator>().enabled = true;
            if (drones[ActiveDroneID].Medical && GetComponent<Animator>().runtimeAnimatorController != MedController)
            {
                GetComponent<Animator>().runtimeAnimatorController = MedController;
            }
            else if (!drones[ActiveDroneID].Medical && GetComponent<Animator>().runtimeAnimatorController != GunController)
            {
                GetComponent<Animator>().runtimeAnimatorController = GunController;
            }
            if (GetComponent<SpriteRenderer>().sprite != drones[ActiveDroneID].Sprite)
            {
                GetComponent<SpriteRenderer>().sprite = drones[ActiveDroneID].Sprite;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().enabled = false;
        }
        
        
    }

    void EnergyConverterDroneeffect()
    {
        if(playerhp.Eldonhp < playerhp.Eldonmaxhp)
        {
            playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
            playerhp.Eldonhp += drones[ActiveDroneID].Effect * playerhp.Eldonmaxhp;
            if(playerhp.Eldonhp> playerhp.Eldonmaxhp)
            {
                playerhp.Eldonhp = playerhp.Eldonmaxhp;
            }
            dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("playanim");
        }
    }

    void EnergyRegenDroneeffect()
    {
        playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
        playerhp.EldonNRG += drones[ActiveDroneID].Effect * playerhp.EldonmaxNRG;

        if (playerhp.EldonNRG > playerhp.EldonmaxNRG)
        {
            playerhp.EldonNRG = playerhp.EldonmaxNRG;
        }

        dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("playanim");
    }

    void HealerDroneeffect()
    {
        playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
        playerhp.Eldonhp += drones[ActiveDroneID].Effect * playerhp.Eldonmaxhp;
        if (playerhp.EldonNRG > playerhp.EldonmaxNRG)
        {
            playerhp.EldonNRG = playerhp.EldonmaxNRG;
        }
        dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("playanim");
    }

    void ReanimatorDroneeffect()
    {
        if(playerhp.Eldonhp<=playerhp.Eldonmaxhp*0.1f)
        {
            playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
            playerhp.Eldonhp += drones[ActiveDroneID].Effect * playerhp.Eldonmaxhp;
            if (playerhp.EldonNRG > playerhp.EldonmaxNRG)
            {
                playerhp.EldonNRG = playerhp.EldonmaxNRG;
            }
            dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("playanim");
        }
        
    }

    void RocketDroneEffect()
    {
        float detectdist = 7.0f;
        Collider2D[] listcollider = Physics2D.OverlapCircleAll(transform.position, detectdist);
        float lowestdist = detectdist+1;
        Transform target = null;
        foreach (Collider2D collider in listcollider)
        {
            
            if((collider.transform.tag=="enemy" || collider.transform.tag == "Boss") && Vector2.Distance(collider.transform.position,transform.position)<lowestdist)
            {
                target = collider.transform;
                lowestdist = Vector2.Distance(collider.transform.position, transform.position);
            }
        }
        if(target != null)
        {
            if (Physics2D.Raycast(transform.position, target.position - transform.position).collider.transform==target)
            {
                GameObject newrocket = Instantiate(drones[ActiveDroneID].Summon, transform.position, Quaternion.identity);
                newrocket.GetComponent<RocketScript>().target = target;
                newrocket.GetComponent<RocketScript>().damage = (int)(augmentsscript.EquipedStats.Damage*drones[ActiveDroneID].Effect);
                playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
                dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
                newrocket.transform.localScale = Vector3.one * 0.2f;
                target = null;
            }
        }
    }

    void LeechDroneEffect()
    {
        float detectdist = 3.0f;
        LeechDroneScript leech = null;
        LeechDroneScript[] leeches = FindObjectsByType<LeechDroneScript>(FindObjectsSortMode.None);
        foreach(LeechDroneScript maybeleech in leeches)
        {
            if(maybeleech.isleech)
            {
                leech = maybeleech;
            }
        }
        if (leech != null && leech.target!=null)
        {
            leech.target.GetComponent<EnemyHP>().TakeDamage(0, (int)(augmentsscript.EquipedStats.NRJDamage * drones[ActiveDroneID].Effect));
            playerhp.EldonNRG += augmentsscript.EquipedStats.NRJDamage * drones[ActiveDroneID].Effect;
            if (playerhp.EldonNRG > playerhp.EldonmaxNRG)
            {
                playerhp.EldonNRG = playerhp.EldonmaxNRG;
            }
            dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
        }
        else
        {
            if(leech != null)
            {
                Destroy(leech.gameObject);
            }
            Collider2D[] listcollider = Physics2D.OverlapCircleAll(transform.position, detectdist);
            float lowestdist = detectdist + 1;
            Transform target = null;
            foreach (Collider2D collider in listcollider)
            {

                if ((collider.transform.tag == "enemy" || collider.transform.tag == "Boss") && Vector2.Distance(collider.transform.position, transform.position) < lowestdist)
                {
                    target = collider.transform;
                    lowestdist = Vector2.Distance(collider.transform.position, transform.position);
                }
            }
            if (target != null)
            {
                GameObject newray = Instantiate(drones[ActiveDroneID].Summon, transform.position, Quaternion.identity);
                newray.GetComponent<LeechDroneScript>().target = target;
                newray.GetComponent<LeechDroneScript>().isleech = true;
                newray.GetComponent<LeechDroneScript>().launcher = transform;
                newray.GetComponent<LeechDroneScript>().detectionrange = detectdist;
                target.GetComponent<EnemyHP>().TakeDamage(0, (int)(augmentsscript.EquipedStats.NRJDamage * drones[ActiveDroneID].Effect));
                playerhp.EldonNRG += augmentsscript.EquipedStats.NRJDamage * drones[ActiveDroneID].Effect;
                if (playerhp.EldonNRG > playerhp.EldonmaxNRG)
                {
                    playerhp.EldonNRG = playerhp.EldonmaxNRG;
                }
                dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
            }
        }
        
    }

    void DrainDroneEffect()
    {
        float detectdist = 3.0f;
        LeechDroneScript leech = null;
        LeechDroneScript[] leeches = FindObjectsByType<LeechDroneScript>(FindObjectsSortMode.None);
        foreach (LeechDroneScript maybeleech in leeches)
        {
            if (!maybeleech.isleech)
            {
                leech = maybeleech;
            }
        }
        if (leech != null && leech.target != null)
        {
            leech.target.GetComponent<EnemyHP>().TakeDamage((int)(augmentsscript.EquipedStats.Damage * drones[ActiveDroneID].Effect),0);
            playerhp.Eldonhp += augmentsscript.EquipedStats.Damage * drones[ActiveDroneID].Effect;
            if (playerhp.Eldonhp > playerhp.Eldonmaxhp)
            {
                playerhp.Eldonhp = playerhp.Eldonmaxhp;
            }
            dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
        }
        else
        {
            if (leech != null)
            {
                Destroy(leech.gameObject);
            }
            Collider2D[] listcollider = Physics2D.OverlapCircleAll(transform.position, detectdist);
            float lowestdist = detectdist + 1;
            Transform target = null;
            foreach (Collider2D collider in listcollider)
            {

                if ((collider.transform.tag == "enemy" || collider.transform.tag == "Boss") && Vector2.Distance(collider.transform.position, transform.position) < lowestdist)
                {
                    target = collider.transform;
                    lowestdist = Vector2.Distance(collider.transform.position, transform.position);
                }
            }
            if (target != null)
            {
                GameObject newray = Instantiate(drones[ActiveDroneID].Summon, transform.position, Quaternion.identity);
                newray.GetComponent<LeechDroneScript>().target = target;
                newray.GetComponent<LeechDroneScript>().launcher = transform;
                newray.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0f,1f,0f);
                newray.GetComponent<LeechDroneScript>().detectionrange = detectdist;
                leech.target.GetComponent<EnemyHP>().TakeDamage((int)(augmentsscript.EquipedStats.Damage * drones[ActiveDroneID].Effect), 0);
                playerhp.Eldonhp += augmentsscript.EquipedStats.Damage * drones[ActiveDroneID].Effect;
                if (playerhp.Eldonhp > playerhp.Eldonmaxhp)
                {
                    playerhp.Eldonhp = playerhp.Eldonmaxhp;
                }
                dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
            }
        }

    }

}
