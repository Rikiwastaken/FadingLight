using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static AugmentsScript;
using static UnityEngine.InputSystem.InputAction;

public class SavePointMenu : MonoBehaviour
{
    public PlayerControls controls;

    private int valuedown;
    private int valueup;
    private int valueclick;
    private int lastinput;

    private bool pressedclick;
    public Button selected;
    int activebuttonID;

    public int floppycounter;
    public Image Floppy;

    public bool unlockallavailable;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        


        controls.gameplay.crossdown.performed += ctx => valuedown = 1;
        controls.gameplay.crossdown.canceled += ctx => valuedown = 0;
        controls.gameplay.crossup.performed += ctx => valueup = 1;
        controls.gameplay.crossup.canceled += ctx => valueup = 0;
        controls.gameplay.down.performed += ctx => valuedown = 1;
        controls.gameplay.down.canceled += ctx => valuedown = 0;
        controls.gameplay.up.performed += ctx => valueup = 1;
        controls.gameplay.up.canceled += ctx => valueup = 0;
        controls.gameplay.jump.performed += ctx => valueclick = 1;
        controls.gameplay.jump.canceled += ctx => valueclick = 0;

        controls.gameplay.Enable();
        if(!unlockallavailable)
        {
            transform.GetChild(transform.childCount-2).gameObject.SetActive(false);
        }


    }
    // Update is called once per frame
    void Update()
    {
        FindAnyObjectByType<Global>().atsavepoint = true;
        if(valueclick==0)
        {
            pressedclick = false;
        }

        if(floppycounter>0)
        {
            floppycounter--;
            Floppy.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            Floppy.color = new Color(1f, 1f, 1f, 0f);
        }
        
        int input = 0;
        if( valueup!=0 || valuedown!=0)
        {
            if(valuedown!=0)
            {
                input = -1;
            }
            else if (valueup!=0)
            {
                input = 1;
            }
            
        }
        
        if(lastinput!=input && input !=0 && !FindAnyObjectByType<Global>().ininventory)
        {
            Direction(input);
        }
        lastinput = input;


        if (selected != null && !FindAnyObjectByType<Global>().ininventory)
        {
            selected.Select();

            if (!pressedclick && valueclick == 1)
            {
                pressedclick = true;
                selected.onClick.Invoke();
            }

        }

    }
    public void Direction(int dirinput)
    {
        
        if(selected==null)
        {
            selected = transform.GetChild(0).GetChild(0).GetComponent<Button>();
            activebuttonID = 0;
            return;
        }
        if(dirinput>0)
        {
            if(activebuttonID>0)
            {
                activebuttonID -=1;
                selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
            }
            else
            {
                if(unlockallavailable)
                {
                    activebuttonID = transform.childCount - 2;
                    selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
                }
                else
                {
                    activebuttonID = transform.childCount - 4;
                    selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
                }
                
            }
            return;
        }
        if (dirinput < 0)
        {
            if (unlockallavailable)
            {
                if (activebuttonID < transform.childCount - 2)
                {
                    activebuttonID += 1;
                    selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
                }
                else
                {
                    activebuttonID = 0;
                    selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
                }
            }
            else
            {
                if (activebuttonID < transform.childCount - 4)
                {
                    activebuttonID += 1;
                    selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
                }
                else
                {
                    activebuttonID = 0;
                    selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
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
