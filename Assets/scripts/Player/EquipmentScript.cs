using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using static AugmentsScript;
using static EquipmentScript;

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

    public Sprite EnergyShard;

    public GameObject gotitempopupprefab;

    // Start is called before the first frame update

    void Awake()
    {
        
        if (FindAnyObjectByType<Global>().clickednewgame)
        {
            FindAnyObjectByType<SaveManager>().CreateEmptySaveFile(FindAnyObjectByType<SaveManager>().CurrentSlot);
        }
        FindAnyObjectByType<SaveManager>().ApplySave();
    }

    void Start()
    {

        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        playerHP = FindAnyObjectByType<PlayerHP>();
    }
    private void FixedUpdate()
    {
        if(FindAnyObjectByType<Global>().atsavepoint|| FindAnyObjectByType<Global>().indialogue)
        {
            return;
        }
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
            if(drone1.GetComponent<SupportDrone>().ActiveDroneID!=-1)
            {
                healthbar.SetMaxDrone1(drone1.GetComponent<SupportDrone>().drones[drone1.GetComponent<SupportDrone>().ActiveDroneID].cooldown / Time.deltaTime);
                healthbar.SetDrone1(drone1.GetComponent<SupportDrone>().dronecd);
                healthbar.drone1usable = playerHP.EldonNRG >= drone1.GetComponent<SupportDrone>().drones[drone1.GetComponent<SupportDrone>().ActiveDroneID].RequiredEnergy;
            }
            else
            {
                healthbar.SetMaxDrone1(1);
                healthbar.SetDrone1(1);
                healthbar.drone1usable=false;
            }
            
        }
        if (drone2 != null)
        {
            if (drone2.GetComponent<SupportDrone>().ActiveDroneID != -1)
            {
                healthbar.SetMaxDrone2(drone2.GetComponent<SupportDrone>().drones[drone2.GetComponent<SupportDrone>().ActiveDroneID].cooldown / Time.deltaTime);
                healthbar.SetDrone2(drone2.GetComponent<SupportDrone>().dronecd);
                healthbar.drone2usable = playerHP.EldonNRG >= drone2.GetComponent<SupportDrone>().drones[drone2.GetComponent<SupportDrone>().ActiveDroneID].RequiredEnergy;
            }
            else
            {
                healthbar.SetMaxDrone2(1);
                healthbar.SetDrone2(1);
                healthbar.drone2usable = false;
            }

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
            case 4:
                GadgetScript gadgetScript = GetComponent<GadgetScript>();
                gadgetScript.ActiveGadgetID = ItemID;
                break;
        }
    }


    public void ReceiveItem(int Type, int ID)
    {
        switch(Type) //0 chain, 1 Plate, 2 drone, 3 Augment, 4 Gadget, 5 CrystalShard
        {
            case 0:
                if(Chainslist.Count > ID)
                {
                    Chainslist[ID].locked = false;
                    GameObject Itempop = Instantiate(gotitempopupprefab, GameObject.Find("Canvas").transform);
                    Itempop.GetComponent<GotItemPopup>().InitiatePopup(Chainslist[ID].image, "Chain", Chainslist[ID].name);
                }
                else
                {
                    Debug.Log("Incorrect ID, "+ID+" too big for Chainslist");
                }
                break;
            case 1:
                if (Platelist.Count > ID)
                {
                    Platelist[ID].locked = false;
                    GameObject Itempop = Instantiate(gotitempopupprefab, GameObject.Find("Canvas").transform);
                    Itempop.GetComponent<GotItemPopup>().InitiatePopup(Platelist[ID].image, "Plate", Platelist[ID].name);
                }
                else
                {
                    Debug.Log("Incorrect ID, "+ID+" too big for Platelist");
                }
                break;
            case 2:
                if (drone1.GetComponent<SupportDrone>().drones.Count > ID)
                {
                    drone1.GetComponent<SupportDrone>().drones[ID].locked = false;
                    drone2.GetComponent<SupportDrone>().drones[ID].locked = false;
                    GameObject Itempop = Instantiate(gotitempopupprefab, GameObject.Find("Canvas").transform);
                    Itempop.GetComponent<GotItemPopup>().InitiatePopup(drone1.GetComponent<SupportDrone>().drones[ID].Sprite, "Drone", drone1.GetComponent<SupportDrone>().drones[ID].name);
                }
                else
                {
                    Debug.Log("Incorrect ID, "+ID+" too big for drones");
                }
                break;
            case 3:
                AugmentsScript augmentscript = GetComponent<AugmentsScript>();
                if (augmentscript.Augmentlist.Count > ID)
                {
                    augmentscript.Augmentlist[ID].locked = false;
                    GameObject Itempop = Instantiate(gotitempopupprefab, GameObject.Find("Canvas").transform);
                    Itempop.GetComponent<GotItemPopup>().InitiatePopup(augmentscript.Augmentlist[ID].image, "Augment", augmentscript.Augmentlist[ID].name);

                }
                else
                {
                    Debug.Log("Incorrect ID, "+ID+" too big for Augmentlist");
                }
                break;
            case 4:
                GadgetScript gadgetScript = GetComponent<GadgetScript>();
                if (gadgetScript.GadgetList.Count > ID)
                {
                    gadgetScript.GadgetList[ID].locked = false;
                    GameObject Itempop = Instantiate(gotitempopupprefab, GameObject.Find("Canvas").transform);
                    Itempop.GetComponent<GotItemPopup>().InitiatePopup(gadgetScript.GadgetList[ID].image, "Gadget", gadgetScript.GadgetList[ID].name);

                }
                else
                {
                    Debug.Log("Incorrect ID, " + ID + " too big for Augmentlist");
                }
                break;
            case 5:
                augmentscript = GetComponent<AugmentsScript>();
                augmentscript.numberofShardsPickedUp++;
                GameObject ItemShard = Instantiate(gotitempopupprefab, GameObject.Find("Canvas").transform);
                ItemShard.GetComponent<GotItemPopup>().InitiatePopup(EnergyShard, "+5 Max HP, +1 Max Energy", "Crystal Shard");
                break;
        }
    }

}
