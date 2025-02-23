using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Animations;
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
    }

    public int ActiveDroneID; //-1 : No Drone, 0 : EnergyConverter Drone, 1 : Energy Regen Drone, 2 Healer Drone

    private PlayerHP playerhp;

    private int dronecd;

    public List<HealerDrone> drones;

    public AnimatorController MedController;
    public AnimatorController GunController;

    // Start is called before the first frame update
    void Start()
    {
        playerhp = GameObject.FindAnyObjectByType<PlayerHP>();
        if(drones[ActiveDroneID].Medical)
        {
            GetComponent<Animator>().runtimeAnimatorController =MedController;
        }
        else
        {
            GetComponent<Animator>().runtimeAnimatorController = GunController;
        }
        GetComponent<SpriteRenderer>().sprite= drones[ActiveDroneID].Sprite;


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
                    }

                }
            }
        }
        
    }

    void ChangeSprite()
    {
        if (drones[ActiveDroneID].Medical && GetComponent<Animator>().runtimeAnimatorController != MedController)
        {
            GetComponent<Animator>().runtimeAnimatorController = MedController;
        }
        else if (!drones[ActiveDroneID].Medical && GetComponent<Animator>().runtimeAnimatorController != GunController)
        {
            GetComponent<Animator>().runtimeAnimatorController = GunController;
        }
        if(GetComponent<SpriteRenderer>().sprite != drones[ActiveDroneID].Sprite)
        {
            GetComponent<SpriteRenderer>().sprite = drones[ActiveDroneID].Sprite;
        }
        
    }

    void EnergyConverterDroneeffect()
    {
        if(playerhp.Eldonhp < playerhp.Eldonmaxhp)
        {
            playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
            playerhp.Eldonhp += drones[ActiveDroneID].Effect * playerhp.Eldonmaxhp;
            dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("playanim");
        }
    }

    void EnergyRegenDroneeffect()
    {
        playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
        playerhp.EldonNRG += drones[ActiveDroneID].Effect * playerhp.EldonmaxNRG;
        dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("playanim");
    }

    void HealerDroneeffect()
    {
        playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
        playerhp.Eldonhp += drones[ActiveDroneID].Effect * playerhp.Eldonmaxhp;
        dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("playanim");
    }

    void RocketDroneEffect()
    {
        float detectdist = 7.0f;
        Collider2D[] listcollider = Physics2D.OverlapCircleAll(transform.position, detectdist);
        float lowestdist = detectdist+1;
        Transform target = null;
        foreach (Collider2D collider in listcollider)
        {
            
            if(collider.transform.tag=="enemy" && Vector2.Distance(collider.transform.position,transform.position)<lowestdist)
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
                newrocket.GetComponent<RocketScript>().damage = (int)drones[ActiveDroneID].Effect;
                playerhp.EldonNRG -= drones[ActiveDroneID].RequiredEnergy;
                dronecd = (int)(drones[ActiveDroneID].cooldown / Time.deltaTime);
                newrocket.transform.localScale = Vector3.one * 0.2f;
                target = null;
            }
        }
    }

}
