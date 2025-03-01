using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static SupportDrone;
using static EquipmentScript;
using static UnityEngine.InputSystem.InputAction;

public class EquipmentMenuWindow : MonoBehaviour
{
    public PlayerControls controls;
    public Transform EquipedContainer;
    public Transform EquipmentContainer;
    public TextMeshProUGUI effecttext;
    private List<Plate> Platelist;
    private List<Chain> Chainlist;
    private List<HealerDrone> Dronelist;
    private List<int> EquipedItemsID; // 0 chain, 1 Plate, 2 drone1, 3drone2

    private EquipmentScript EquipmentScript;

    public Button selected;

    private int valueleft;
    private int valueright;
    private int valuedown;
    private int valueup;
    private int valueclick;
    private Vector2 lastinput;

    private int upperlineindex=0;

    private bool onquiped;

    private bool pressedclick;

    public int selectedsection; //0 : nothing, 1 : Chains, 2 : Plates, 3 : Drone1, 4 : Drone2 

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        EquipmentScript = FindAnyObjectByType<EquipmentScript>();

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

        controls.gameplay.Enable();


    }
    // Update is called once per frame
    void Update()
    {

        if(valueclick==0)
        {
            pressedclick = false;
        }


        EquipedItemsID = new List<int> { EquipmentScript.equipedChainIndex,EquipmentScript.equipedPlateIndex, EquipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID, EquipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID };

        for(int i=0; i<10; i++)
        {
            EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color=new Color(1f,1f,1f,0f);
            if(i<=3)
            {
                if (EquipedItemsID[i]!=-1)
                {
                    EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    switch (i)
                    {
                        case 0:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Chainlist[EquipmentScript.equipedChainIndex].image;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 0;
                            break;
                        case 1:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Platelist[EquipmentScript.equipedPlateIndex].image;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 1;
                            break;
                        case 2:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[EquipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID].Sprite;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 2;
                            break;
                        case 3:
                            EquipedContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[EquipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID].Sprite;
                            EquipedContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 3;
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

        if (lastinput != input && input != Vector2.zero)
        {
            Direction(input);
        }
        lastinput = input;


        if (selected != null)
        {
            selected.Select();

            switch(selectedsection)
            {
                case 0:
                    effecttext.text = Chainlist[selected.transform.GetComponent<buttonscript>().ObjectID].description;
                    break;
                case 1:
                    effecttext.text = Platelist[selected.transform.GetComponent<buttonscript>().ObjectID].description;
                    break;
                case 2:
                    effecttext.text = Dronelist[selected.transform.GetComponent<buttonscript>().ObjectID].Description;
                    break;
                case 3:
                    effecttext.text = Dronelist[selected.transform.GetComponent<buttonscript>().ObjectID].Description;
                    break;
            }
            if (!pressedclick && valueclick == 1)
            {
                pressedclick = true;
                selected.onClick.Invoke();
            }

        }
        else
        {
            effecttext.text = "Choose an Equipment to see its effect";
        }

    }



    void Displaysection(int selectedsection)
    {
        switch(selectedsection)
        {
            case 0:
                for (int i = 0; i < Mathf.Min(Chainlist.Count - upperlineindex * 5, 9); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Chainlist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Chainlist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 0;
                }
                break;
            case 1:
                for (int i = 0; i < Mathf.Min(Platelist.Count - upperlineindex * 5, 9); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Platelist[i + upperlineindex * 5].image;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Platelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 1;
                }
                break;
            case 2:
                for (int i = 0; i < Mathf.Min(Dronelist.Count - upperlineindex * 5, 9); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[i + upperlineindex * 5].Sprite;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Dronelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 2;
                }
                break;
            case 3:
                for (int i = 0; i < Mathf.Min(Dronelist.Count - upperlineindex * 5, 9); i++)
                {
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    EquipmentContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Dronelist[i + upperlineindex * 5].Sprite;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = Dronelist[i + upperlineindex * 5].ID;
                    EquipmentContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().equipmentslotID = 3;
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
        }
        if (dirinput != Vector2.zero)
        {
            if (selected == null)
            {
                selected = EquipedContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                onquiped = true;
                return;
            }
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
                    if (activebuttonidstring < 3)
                    {
                        selected = EquipedContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                        selectedsection += 1;
                    }
                    else if (activebuttonidstring == 3)
                    {
                        selected = EquipedContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                        selectedsection =0;
                    }
                }
                else if (dirinput.x < 0)
                {
                    if (activebuttonidstring > 0)
                    {
                        selected = EquipedContainer.GetChild(activebuttonidstring - 1).GetChild(1).GetComponentInChildren<Button>();
                        selectedsection -= 1;
                    }
                    else
                    {
                        selected = EquipedContainer.GetChild(3).GetChild(1).GetComponentInChildren<Button>();
                        selectedsection = 3;
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
                    switch (activebuttonidstring)
                    {
                        case < 4:
                            selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                            break;
                        case 4:
                            selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                            break;
                        case 9:
                            selected = EquipmentContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                            break;
                        case > 4:
                            switch(selectedsection)
                            {
                                case 0:
                                    if (activebuttonidstring + 1 < Chainlist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 1:
                                    if (activebuttonidstring + 1 < Platelist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 2:
                                    if (activebuttonidstring + 1 < Dronelist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    break;
                                case 3:
                                    if (activebuttonidstring + 1 < Dronelist.Count - upperlineindex * 5)
                                    {
                                        selected = EquipmentContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                                    }
                                    else
                                    {
                                        selected = EquipmentContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
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
                        selected = EquipmentContainer.GetChild(4).GetChild(1).GetComponentInChildren<Button>();
                    }
                    else if (activebuttonidstring == 5)
                    {
                        switch (selectedsection)
                        {
                            case 0:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Chainlist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 1:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Platelist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 2:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Dronelist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
                                break;
                            case 3:
                                selected = EquipmentContainer.GetChild(Mathf.Min(9, Dronelist.Count - upperlineindex * 5)).GetChild(1).GetComponentInChildren<Button>();
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
                            }
                            break;
                    }
                }
            }
        }
    }


    void OnEnable()
    {
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
