using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class AugmentsScript : MonoBehaviour
{

    [System.Serializable]
    public class Augment
    {
        public string name;
        public string description;
        public int attributeID; //0 maxHP, 1 maxNRG, 2 Damage, 3 Damage, 4 DamageReduction, 5 JumpHeight, 6 Speed
        public float valueincr;
        public bool mult; //if true, multiplicative, else additive
        public Sprite image;
        public int SlotsUsed;
    }



    [System.Serializable]
    public class Stats
    {
        public float MaxHP;
        public float MaxNRJ;
        public float Damage;
        public float NRJDamage;
        public float DamageReduction;
        public float JumpHeight;
        public float Speed;
        public int MaxSlots;
        public int EquipedSlots;
    }

    public List<Augment> Augmentlist;
    public List<bool> EquipedAugments;
    public Stats Basestats;
    public Stats EquipedStats;
    public Statstext statdisplayobject;

    public bool manuallyapplyaugmentboosts;

    private float HPfraction;

    public int numberofequipedaugments;

    private void Start()
    {
        ApplyAugmentBoost();
    }
    private void Update()
    {
        if (manuallyapplyaugmentboosts)
        {
            manuallyapplyaugmentboosts = false;
            ApplyAugmentBoost();
        }
    }

    public void ApplyAugmentBoost()
    {
        numberofequipedaugments = 0;
        HPfraction = GetComponent<PlayerHP>().Eldonhp/ GetComponent<PlayerHP>().Eldonmaxhp;
        InitiateEquipedStats();

        for (int i = 0; i < EquipedAugments.Count; i++)
        {
            if (EquipedAugments[i] == true && i<Augmentlist.Count)
            {
                numberofequipedaugments++;
                ApplyAttribute(Augmentlist[i].attributeID, Augmentlist[i].valueincr, Augmentlist[i].mult);

            }
        }

        UpdateInGameValues();

    }

    private void ApplyAttribute(int AttributeID, float value, bool mult)
    {
        if(mult)
        {
            switch (AttributeID)
            {
                case 0:
                    EquipedStats.MaxHP *= value;
                    break;
                case 1:
                    EquipedStats.MaxNRJ *= value;
                    break;
                case 2:
                    EquipedStats.Damage *= value;
                    break;
                case 3:
                    EquipedStats.NRJDamage *= value;
                    break;
                case 4:
                    EquipedStats.DamageReduction *= value;
                    break;
                case 5:
                    EquipedStats.JumpHeight *= value;
                    break;
                case 6:
                    EquipedStats.Speed *= value;
                    break;
                case 7:
                    EquipedStats.MaxSlots = (int)(EquipedStats.MaxSlots*value);
                    break;

            }
        }
        else
        {
            switch (AttributeID)
            {
                case 0:
                    EquipedStats.MaxHP += value;
                    break;
                case 1:
                    EquipedStats.MaxNRJ += value;
                    break;
                case 2:
                    EquipedStats.Damage += value;
                    break;
                case 3:
                    EquipedStats.NRJDamage += value;
                    break;
                case 4:
                    EquipedStats.DamageReduction += value;
                    break;
                case 5:
                    EquipedStats.JumpHeight += value;
                    break;
                case 6:
                    EquipedStats.Speed += value;
                    break;
                case 7:
                    EquipedStats.MaxSlots = (int)(EquipedStats.MaxSlots + value);
                    break;

            }
        }
    }

    private void InitiateEquipedStats()
    {
        Stats newEquipedStats = new Stats();
        newEquipedStats.MaxHP = Basestats.MaxHP;
        newEquipedStats.MaxNRJ= Basestats.MaxNRJ;
        newEquipedStats.Damage= Basestats.Damage;
        newEquipedStats.NRJDamage = Basestats.NRJDamage;
        newEquipedStats.DamageReduction= Basestats.DamageReduction;
        newEquipedStats.JumpHeight= Basestats.JumpHeight;
        newEquipedStats.Speed = Basestats.Speed;
        newEquipedStats.MaxSlots= Basestats.MaxSlots;
        EquipedStats = newEquipedStats;
    }
    
    private void UpdateInGameValues()
    {
        GetComponent<EldonAttack>().nrgdamage = (int)EquipedStats.NRJDamage;
        GetComponent<EldonAttack>().hpdamage = (int)EquipedStats.Damage;
        GetComponent<PlayerHP>().Eldonmaxhp = (int)EquipedStats.MaxHP;
        GetComponent<PlayerHP>().EldonmaxNRG = (int)EquipedStats.MaxNRJ;
        GetComponent<PlayerHP>().damagereduction = (int)EquipedStats.DamageReduction;
        GetComponent<PlayerJumpV3>().jumpForce = (int)EquipedStats.JumpHeight;
        GetComponent<PlayerMovement>().maxspeed = (int)EquipedStats.Speed;
        if (HPfraction * EquipedStats.MaxHP >= 1f )
        {
            GetComponent<PlayerHP>().Eldonhp = (int)(HPfraction* EquipedStats.MaxHP);
        }
        else
        {
            GetComponent<PlayerHP>().Eldonhp = (int)EquipedStats.MaxHP;
        }
        GetComponent<PlayerHP>().UpdateBars();
        if(statdisplayobject!=null)
        {
            statdisplayobject.MaxHP = (int)EquipedStats.MaxHP;
            statdisplayobject.Damage = (int)EquipedStats.Damage;
            statdisplayobject.NRJDamage = (int)EquipedStats.NRJDamage;
            statdisplayobject.MaxHP = (int)EquipedStats.MaxHP;
            statdisplayobject.MaxNRJ = (int)EquipedStats.MaxNRJ;
            statdisplayobject.DamageReduction = (int)EquipedStats.DamageReduction;
            statdisplayobject.JumpHeight = (int)(100f*EquipedStats.JumpHeight / Basestats.JumpHeight);
            statdisplayobject.Speed = (int)(100f * EquipedStats.Speed / Basestats.Speed);
            statdisplayobject.MaxSlots = EquipedStats.MaxSlots;
            statdisplayobject.UsedSlots = EquipedStats.EquipedSlots;
        }
    }

    public void EquipAugment(int ID)
    {
        if(ID<Augmentlist.Count && ID>=0 && numberofequipedaugments<14)
        {
            if (Augmentlist[ID].SlotsUsed < EquipedStats.MaxSlots-EquipedStats.EquipedSlots)
            {
                EquipedAugments[ID] = true;
                EquipedStats.EquipedSlots += Augmentlist[ID].SlotsUsed;
                ApplyAugmentBoost();
            }
        }
    }

    public void UnEquipAugment(int ID)
    {
        if (ID < Augmentlist.Count && ID >= 0)
        {
            if (EquipedAugments[ID])
            {
                EquipedAugments[ID] = false;
                EquipedStats.EquipedSlots -= Augmentlist[ID].SlotsUsed;
                ApplyAugmentBoost();
            }
        }
    }

}
