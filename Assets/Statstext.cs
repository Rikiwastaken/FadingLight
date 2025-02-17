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

    private bool playerobjectfound;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindAnyObjectByType<AugmentsScript>() != null)
        {
            playerobjectfound = true;
            GameObject.FindAnyObjectByType<AugmentsScript>().statdisplayobject = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerobjectfound)
        {
            if (GameObject.FindAnyObjectByType<AugmentsScript>() != null)
            {
                playerobjectfound = true;
                GameObject.FindAnyObjectByType<AugmentsScript>().statdisplayobject = this;
            }
        }
        currentHP = (int)GameObject.FindAnyObjectByType<PlayerHP>().Eldonhp;
        currentNRJ = (int)GameObject.FindAnyObjectByType<PlayerHP>().EldonNRG;
        if (MaxHP!=0)
        {
            string texttodisplay = "Health : " + currentHP + " / " + MaxHP + "\n";
            texttodisplay+="Energy : " + currentNRJ + " / " + MaxNRJ + "\n";
            texttodisplay += "Physical Damage : " + Damage + "\n";
            texttodisplay += "Energy Damage : " + NRJDamage + "\n";
            texttodisplay += "Defense : " + DamageReduction + "\n";
            texttodisplay += "Jump Height : " + JumpHeight + "%\n";
            texttodisplay += "Speed : " + Speed + "%\n";
            GetComponent<TextMeshProUGUI>().text = texttodisplay;
        }
    }
}
