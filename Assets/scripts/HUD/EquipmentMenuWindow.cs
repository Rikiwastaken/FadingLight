using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static SupportDrone;
using static EquipmentScript;
using static GadgetScript;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem.Switch;
using UnityEngine.XR;

public class EquipmentMenuWindow : MonoBehaviour
{
    public PlayerControls controls;
    public Transform EquipedContainer;
    public Transform EquipmentContainer;
    public Transform ActivepageBackground;
    public TextMeshProUGUI effecttext;
    public TextMeshProUGUI nametext;
    private List<Plate> Platelist;
    private List<Chain> Chainlist;
    public List<HealerDrone> Dronelist;
    public List<Gadget> Gadgetlist;
    private List<int> EquipedItemsID; // 0 chain, 1 Plate, 2 drone1, 3drone2

    private EquipmentScript EquipmentScript;
    private GadgetScript GadgetScript;

    public Button selected;

    private int valueleft;
    private int valueright;
    private int valuedown;
    private int valueup;
    private int valuestickleft;
    private int valuestickright;
    private int valuestickdown;
    private int valuestickup;
    private int valueclick;
    private Vector2 lastinput;

    private int upperlineindex=0;

    private bool onquiped;

    private bool pressedclick;

    public int selectedsection; //0 : nothing, 1 : Chains, 2 : Plates, 3 : Drone1, 4 : Drone2 

    public TextMeshProUGUI NotAtSavePointText;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        EquipmentScript = FindAnyObjectByType<EquipmentScript>();
        GadgetScript = FindAnyObjectByType<GadgetScript>();

        InitializeLists();



        controls.gameplay.crossleft.performed += ctx => valueleft = 1;
        controls.gameplay.crossright.performed += ctx => valueright = 1;
        controls.gameplay.crossleft.canceled += ctx => valueleft = 0;
        controls.gameplay.crossright.canceled += ctx => valueright = 0;
        controls.gameplay.crossdown.performed += ctx => valuedown = 1;
        controls.gameplay.crossdown.canceled += ctx => valuedown = 0;
        controls.gameplay.crossup.performed += ctx => valueup = 1;
        controls.gameplay.crossup.canceled += ctx => valueup = 0;
        controls.gameplay.jump.performed += ctx => valueclick = 1;
        controls.gameplay.jump.canceled += ctx => valueclick = 0;

        controls.gameplay.moveleft.performed += ctx => valuestickleft = 1;
        controls.gameplay.moveright.performed += ctx => valuestickright = 1;
        controls.gameplay.moveleft.canceled += ctx => valuestickleft = 0;
        controls.gameplay.moveright.canceled += ctx => valuestickright = 0;
        controls.gameplay.down.performed += ctx => valuestickdown = 1;
        controls.gameplay.down.canceled += ctx => valuestickdown = 0;
        controls.gameplay.up.performed += ctx => valuestickup = 1;
        controls.gameplay.up.canceled += ctx => valuestickup = 0;

        controls.gameplay.Enable();


    }
    // Update is called once per frame
    void Update()
    {

        if(FindAnyObjectByType<Global>().atsavepoint)
        {
            NotAtSavePointText.text = "";
        }
        else
        {
            NotAtSavePointText.text = "<color=\"red\">You can only change your equipment in Safe Places.</color>";
        }

        switch(selectedsection)
        {
            case 0:
                ActivepageBackground.GetChild(0).gameObject.SetActive(true);
                ActivepageBackground.GetChild(1).gameObject.SetActive(false);
                ActivepageBackground.GetChild(2).gameObject.SetActive(false);
                ActivepageBackground.GetChild(3).gameObject.SetActive(false);
                break;
            case 1:
                ActivepageBackground.GetChild(0).gameObject.SetActive(false);
                ActivepageBackground.GetChild(1).gameObject.SetActive(true);
                ActivepageBackground.GetChild(2).gameObject.SetActive(false);
                ActivepageBackground.GetChild(3).gameObject.SetActive(false);
                break;
            case 2:
                ActivepageBackground.GetChild(0).gameObject.SetActive(false);
                ActivepageBackground.GetChild(1).gameObject.SetActive(false);
                ActivepageBackground.GetChild(2).gameObject.SetActive(true);
                ActivepageBackground.GetChild(3).gameObject.SetActive(false);
                break;
            case 3:
                ActivepageBackground.GetChild(0).gameObject.SetActive(false);
                ActivepageBackground.GetChild(1).gameObject.SetActive(false);
                ActivepageBackground.GetChild(2).gameObject.SetActive(true);
                ActivepageBackground.GetChild(3).gameObject.SetActive(false);
                break;
            case 4:
                ActivepageBackground.GetChild(0).gameObject.SetActive(false);
                ActivepageBackground.GetChild(1).gameObject.SetActive(false);
                ActivepageBackground.GetChild(2).gameObject.SetActive(false);
                ActivepageBackground.GetChild(3).gameObject.SetActive(true);
                break;

        }
        if(valueclick==0)
        {
            pressedclick = false;
        }


        EquipedItemsID = new List<int> { EquipmentScript.equipedChainIndex,EquipmentScript.equipedPlateIndex, EquipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID, EquipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID, GadgetScript.ActiveGadgetID };

        for(int i=0; i<10; i++)
        {
            EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color=new Color(1f,1f,1f,0f);
            EquipmentContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = false;
            if (i<=4)
            {
                EquipedContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                if (EquipedItemsID[i]!=-1)
                {
                    EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    
                    switch (i)
                    {
                        case 0:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentScript.Chainslist[EquipmentScript.equipedChainIndex].image;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 0;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = EquipmentScript.equipedChainIndex;
                            break;
                        case 1:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentScript.Platelist[EquipmentScript.equipedPlateIndex].image;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 1;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = EquipmentScript.equipedPlateIndex;
                            break;
                        case 2:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentScript.drone1.GetComponent<SupportDrone>().drones[EquipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID].Sprite;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 2;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = EquipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID;
                            break;
                        case 3:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentScript.drone1.GetComponent<SupportDrone>().drones[EquipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID].Sprite;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 3;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = EquipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID;
                            break;
                        case 4:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GadgetScript.GadgetList[GadgetScript.ActiveGadgetID].image;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 4;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = GadgetScript.ActiveGadgetID;
                            break;
                    }
                }
                else
                {
                    EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }
                
            }
        }

        Displaysection(selectedsection);

        Vector2 inputstick = Vector2.zero;
        if (valuestickleft != 0 || valuestickright != 0 || valuestickup != 0 || valuestickdown != 0)
        {
            if (valuestickdown != 0)
            {
                inputstick.y = -1;
            }
            else if (valuestickup != 0)
            {
                inputstick.y = 1;
            }
            else if (valuestickleft != 0)
            {
                inputstick.x = -1;
            }
            else if (valuestickright != 0)
            {
                inputstick.x = 1;
            }
        }

        Vector2 input = Vector2.zero;
        if (valueleft != 0 || valueright != 0 || valueup != 0 || valuedown != 0)
        {
            if (valuedown != 0)
            {
                input.y = -1;
            }
            else if (valueup != 0)
            {
                input.y = 1;
            }
            else if (valueleft != 0)
            {
                input.x = -1;
            }
            else if (valueright != 0)
            {
                input.x = 1;
            }
        }

        if (input == Vector2.zero && inputstick != Vector2.zero)
        {
            input = inputstick;
        }

        if (lastinput != input && input != Vector2.zero)
        {
            Direction(input);
        }
        lastinput = input;


        if (selected != null)
        {
            selected.Select();

            UpdateDisplay();


            if (!pressedclick && valueclick == 1 && FindAnyObjectByType<Global>().atsavepoint)
            {
                pressedclick = true;
                selected.onClick.Invoke();
                if(onquiped)
                {
                    onquiped = false;
                    selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                }
            }

        }
        else
        {
            selected = EquipedContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
            onquiped = true;
        }

    }

    private void OnAttack()
    {
        if (onquiped && FindAnyObjectByType<Global>().atsavepoint)
        {
            switch(selectedsection)
            {
                case 0:
                    EquipmentScript.equipedChainIndex = 0;
                    break;
                case 1:
                    EquipmentScript.equipedPlateIndex = 0;
                    break;
                case 2:
                    EquipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID = -1;
                    break;
                case 3:
                    EquipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID = -1;
                    break;
                case 4:
                    GadgetScript.ActiveGadgetID = 0;
                    break;
            }
        }
    }

    void UpdateDisplay()
    {
        switch (selectedsection)
        {
            case 0:
                nametext.text = EquipmentScript.Chainslist[selected.transform.GetComponent<buttonscript>().ObjectID].name;
                int basemultiplier = (int)Math.Round(EquipmentScript.Chainslist[EquipmentScript.equipedChainIndex].DamageMultiplier * 100 - 100);
                int newmultiplier = (int)Math.Round(EquipmentScript.Chainslist[selected.transform.GetComponent<buttonscript>().ObjectID].DamageMultiplier * 100 - 100);
                string basetoshow = "";
                if (basemultiplier < 0)
                {
                    basetoshow = basemultiplier + "%";
                }
                else
                {
                    basetoshow = "+" + basemultiplier + "%";
                }
                string newtoshow = "";
                if (newmultiplier < 0)
                {
                    newtoshow = "<b>" + newmultiplier + "%" + "</b>";
                }
                else
                {
                    newtoshow = "<b>" + "+" + newmultiplier + "%" + "</b>";
                }
                if (newmultiplier < basemultiplier)
                {
                    newtoshow = "<color=\"red\">" + newtoshow + "</color>";
                }
                else if (newmultiplier > basemultiplier)
                {
                    newtoshow = "<color=\"green\">" + newtoshow + "</color>";
                }

                int baseabs = (int)Math.Round(EquipmentScript.Chainslist[EquipmentScript.equipedChainIndex].AbsorbMultiplier * 100 - 100);
                int newmabs = (int)Math.Round(EquipmentScript.Chainslist[selected.transform.GetComponent<buttonscript>().ObjectID].AbsorbMultiplier * 100 - 100);
                string baseabstoshow = "";
                if (baseabs < 0)
                {
                    baseabstoshow = baseabs + "%";
                }
                else
                {
                    baseabstoshow = "+" + baseabs + "%";
                }
                string newabstoshow = "";
                if (newmabs < 0)
                {
                    newabstoshow = "<b>" + newmabs + "%" + "</b>";
                }
                else
                {
                    newabstoshow = "<b>" + "+" + newmabs + "%" + "</b>";
                }
                if (newmabs < baseabs)
                {
                    newabstoshow = "<color=\"red\">" + newabstoshow + "</color>";
                }
                else if (newmabs > baseabs)
                {
                    newabstoshow = "<color=\"green\">" + newabstoshow + "</color>";
                }

                effecttext.text = "Melee Multiplier : " + basetoshow + " -> " + newtoshow + "\n" + "Absorption Multiplier : " + baseabstoshow + " -> " + newabstoshow;
                effecttext.text += "\n\n" + EquipmentScript.Chainslist[selected.transform.GetComponent<buttonscript>().ObjectID].description;
                break;
            case 1:

                nametext.text = EquipmentScript.Platelist[selected.transform.GetComponent<buttonscript>().ObjectID].name;
                int baseregen = (int)Math.Round(EquipmentScript.Platelist[EquipmentScript.equipedPlateIndex].HPRegen * 100);
                int newmregen = (int)Math.Round(EquipmentScript.Platelist[selected.transform.GetComponent<buttonscript>().ObjectID].HPRegen * 100);
                string baseregentoshow = "";
                if (baseregen < 0)
                {
                    baseregentoshow = baseregen + "%/s";
                }
                else
                {
                    baseregentoshow = "+" + baseregen + "%/s";
                }
                string newregentoshow = "";
                if (newmregen < 0)
                {
                    newregentoshow = "<b>" + newmregen + "%/s" + "</b>";
                }
                else
                {
                    newregentoshow = "<b>+" + newmregen + "%/s" + "</b>";
                }
                if (newmregen < baseregen)
                {
                    newregentoshow = "<color=\"red\">" + newregentoshow + "</color>";
                }
                else if (newmregen > baseregen)
                {
                    newregentoshow = "<color=\"green\">" + newregentoshow + "</color>";
                }

                int basedef = (int)EquipmentScript.Platelist[EquipmentScript.equipedPlateIndex].Defense;
                int newdef = (int)EquipmentScript.Platelist[selected.transform.GetComponent<buttonscript>().ObjectID].Defense;
                string deftoshow = "<b>" + newdef + "</b>";
                if (newdef < basedef)
                {
                    deftoshow = "<color=\"red\">" + deftoshow + "</color>";
                }
                else if (newdef > basedef)
                {
                    deftoshow = "<color=\"green\">" + deftoshow + "</color>";
                }

                effecttext.text = "Defense : " + basedef + " -> " + deftoshow + "\n" + "Regen per Second : " + baseregentoshow + " -> " + newregentoshow;
                effecttext.text += "\n\n" + EquipmentScript.Platelist[selected.transform.GetComponent<buttonscript>().ObjectID].description;
                break;
            case 2:
                if (!EquipmentScript.drone1.GetComponent<SupportDrone>().drones[selected.transform.GetComponent<buttonscript>().ObjectID].locked)
                {
                    nametext.text = EquipmentScript.drone1.GetComponent<SupportDrone>().drones[selected.transform.GetComponent<buttonscript>().ObjectID].name;
                    effecttext.text = EquipmentScript.drone1.GetComponent<SupportDrone>().drones[selected.transform.GetComponent<buttonscript>().ObjectID].Description;
                }
                else
                {
                    nametext.text = "Locked Feature";
                    effecttext.text = "Locked Feature";
                }
                break;
            case 3:
                if(!EquipmentScript.drone1.GetComponent<SupportDrone>().drones[selected.transform.GetComponent<buttonscript>().ObjectID].locked)
                {
                    nametext.text = EquipmentScript.drone1.GetComponent<SupportDrone>().drones[selected.transform.GetComponent<buttonscript>().ObjectID].name;
                    effecttext.text = EquipmentScript.drone1.GetComponent<SupportDrone>().drones[selected.transform.GetComponent<buttonscript>().ObjectID].Description;
                }
                else
                {
                    nametext.text = "Locked Feature";
                    effecttext.text = "Locked Feature";
                }
                break;
            case 4:
                if (!GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].locked)
                {
                    nametext.text = GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].name;
                    if(GadgetScript.GadgetList[GadgetScript.ActiveGadgetID].DamageMultiplier!=0)
                    {
                        basemultiplier = (int)Math.Round(GadgetScript.GadgetList[GadgetScript.ActiveGadgetID].DamageMultiplier * 100 - 100);
                    }
                    else
                    {
                        basemultiplier = 0;
                    }


                    if (GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].DamageMultiplier != 0)
                    {
                        newmultiplier = (int)Math.Round(GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].DamageMultiplier * 100 - 100);
                    }
                    else
                    {
                        newmultiplier = 0;
                    }

                    
                    basetoshow = "";
                    if (basemultiplier < 0)
                    {
                        basetoshow = basemultiplier + "%";
                    }
                    else if(basemultiplier > 0)
                    {
                        basetoshow = "+" + basemultiplier + "%";
                    }
                    else
                    {
                        basetoshow = "None";
                    }
                    newtoshow = "";
                    if (newmultiplier < 0)
                    {
                        newtoshow = "<b>" + newmultiplier + "%" + "</b>";
                    }
                    else if(newmultiplier > 0)
                    {
                        newtoshow = "<b>" + "+" + newmultiplier + "%" + "</b>";
                    }
                    else
                    {
                        newtoshow = "<b>" + "None" + "</b>";
                    }
                    if (newmultiplier < basemultiplier)
                    {
                        newtoshow = "<color=\"red\">" + newtoshow + "</color>";
                    }
                    else if (newmultiplier > basemultiplier)
                    {
                        newtoshow = "<color=\"green\">" + newtoshow + "</color>";
                    }


                    if (GadgetScript.GadgetList[GadgetScript.ActiveGadgetID].AbsorbMultiplier != 0)
                    {
                        baseabs = (int)Math.Round(GadgetScript.GadgetList[GadgetScript.ActiveGadgetID].AbsorbMultiplier * 100 - 100);
                    }
                    else
                    {
                        baseabs = 0;
                    }

                    if (GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].AbsorbMultiplier != 0)
                    {
                        newmabs = (int)Math.Round(GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].AbsorbMultiplier * 100 - 100);
                    }
                    else
                    {
                        newmabs = 0;
                    }

                    
                    baseabstoshow = "";
                    if (baseabs < 0)
                    {
                        baseabstoshow = baseabs + "%";
                    }
                    else if(baseabs > 0)
                    {
                        baseabstoshow = "+" + baseabs + "%";
                    }
                    else
                    {
                        baseabstoshow = "None";
                    }
                    
                    newabstoshow = "";
                    if (newmabs < 0)
                    {
                        newabstoshow = "<b>" + newmabs + "%" + "</b>";
                    }
                    else if(newmabs > 0)
                    {
                        newabstoshow = "<b>" + "+" + newmabs + "%" + "</b>";
                    }
                    else
                    {
                        newabstoshow = "<b>None</b>";
                    }
                    if (newmabs < baseabs)
                    {
                        newabstoshow = "<color=\"red\">" + newabstoshow + "</color>";
                    }
                    else if (newmabs > baseabs)
                    {
                        newabstoshow = "<color=\"green\">" + newabstoshow + "</color>";
                    }


                    int baseCost = GadgetScript.GadgetList[GadgetScript.ActiveGadgetID].Energycost;
                    int newcost = GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].Energycost;
                    string basecosttoshow = "";
                    if (baseCost != 0)
                    {
                        basecosttoshow = baseCost + "";
                    }
                    else 
                    {
                        basecosttoshow = "None";
                    }
                    string newcosttoshow = "";
                    if (newcost != 0)
                    {
                        newcosttoshow = "<b>" + newcost  + "</b>";
                    }
                    else
                    {
                        newcosttoshow = "<b>None</b>";
                    }
                    if (newcost > baseabs)
                    {
                        newcosttoshow = "<color=\"red\">" + newcosttoshow + "</color>";
                    }
                    else if (newcost < baseabs)
                    {
                        newcosttoshow = "<color=\"green\">" + newcosttoshow + "</color>";
                    }

                    float baseCd = GadgetScript.GadgetList[GadgetScript.ActiveGadgetID].cooldown;
                    float newCd = GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].cooldown;
                    string baseCDtoshow = "";
                    if (baseCd > 0)
                    {
                        baseCDtoshow = baseCd + "s";
                    }
                    else
                    {
                        baseCDtoshow = "None";
                    }
                    string newCDtoshow = "";
                    if (newCd > 0)
                    {
                        newCDtoshow = "<b>" + newCd + "s</b>";
                    }
                    else
                    {
                        newCDtoshow = "<b>None</b>";
                    }
                    if (newCd > baseCd)
                    {
                        newCDtoshow = "<color=\"red\">" + newCDtoshow + "</color>";
                    }
                    else if (newCd < baseCd)
                    {
                        newCDtoshow = "<color=\"green\">" + newCDtoshow + "</color>";
                    }



                    effecttext.text = "Damage Multiplier : " + basetoshow + " -> " + newtoshow + "\n" + "Absorption Multiplier : " + baseabstoshow + " -> " + newabstoshow + "\n" + "Energy Cost : " + basecosttoshow + " -> " + newcosttoshow + "\n" + "Cooldown : " + baseCDtoshow + " -> " + newCDtoshow;
                    effecttext.text += "\n\n" + GadgetScript.GadgetList[selected.transform.GetComponent<buttonscript>().ObjectID].description;
                }
                else
                {
                    nametext.text = "Locked Feature";
                    effecttext.text = "Locked Feature";
                }
                break;
        }
    }

    void Displaysection(int selectedsection)
    {
        switch(selectedsection)
        {
            case 0:
                for (int i = 0; i < Mathf.Min(Chainlist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Chainlist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Chainlist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 0;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                }
                break;
            case 1:
                for (int i = 0; i < Mathf.Min(Platelist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Platelist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Platelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 1;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                }
                break;
            case 2:
                for (int i = 0; i < Mathf.Min(Dronelist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[i + upperlineindex * 5].Sprite;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Dronelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 2;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                }
                break;
            case 3:
                for (int i = 0; i < Mathf.Min(Dronelist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[i + upperlineindex * 5].Sprite;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Dronelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 3;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                }
                break;
            case 4:
                for (int i = 0; i < Mathf.Min(Gadgetlist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Gadgetlist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Gadgetlist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 4;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                }
                break;
        }
    }


    void InitializeLists()
    {
        Platelist = new List<Plate>();
        foreach (Plate plate in EquipmentScript.Platelist)
        {
            if(!plate.locked)
            {
                Platelist.Add(plate);
            }
        }

        Chainlist = new List<Chain>();
        foreach (Chain Chain in EquipmentScript.Chainslist)
        {
            if (!Chain.locked)
            {
                Chainlist.Add(Chain);
            }
        }

        Dronelist = new List<HealerDrone>();
        foreach(HealerDrone Drone in EquipmentScript.drone1.GetComponent<SupportDrone>().drones)
        {
            if(!Drone.locked)
            {
                Dronelist.Add(Drone);
            }
        }

        Gadgetlist = new List<Gadget>();
        foreach(Gadget Gadget in GadgetScript.GadgetList)
        {
            if (!Gadget.locked)
            {
                Gadgetlist.Add(Gadget);
            }
        }

    }

    void OnDodge()
    {
        if(onquiped)
        {
            FindAnyObjectByType<Global>().CloseInventory();
        }
        else
        {
            selected = EquipedContainer.GetChild(selectedsection).GetChild(1).GetComponentInChildren<Button>();
        }
    }

    void Direction(Vector2 dirinput)
    {
        switch (selectedsection)
        {
            case 0:
                for (int i = 0; i < Mathf.Min(Chainlist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Chainlist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Chainlist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 0;
                }
                break;
            case 1:
                for (int i = 0; i < Mathf.Min(Platelist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Platelist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Platelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 1;
                }
                break;
            case 2:
                for (int i = 0; i < Mathf.Min(Dronelist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[i + upperlineindex * 5].Sprite;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Dronelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 2;
                }
                break;
            case 3:
                for (int i = 0; i < Mathf.Min(Dronelist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[i + upperlineindex * 5].Sprite;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Dronelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 3;
                }
                break;
            case 4:
                for (int i = 0; i < Mathf.Min(Gadgetlist.Count - upperlineindex * 5, 10); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Gadgetlist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Gadgetlist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 4;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                }
                break;
        }

        int activebuttonidstring = 0;
        if (selected != null)
        {
            activebuttonidstring = int.Parse(selected.transform.parent.name.Replace("Slot", "0"));
        }
        if (onquiped)
        {
            upperlineindex = 0;
            if (dirinput != Vector2.zero)
            {
                if (dirinput.x > 0)
                {
                    if (activebuttonidstring < 4)
                    {
                        if(selectedsection+1==2 && EquipedItemsID[2] == -1 && Dronelist.Count==0)
                        {
                            selected = EquipedContainer.GetChild(activebuttonidstring + 3).GetChild(1).GetComponentInChildren<Button>();
                            selectedsection += 3;
                        }
                        else
                        {
                            selected = EquipedContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                            selectedsection += 1;
                        }
                    }
                    else if (activebuttonidstring == 4)
                    {
                        selected = EquipedContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                        selectedsection =0;
                    }
                }
                else if (dirinput.x < 0)
                {
                    if (activebuttonidstring > 0)
                    {
                        if (selectedsection - 1 == 3 && EquipedItemsID[3] == -1 && Dronelist.Count == 0)
                        {
                            selected = EquipedContainer.GetChild(activebuttonidstring - 3).GetChild(1).GetComponentInChildren<Button>();
                            selectedsection -= 3;
                        }
                        else
                        {
                            selected = EquipedContainer.GetChild(activebuttonidstring - 1).GetChild(1).GetComponentInChildren<Button>();
                            selectedsection -= 1;
                        }
                    }
                    else
                    {
                        selected = EquipedContainer.GetChild(4).GetChild(1).GetComponentInChildren<Button>();
                        selectedsection = 4;
                    }
                }
                else if (dirinput.y != 0)
                {
                    selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                    onquiped = false;
                    return;
                }
            }
        }

        else
        {
            if (dirinput != Vector2.zero)
            {
                if (dirinput.x > 0)
                {
                    switch (selectedsection)
                    {
                        case 0:
                            switch(activebuttonidstring)
                            {
                                case < 4:
                                    if (Chainlist.Count > activebuttonidstring + 1)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 4:
                                    selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case 9:
                                    selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case > 4:
                                    if (activebuttonidstring + 1 < Chainlist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                            }
                            break;
                        case 1:
                            switch (activebuttonidstring)
                            {
                                case < 4:
                                    if (Platelist.Count > activebuttonidstring + 1)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 4:
                                    selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case 9:
                                    selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case > 4:
                                    if (activebuttonidstring + 1 < Platelist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                            }
                            break;
                        case 2:
                            switch (activebuttonidstring)
                            {
                                case < 4:
                                    if (Dronelist.Count > activebuttonidstring+1)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 4:
                                    selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case 9:
                                    selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case > 4:
                                    if (activebuttonidstring + 1 < Dronelist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            switch (activebuttonidstring)
                            {
                                case < 4:
                                    if (Dronelist.Count > activebuttonidstring + 1)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 4:
                                    selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case 9:
                                    selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case > 4:
                                    if (activebuttonidstring + 1 < Dronelist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            switch (activebuttonidstring)
                            {
                                case < 4:
                                    if (Gadgetlist.Count > activebuttonidstring + 1)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 4:
                                    selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case 9:
                                    selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    break;
                                case > 4:
                                    if (activebuttonidstring + 1 < Gadgetlist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                            }
                            break;
                    }
                    
                }
                else if (dirinput.x < 0)
                {
                    if (activebuttonidstring == 0)
                    {
                        switch (selectedsection)
                        {
                            case 0:
                                selected = EquipmentContainer.GetChild(Math.Min(4, Chainlist.Count - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 1:
                                selected = EquipmentContainer.GetChild(Mathf.Min(4, Platelist.Count - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 2:
                                selected = EquipmentContainer.GetChild(Mathf.Min(4, Dronelist.Count - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 3:
                                selected = EquipmentContainer.GetChild(Mathf.Min(4, Dronelist.Count - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 4:
                                selected = EquipmentContainer.GetChild(Mathf.Min(4, Gadgetlist.Count - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                        }
                        
                    }
                    else if (activebuttonidstring == 5)
                    {
                        switch (selectedsection)
                        {
                            case 0:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Chainlist.Count - upperlineindex * 5-1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 1:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Platelist.Count - upperlineindex * 5 - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 2:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Dronelist.Count - upperlineindex * 5 - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 3:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Dronelist.Count - upperlineindex * 5 - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 4:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Gadgetlist.Count - upperlineindex * 5 - 1)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                        }
                        
                    }
                    else
                    {
                        selected = EquipmentContainer.GetChild(activebuttonidstring - 1).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                else if (dirinput.y > 0)
                {
                    if (activebuttonidstring <= 4)
                    {
                        if (upperlineindex == 0)
                        {
                            selected = EquipedContainer.GetChild(selectedsection).GetChild(1).GetComponentInChildren<Button>();
                            onquiped = true;
                        }
                        else
                        {
                            upperlineindex--;
                        }

                    }
                    else
                    {
                        selected = EquipmentContainer.GetChild(activebuttonidstring - 5).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                else if (dirinput.y < 0)
                {
                    if (activebuttonidstring <= 4)
                    {
                        switch (selectedsection)
                        {
                            case 0:
                                if (Chainlist.Count - upperlineindex * 5 > 10)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                else if (Chainlist.Count - upperlineindex * 5 > activebuttonidstring + 5)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                break;
                            case 1:
                                if (Platelist.Count - upperlineindex * 5 > 10)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                else if (Platelist.Count - upperlineindex * 5 > activebuttonidstring + 5)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                break;
                            case 2:
                                if (Dronelist.Count - upperlineindex * 5 > 10)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                else if (Dronelist.Count - upperlineindex * 5 > activebuttonidstring + 5)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                break;
                            case 3:
                                if (Dronelist.Count - upperlineindex * 5 > 10)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                else if (Dronelist.Count - upperlineindex * 5 > activebuttonidstring + 5)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                break;
                            case 4:
                                if (Gadgetlist.Count - upperlineindex * 5 > 10)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                else if (Gadgetlist.Count - upperlineindex * 5 > activebuttonidstring + 5)
                                {
                                    selected = EquipmentContainer.GetChild(activebuttonidstring + 5).GetChild(1).GetComponentInChildren<Button>();
                                }
                                break;
                        }
                        return;
                    }
                    switch (selectedsection)
                    {
                        case 0:
                            if (Chainlist.Count > 10 + upperlineindex * 5 + 1)
                            {
                                upperlineindex++;
                                selected = EquipmentContainer.GetChild(Mathf.Min(activebuttonidstring, Chainlist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                            }
                            else
                            {
                                selected = EquipedContainer.GetChild(selectedsection).GetChild(1).GetComponentInChildren<Button>();
                                onquiped = true;
                            }
                            
                            break;
                        case 1:
                            if (Platelist.Count > 10 + upperlineindex * 5 + 1)
                            {
                                upperlineindex++;
                                selected = EquipmentContainer.GetChild(Mathf.Min(activebuttonidstring, Platelist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                            }
                            else
                            {
                                selected = EquipedContainer.GetChild(selectedsection).GetChild(1).GetComponentInChildren<Button>();
                                onquiped = true;
                            }
                            break;
                        case 2:
                            if (Dronelist.Count > 10 + upperlineindex * 5 + 1)
                            {
                                upperlineindex++;
                                selected = EquipmentContainer.GetChild(Mathf.Min(activebuttonidstring, Dronelist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                            }
                            else
                            {
                                selected = EquipedContainer.GetChild(selectedsection).GetChild(1).GetComponentInChildren<Button>();
                                onquiped = true;
                            }
                            break;
                        case 3:
                            if (Dronelist.Count > 10 + upperlineindex * 5 + 1)
                            {
                                upperlineindex++;
                                selected = EquipmentContainer.GetChild(Mathf.Min(activebuttonidstring, Dronelist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                            }
                            else
                            {
                                selected = EquipedContainer.GetChild(selectedsection).GetChild(1).GetComponentInChildren<Button>();
                                onquiped = true;
                            }
                            break;
                        case 4:
                            if (Gadgetlist.Count > 10 + upperlineindex * 5 + 1)
                            {
                                upperlineindex++;
                                selected = EquipmentContainer.GetChild(Mathf.Min(activebuttonidstring, Gadgetlist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                            }
                            else
                            {
                                selected = EquipedContainer.GetChild(selectedsection).GetChild(1).GetComponentInChildren<Button>();
                                onquiped = true;
                            }

                            break;
                    }
                }
            }
        }
    }


    void OnEnable()
    {
        EquipmentScript = FindAnyObjectByType<EquipmentScript>();
        GadgetScript = FindAnyObjectByType<GadgetScript>();
        InitializeLists();
        if (controls!=null)
        {
            controls.gameplay.Enable();
        }

    }
    void OnDisable()
    {
        if (controls != null)
        {
            controls.gameplay.Disable();
        }
    }

}
