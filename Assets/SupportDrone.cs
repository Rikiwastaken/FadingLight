using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportDrone : MonoBehaviour
{

    [System.Serializable]
    public class HealerDrone
    {
        public int ID;
        public int RequiredEnergy;
        public float Effect;
        public string Description;
        public float cooldown;
    }

    public int ActiveDroneID; //-1 : No Drone, 0 : EnergyConverter Drone, 1 : Energy Regen Drone, 2 Healer Drone

    private PlayerHP playerhp;

    private int dronecd;

    public List<HealerDrone> drones;

    // Start is called before the first frame update
    void Start()
    {
        playerhp = GameObject.FindAnyObjectByType<PlayerHP>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                    }

                }
            }
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

}
