using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class basicmenunav : MonoBehaviour
{
    private int valuedown;
    private int valueup;
    private int valueclick;
    private int lastinput;
    private int valuestickdown;
    private int valuestickup;

    private bool pressedclick;
    public Button selected;
    int activebuttonID;

    PlayerControls controls;

    // Start is called before the first frame update
    void OnEnable()
    {
        controls = new PlayerControls();



        controls.gameplay.crossdown.performed += ctx => valuedown = 1;
        controls.gameplay.crossdown.canceled += ctx => valuedown = 0;
        controls.gameplay.crossup.performed += ctx => valueup = 1;
        controls.gameplay.crossup.canceled += ctx => valueup = 0;
        controls.gameplay.jump.performed += ctx => valueclick = 1;
        controls.gameplay.jump.canceled += ctx => valueclick = 0;


        controls.gameplay.down.performed += ctx => valuestickdown = 1;
        controls.gameplay.down.canceled += ctx => valuestickdown = 0;
        controls.gameplay.up.performed += ctx => valuestickup = 1;
        controls.gameplay.up.canceled += ctx => valuestickup = 0;

        controls.gameplay.Enable();


    }

    // Update is called once per frame
    void Update()
    {

        if (valueclick == 0)
        {
            pressedclick = false;
        }



        int inputstick = 0;
        if (valuestickup != 0 || valuestickdown != 0)
        {
            if (valuestickdown != 0)
            {
                inputstick = -1;
            }
            else if (valuestickup != 0)
            {
                inputstick = 1;
            }
        }

        int input = 0;
        if ( valueup != 0 || valuedown != 0)
        {
            if (valuedown != 0)
            {
                input = -1;
            }
            else if (valueup != 0)
            {
                input = 1;
            }
        }

        if (input == 0 && inputstick != 0)
        {
            input = inputstick;
        }

        if (lastinput != input && input != 0)
        {
            Direction(input);
        }
        lastinput = input;


        if (selected != null)
        {
            selected.Select();

            if (!pressedclick && valueclick == 1)
            {
                pressedclick = true;
                selected.onClick.Invoke();
                selected = null;
            }

        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable)
            {
                transform.GetChild(i).GetComponent<Image>().color = Color.gray;
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }

    }
    public void Direction(int dirinput)
    {

        

        if (selected == null)
        {
            activebuttonID = -1;
            getfirstactive();
            return;
        }
        if (dirinput > 0)
        {
            if (activebuttonID > 0)
            {
                activebuttonID -= 1;
                while(activebuttonID >=0)
                {
                    if (transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>().interactable)
                    {
                        selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
                        return;
                    }
                    else
                    {
                        activebuttonID -= 1;

                    }
                }
                if(activebuttonID < 0)
                {
                    getlastactive();
                }
                
            }
            else
            {
                activebuttonID = transform.childCount;
                getlastactive();

            }
            return;
        }
        if (dirinput < 0)
        {
            if (activebuttonID < transform.childCount - 1)
            {
                while (activebuttonID < transform.childCount - 1)
                {
                    
                    activebuttonID++;
                    if (transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>().interactable)
                    {
                        selected = transform.GetChild(activebuttonID).GetChild(0).GetComponent<Button>();
                        return;
                    }
                }
                if (activebuttonID >= transform.childCount - 1)
                {
                    getfirstactive();
                }
            }
            else
            {
                getfirstactive();
            }
        }
    }

    void getfirstactive()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable)
            {
                selected = transform.GetChild(i).GetChild(0).GetComponent<Button>();
                activebuttonID = i;
                return;
            }

        }

    }

    void getlastactive()
    {
        for (int i = transform.childCount-1; i >=0; i--)
        {
            if (transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable)
            {
                selected = transform.GetChild(i).GetChild(0).GetComponent<Button>();
                activebuttonID = i;
                return;
            }

        }

    }
}
