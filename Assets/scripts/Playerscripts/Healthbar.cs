using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public Image imageHP;
    public Image imageNRG;

    private float maxhp;
    private float maxnrj;


    public void SetMaxhealth(float maxhealth)
    {
        maxhp=maxhealth;
    }
    


    public void SetHealth(float health)
    {
        imageHP.fillAmount = health/maxhp;
    }

    public void SetMaxEnergy(float maxenergy)
    {
        maxnrj = maxenergy;
    }



    public void SetEnergy(float energy)
    {
        imageNRG.fillAmount = energy/maxnrj;
    }

}
