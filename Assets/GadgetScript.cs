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
    private void Start()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        PlayerHP = GetComponent<PlayerHP>();
    }
    private void FixedUpdate()
    {
        if(GadgetList[ActiveGadgetID].cooldown!=0)
        {
            healthbar.SetMaxGadget(GadgetList[ActiveGadgetID].cooldown / Time.deltaTime);
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
        else
        {
            UseGadget();
        }


    }

    private void UseGadget()
    {
        Gadget ActiveGadget = GadgetList[ActiveGadgetID];

        if(PlayerHP.EldonNRG>= ActiveGadget.Energycost)
        {
            PlayerHP.EldonNRG -= ActiveGadget.Energycost;
            gadgetCDcounter = (int)(ActiveGadget.cooldown / Time.deltaTime);
        }
    }

}
