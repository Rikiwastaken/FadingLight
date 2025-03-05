using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public Image imageHP;
    public Image imageNRG;
    public Image imageDrone1;
    public Image imageDrone2;

    private float maxhp;
    private float maxnrj;
    private float maxd1;
    private float maxd2;

    public void SetMaxDrone1(float maxCD)
    {
        maxd1 = maxCD;
    }

    public void SetMaxDrone2(float maxCD)
    {
        maxd2 = maxCD;
    }

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

    public void SetDrone1(float CD)
    {
        imageDrone1.fillAmount=(maxd1-CD)/maxd1;
    }


    public void SetDrone2(float CD)
    {
        imageDrone2.fillAmount = (maxd2 - CD) / maxd2;
    }
}
