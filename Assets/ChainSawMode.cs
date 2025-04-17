using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSawMode : MonoBehaviour
{

    public float EnergyPerSecond;

    public bool chainsawmode;

    public float chainsawdamageMultiplier;
    public float chainsawdefenseMultiplier;

    private Global global;
    private Healthbar healthbar;
    // Start is called before the first frame update
    void Start()
    {
        global = FindAnyObjectByType<Global>();
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (global.atsavepoint || global.indialogue || global.zipping || global.grappling || global.inpause)
        {
            return;
        }
        if (chainsawmode)
        {
            float costmultiplier = 1f;
            if (GetComponent<AugmentsScript>().EquipedAugments[15])
            {
                costmultiplier*= GetComponent<AugmentsScript>().Augmentlist[15].valueincr;
            }
            GetComponent<PlayerHP>().EldonNRG-=EnergyPerSecond*Time.fixedDeltaTime*costmultiplier;
            if(GetComponent<PlayerHP>().EldonNRG<=0)
            {
                GetComponent<PlayerHP>().EldonNRG = 0;
                chainsawmode = false;
            }
            if(!GetComponent<EldonAttack>().slayermode)
            {
                GetComponent<EldonAttack>().Modeswitch(true);
            }
        }
        healthbar.chainsawmode=chainsawmode;
    }

    private void OnNorthButton()
    {
        if (global.atsavepoint || global.indialogue || global.zipping || global.grappling || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll") || global.inpause || global.ininventory)
        {
            return;
        }
        if(!chainsawmode)
        {
            if (!GetComponent<GrappleScript>().pressedtrigger && GetComponent<PlayerHP>().EldonNRG > 0)
            {
                chainsawmode = true;
                if (!GetComponent<EldonAttack>().slayermode)
                {
                    GetComponent<EldonAttack>().Modeswitch(true);
                }
            }
        }
        else
        {
            chainsawmode=false;
        }
        
    }

}
