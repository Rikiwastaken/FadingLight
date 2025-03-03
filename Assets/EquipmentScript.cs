using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentScript : MonoBehaviour
{

    [System.Serializable]
    public class Chain
    {
        public string name;
        public string description;
        public float DamageMultiplier;
        public float AbsorbMultiplier;
        public int ID;
        public Sprite image;
        public bool locked;
    }



    [System.Serializable]
    public class Plate
    {
        public string name;
        public string description;
        public float Defense;
        public float HPRegen;
        public int ID;
        public Sprite image;
        public bool locked;
    }

    public List<Chain> Chainslist;
    public List<Plate> Platelist;

    public int equipedChainIndex;
    public int equipedPlateIndex;

    private float HPfraction;

    public GameObject drone1;
    public GameObject drone2;

    private Healthbar healthbar;
    private PlayerHP playerHP;

    private int healcd;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        playerHP = FindAnyObjectByType<PlayerHP>();
    }
    private void FixedUpdate()
    {
        //armorbaseregen;
        if(healcd>0)
        {
            healcd--;
        }
        else if(playerHP.Eldonhp < playerHP.Eldonmaxhp && equipedPlateIndex!=-1)
        {
            playerHP.Eldonhp += (int)(playerHP.Eldonmaxhp * Platelist[equipedPlateIndex].HPRegen);
            if(playerHP.Eldonhp > playerHP.Eldonmaxhp)
            {
                playerHP.Eldonhp = playerHP.Eldonmaxhp;
            }
            healcd = (int)(1/Time.deltaTime);
        }


        if(drone1 != null)
        {
            healthbar.SetMaxDrone1(drone1.GetComponent<SupportDrone>().drones[drone1.GetComponent<SupportDrone>().ActiveDroneID].cooldown/Time.deltaTime);
            healthbar.SetDrone1(drone1.GetComponent<SupportDrone>().dronecd);
        }
        if (drone2 != null)
        {
            healthbar.SetMaxDrone2(drone2.GetComponent<SupportDrone>().drones[drone2.GetComponent<SupportDrone>().ActiveDroneID].cooldown / Time.deltaTime);
            healthbar.SetDrone2(drone2.GetComponent<SupportDrone>().dronecd);
        }

    }

    public void EquipItem(int SlotID,  int ItemID)
    {
        switch(SlotID)
        {
            case 0:
                equipedChainIndex = ItemID;
                break;
            case 1:
                equipedPlateIndex = ItemID;
                break;
            case 2:
                if(drone2.GetComponent<SupportDrone>().ActiveDroneID == ItemID)
                {
                    drone2.GetComponent<SupportDrone>().ActiveDroneID = drone1.GetComponent<SupportDrone>().ActiveDroneID;
                    drone1.GetComponent<SupportDrone>().ActiveDroneID = ItemID;
                }
                else
                {
                    drone1.GetComponent<SupportDrone>().ActiveDroneID = ItemID;
                }
                break;
            case 3:
                if (drone1.GetComponent<SupportDrone>().ActiveDroneID == ItemID)
                {
                    drone1.GetComponent<SupportDrone>().ActiveDroneID = drone2.GetComponent<SupportDrone>().ActiveDroneID;
                    drone2.GetComponent<SupportDrone>().ActiveDroneID = ItemID;
                }
                else
                {
                    drone2.GetComponent<SupportDrone>().ActiveDroneID = ItemID;
                }
                break;
        }
    }

}
