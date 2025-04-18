using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Statstext : MonoBehaviour
{

    public int MaxHP;
    public int currentHP;
    public int currentNRJ;
    public int MaxNRJ;
    public int Damage;
    public int NRJDamage;
    public int DamageReduction;
    public int JumpHeight;
    public int Speed;
    public int MaxSlots;
    public int UsedSlots;
    public int Shards;

    public TextMeshProUGUI MetalText;
    public TextMeshProUGUI CoreText;
    public TextMeshProUGUI ElecText;
    private PlayerHP PlayerHP;

    public bool restrictedstats;

    private bool playerobjectfound;

    // Start is called before the first frame update
    void Start()
    {
        if(FindAnyObjectByType<AugmentsScript>() != null)
        {
            playerobjectfound = true;
            FindAnyObjectByType<AugmentsScript>().statdisplayobject = this;
        }
        PlayerHP = FindAnyObjectByType<PlayerHP>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerobjectfound)
        {
            if (FindAnyObjectByType<AugmentsScript>() != null)
            {
                playerobjectfound = true;
                FindAnyObjectByType<AugmentsScript>().statdisplayobject = this;
            }
        }
        currentHP = (int)FindAnyObjectByType<PlayerHP>().Eldonhp;
        currentNRJ = (int)FindAnyObjectByType<PlayerHP>().EldonNRG;
        if (MaxHP!=0)
        {
            if(restrictedstats)
            {
                string texttodisplay = "Health : " + currentHP + " / " + MaxHP + "\n";
                texttodisplay += "Energy : " + currentNRJ + " / " + MaxNRJ + "\n";
                texttodisplay += "Physical Damage : " + Damage + "\n";
                texttodisplay += "Energy Damage : " + NRJDamage + "\n";
                texttodisplay += "Defense : " + DamageReduction + "\n";
                texttodisplay += "Jump Height : " + JumpHeight + "%\n";
                texttodisplay += "Speed : " + Speed + "%\n";
                GetComponent<TextMeshProUGUI>().text = texttodisplay;
            }
            else
            {
                string texttodisplay = "Health : " + currentHP + " / " + MaxHP + "\n";
                texttodisplay += "Energy : " + currentNRJ + " / " + MaxNRJ + "\n";
                texttodisplay += "Physical Damage : " + Damage + "\n";
                texttodisplay += "Energy Damage : " + NRJDamage + "\n";
                texttodisplay += "Defense : " + DamageReduction + "\n";
                texttodisplay += "Jump Height : " + JumpHeight + "%\n";
                texttodisplay += "Speed : " + Speed + "%\n";
                texttodisplay += "Augment Slots : " + UsedSlots + " / " + MaxSlots + "\n";
                texttodisplay += "Crystal Shards : " + Shards + "\n";
                GetComponent<TextMeshProUGUI>().text = texttodisplay;
            }
            
        }

        if(MetalText!=null && CoreText!=null && ElecText != null)
        {
            MetalText.text = " : " + PlayerHP.MetalScrap;
            CoreText.text = " : " + PlayerHP.CorePieces;
            ElecText.text = " : " + PlayerHP.ElectronicComponents;
        }
    }

    private void OnEnable()
    {
        FindAnyObjectByType<AugmentsScript>().statdisplayobject = this;
        FindAnyObjectByType<AugmentsScript>().ApplyAugmentBoost();
    }
}
