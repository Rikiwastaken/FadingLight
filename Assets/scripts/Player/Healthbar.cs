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
    public Image imageGadget;
    public Image imageIndicatorDrone1;
    public Image imageIndicatorDrone2;
    public Image imageIndicatorGadget;

    private float maxhp;
    private float maxnrj;
    private float maxd1;
    private float maxd2;
    private float maxGadget;
    public bool drone1usable;
    public bool drone2usable;
    public bool gadgetusable;

    void FixedUpdate()
    {
        if(imageGadget != null)
        {
            float color = imageGadget.fillAmount*(2f / 3f) + 1f/3f;
            imageGadget.color = new Color(color, color, color);
        }
        
        if(imageDrone1 != null)
        {
            float color = imageDrone1.fillAmount * (2f / 3f) + 1f / 3f;
            imageDrone1.color = new Color(color, color, color);
        }
        
        if( imageDrone2 != null)
        {
            float color = imageDrone2.fillAmount * (2f / 3f) + 1f / 3f;
            imageDrone2.color = new Color(color, color, color);
        }
        if(imageIndicatorGadget!=null)
        {
            if (gadgetusable)
            {
                imageIndicatorGadget.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                imageIndicatorGadget.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        if(imageIndicatorDrone1 != null)
        {
            if (drone1usable)
            {
                imageIndicatorDrone1.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                imageIndicatorDrone1.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        if(imageIndicatorDrone2 != null)
        {
            if (drone2usable)
            {
                imageIndicatorDrone2.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                imageIndicatorDrone2.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

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

    public void SetMaxGadget(float maxgadgetcd)
    {
        maxGadget = maxgadgetcd;
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

    public void SetGadgetCD(float CD)
    {
        imageGadget.fillAmount=(maxGadget - CD)/ maxGadget;
    }
}
