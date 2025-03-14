using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static AugmentsScript;
using static UnityEngine.InputSystem.InputAction;

public class AugmentsMenuWindow : MonoBehaviour
{
    public PlayerControls controls;
    public Transform EquipedAugmentsContainer;
    public Transform DisplayAugmentsContainer;
    public TextMeshProUGUI effecttext;
    public TextMeshProUGUI necessaryslots;
    private List<Augment> Augmentlist;
    private List<bool> EquipedAugments;

    private AugmentsScript augmentscript;
    public Sprite Lockedsprite;

    public Button selected;

    private int usedslot;
    private int valueleft;
    private int valueright;
    private int valuedown;
    private int valueup;
    private int valueclick;
    private Vector2 lastinput;

    private int upperlineindex=0;

    private bool onquiped;

    private bool pressedclick;

    public TextMeshProUGUI NotAtSavePointText;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        augmentscript = FindAnyObjectByType<AugmentsScript>();

        Augmentlist = augmentscript.Augmentlist;


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

        if (FindAnyObjectByType<Global>().atsavepoint)
        {
            NotAtSavePointText.text = "";
        }
        else
        {
            NotAtSavePointText.text = "<color=\"red\">You can only switch Augments in Safe Places.</color>";
        }


        usedslot = 0;
        EquipedAugments = augmentscript.EquipedAugments;

        for(int i=0; i<14; ++i)
        {
            EquipedAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().color=new Color(1f,1f,1f,0f);
            EquipedAugmentsContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = false;
            if (i<=11)
            {
                DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                DisplayAugmentsContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable=false;
            }
            
        }

        for (int i = 0; i < EquipedAugments.Count; i++)
        {
            if (EquipedAugments[i])
            {
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().sprite = Augmentlist[i].image;
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(1).GetComponent<buttonscript>().ObjectID = i;
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(1).GetComponent<Button>().interactable = true;
                usedslot++;
            }
        }
        for (int i = 0; i < Mathf.Min(Augmentlist.Count - upperlineindex * 6, 12); i++)
        {
            DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            if (Augmentlist[i + upperlineindex * 6].locked)
            {
                DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Lockedsprite;
            }
            else
            {
                DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Augmentlist[i + upperlineindex * 6].image;
            }
            DisplayAugmentsContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;

            DisplayAugmentsContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = i + upperlineindex * 6;
        }
        
        Vector2 input = Vector2.zero;
        if(valueleft!=0 || valueright!=0 || valueup!=0 || valuedown!=0)
        {
            if(valuedown!=0)
            {
                input.y = -1;
            }
            else if (valueup!=0)
            {
                input.y = 1;
            }
            else if(valueleft!=0)
            {
                input.x = -1;
            }
            else if(valueright!=0)
            {
                input.x = 1;
            }
        }
        
        if(lastinput!=input && input !=Vector2.zero)
        {
            Direction(input);
        }
        lastinput = input;


        if (selected != null)
        {
            selected.Select();
            
            if(Augmentlist[selected.transform.GetComponent<buttonscript>().ObjectID].locked)
            {
                effecttext.text = "Undiscovered Augment.";
                necessaryslots.text = "Necessary slots : ?";
            }
            else
            {
                effecttext.text = Augmentlist[selected.transform.GetComponent<buttonscript>().ObjectID].description;
                necessaryslots.text = "Necessary slots : " + Augmentlist[selected.transform.GetComponent<buttonscript>().ObjectID].SlotsUsed;
                if (!pressedclick && valueclick == 1 && FindAnyObjectByType<Global>().atsavepoint)
                {
                    pressedclick = true;
                    selected.onClick.Invoke();
                }
            }
            
        }
        else
        {
            effecttext.text = "Choose an Augment to see its effect";
            necessaryslots.text = "Necessary slots : ";
        }

    }

    void OnDodge()
    {
        if (onquiped || !EquipedAugmentsContainer.GetChild(0).GetChild(1).GetComponent<Button>().interactable)
        {
            FindAnyObjectByType<Global>().CloseInventory();
        }
        else
        {
            selected = EquipedAugmentsContainer.GetChild(0).GetChild(1).GetComponent<Button>();
        }
    }
    public void Direction(Vector2 dirinput)
    {
        usedslot = 0;
        for (int i = 0; i < EquipedAugments.Count; i++)
        {
            if (EquipedAugments[i])
            {
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().sprite = Augmentlist[i].image;
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(1).GetComponent<buttonscript>().ObjectID = i;
                EquipedAugmentsContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
                usedslot++;
            }
        }
        for(int i = 0;i< Mathf.Min(Augmentlist.Count-upperlineindex*6,12);i++)
        {
            DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            DisplayAugmentsContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
            if (Augmentlist[i + upperlineindex * 6].locked)
            {
                DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Lockedsprite;
            }
            else
            {
                DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Augmentlist[i + upperlineindex * 6].image;
            }
            DisplayAugmentsContainer.GetChild(i).GetChild(1).GetComponent<buttonscript>().ObjectID = i + upperlineindex * 6;
        }
        for(int i = Augmentlist.Count - upperlineindex * 6;i<12;i++)
        {
            DisplayAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            DisplayAugmentsContainer.GetChild(i).GetChild(1).GetComponent<Button>().interactable = false;
        }
        if (dirinput != Vector2.zero)
        {
            if (selected == null && usedslot > 0)
            {
                selected = EquipedAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                onquiped = true;
                return;
            }
            else if (selected == null)
            {
                selected = DisplayAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
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
                    if (activebuttonidstring < usedslot - 1 && activebuttonidstring != 6 && activebuttonidstring != 13)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                    }
                    else if (activebuttonidstring == usedslot - 1)
                    {
                        if (activebuttonidstring <= 6)
                        {
                            selected = EquipedAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                        }
                        else
                        {
                            selected = EquipedAugmentsContainer.GetChild(7).GetChild(1).GetComponentInChildren<Button>();
                        }
                    }
                    else if (activebuttonidstring == 6 || activebuttonidstring == 13)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring - 6).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                else if (dirinput.x < 0)
                {
                    if (activebuttonidstring > 0 && activebuttonidstring != 7)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring - 1).GetChild(1).GetComponentInChildren<Button>();
                    }
                    else if (activebuttonidstring == 0 && usedslot <= 7)
                    {
                        if (usedslot <= 7)
                        {
                            selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + usedslot - 1).GetChild(1).GetComponentInChildren<Button>();
                        }
                        else
                        {
                            selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + 6).GetChild(1).GetComponentInChildren<Button>();
                        }
                    }
                    else if (activebuttonidstring == 7 && usedslot <= 14)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + usedslot - 8).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                else if (dirinput.y != 0)
                {
                    if (activebuttonidstring > 6 )
                    {
                        if(dirinput.y < 0)
                        {
                            selected = DisplayAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                            onquiped = false;
                        }
                        else
                        {
                            selected = EquipedAugmentsContainer.GetChild(activebuttonidstring - 7).GetChild(1).GetComponentInChildren<Button>();
                        }
                        
                    }
                    else if (activebuttonidstring <= 6 && activebuttonidstring + 7 <= usedslot - 1)
                    {
                        if(dirinput.y<0)
                        {
                            selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + 7).GetChild(1).GetComponentInChildren<Button>();
                        }
                        else
                        {
                            selected = DisplayAugmentsContainer.GetChild(activebuttonidstring).GetChild(1).GetComponentInChildren<Button>();
                        }
                    }
                    else if (dirinput.y < 0)
                    {
                        selected = DisplayAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                        onquiped = false;
                    }
                }
            }
        }
        else
        {
            if (dirinput != Vector2.zero)
            {
                if (dirinput.x > 0)
                {
                    switch(activebuttonidstring)
                    {
                        case < 5:
                            selected = DisplayAugmentsContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                            break;
                        case 5:
                            selected = DisplayAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                            break;
                        case 11:
                            selected = DisplayAugmentsContainer.GetChild(6).GetChild(1).GetComponentInChildren<Button>();
                            break;
                        case >5:
                            if(activebuttonidstring+1<Augmentlist.Count-upperlineindex*6)
                            {
                                selected = DisplayAugmentsContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                            }
                            else
                            {
                                selected = DisplayAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                            }
                            break;
                    }
                }
                else if (dirinput.x < 0)
                {
                    if(activebuttonidstring==0)
                    {
                        selected = DisplayAugmentsContainer.GetChild(5).GetChild(1).GetComponentInChildren<Button>();
                    }
                    else if(activebuttonidstring==6)
                    {
                        selected = DisplayAugmentsContainer.GetChild(Mathf.Min(11, Augmentlist.Count - upperlineindex * 6)).GetChild(1).GetComponentInChildren<Button>();
                    }
                    else
                    {
                        selected = DisplayAugmentsContainer.GetChild(activebuttonidstring-1).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                else if (dirinput.y > 0)
                {
                    if(activebuttonidstring<=5)
                    {
                        if(upperlineindex==0)
                        {
                            if(usedslot>0)
                            {
                                onquiped = true;
                                selected = EquipedAugmentsContainer.GetChild(0).GetChild(1).GetComponentInChildren<Button>();
                            }
                        }
                        else
                        {
                            upperlineindex--;
                        }
                        
                    }
                    else
                    {
                        selected = DisplayAugmentsContainer.GetChild(activebuttonidstring - 6).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                else if (dirinput.y < 0)
                {
                    if(activebuttonidstring<=5)
                    {
                        if(Augmentlist.Count-upperlineindex*6 > 12)
                        {
                            selected = DisplayAugmentsContainer.GetChild(activebuttonidstring + 6).GetChild(1).GetComponentInChildren<Button>();
                        }
                        else if(Augmentlist.Count-upperlineindex*6> activebuttonidstring +6)
                        {
                            selected = DisplayAugmentsContainer.GetChild(activebuttonidstring + 6).GetChild(1).GetComponentInChildren<Button>();
                        }
                    }
                    else if (Augmentlist.Count>12+upperlineindex*6+1)
                    {
                        upperlineindex++;
                        selected = DisplayAugmentsContainer.GetChild(Mathf.Min(activebuttonidstring, Augmentlist.Count - upperlineindex * 6)).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
            }
        }
        
        
        
        
    }
    void OnEnable()
    {
        if(controls!=null)
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
