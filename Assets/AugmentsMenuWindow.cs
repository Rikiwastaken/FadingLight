using System.Collections;
using System.Collections.Generic;
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
    private List<Augment> Augmentlist;
    private List<bool> EquipedAugments;

    private AugmentsScript augmentscript;

    public Button selected;

    private int usedslot;
    private int valueleft;
    private int valueright;
    private int valuedown;
    private int valueup;
    private Vector2 lastinput;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        augmentscript = FindAnyObjectByType<AugmentsScript>();

        Augmentlist = augmentscript.Augmentlist;

        controls = new PlayerControls();

        controls.gameplay.moveleft.performed += ctx => valueleft = 1;
        controls.gameplay.moveright.performed += ctx => valueright = 1;
        controls.gameplay.moveleft.canceled += ctx => valueleft = 0;
        controls.gameplay.moveright.canceled += ctx => valueright = 0;
        controls.gameplay.down.performed += ctx => valuedown = 1;
        controls.gameplay.down.canceled += ctx => valuedown = 0;
        controls.gameplay.up.performed += ctx => valueup = 1;
        controls.gameplay.up.canceled += ctx => valueup = 0;

    }
    // Update is called once per frame
    void Update()
    {
        usedslot = 0;
        EquipedAugments = augmentscript.EquipedAugments;

        for(int i=0; i<14; ++i)
        {
            EquipedAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().color=new Color(1f,1f,1f,0f);
        }

        for (int i = 0; i < EquipedAugments.Count; i++)
        {
            if (EquipedAugments[i])
            {
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().sprite = Augmentlist[i].image;
                usedslot++;
            }
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
            if(valueleft!=0)
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
                usedslot++;
            }
        }

        
        if(dirinput!=Vector2.zero)
        {
            if(selected==null && usedslot > 0)
            {
                selected = EquipedAugmentsContainer.GetChild(usedslot).GetChild(1).GetComponentInChildren<Button>();
            }
            else if(selected!=null)
            {
                int activebuttonidstring = int.Parse(selected.transform.parent.name.Replace("Slot", "0"));
                if(dirinput.x>0)
                {
                    if(activebuttonidstring<usedslot-1)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + 1).GetChild(1).GetComponentInChildren<Button>();
                    }
                    else if(activebuttonidstring==6 || activebuttonidstring == 13)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring - 6).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                if (dirinput.x < 0)
                {
                    if (activebuttonidstring > 0)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring - 1).GetChild(1).GetComponentInChildren<Button>();
                    }
                    else if (activebuttonidstring == 0 || activebuttonidstring == 7)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + 6).GetChild(1).GetComponentInChildren<Button>();
                    }
                }
                if (dirinput.y !=0)
                {
                    if(activebuttonidstring>6)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring - 7).GetChild(1).GetComponentInChildren<Button>();
                    }
                    if (activebuttonidstring <= 6 && activebuttonidstring+7<=usedslot-1)
                    {
                        selected = EquipedAugmentsContainer.GetChild(activebuttonidstring + 7).GetChild(1).GetComponentInChildren<Button>();
                    }
                }

            }
        }
        if(selected != null)
        {
            selected.Select();
        }
        
    }
    void OnEnable()
    {
        controls.gameplay.Enable();
    }
    void OnDisable()
    {
        controls.gameplay.Disable();
    }

}
