using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayInputScript : MonoBehaviour
{
    public GameObject LT;
    public GameObject RT;
    public GameObject LB;
    public GameObject RB;
    public GameObject NorthButton;
    public GameObject SouthButton;
    public GameObject EastButton;
    public GameObject WestButton;
    public GameObject StartButton;
    public GameObject SelectButton;
    public GameObject LeftStick;
    public GameObject RightStick;
    public GameObject DPadUp;
    public GameObject DPadDown;
    public GameObject DPadLeft;
    public GameObject DPadRight;

    public PlayerControls controls;

    private Vector2 leftstickoffset;

    private Vector2 leftstickbasepos;

    // Start is called before the first frame update
    void Start()
    {
        leftstickbasepos = LeftStick.transform.position;
        controls = new PlayerControls();

        // Dpad
        controls.gameplay.crossleft.performed += ctx => DPadLeft.SetActive(true);
        controls.gameplay.crossright.performed += ctx => DPadRight.SetActive(true);
        controls.gameplay.crossleft.canceled += ctx => DPadLeft.SetActive(false);
        controls.gameplay.crossright.canceled += ctx => DPadRight.SetActive(false);
        controls.gameplay.crossdown.performed += ctx => DPadDown.SetActive(true);
        controls.gameplay.crossdown.canceled += ctx => DPadDown.SetActive(false);
        controls.gameplay.crossup.performed += ctx => DPadUp.SetActive(true);
        controls.gameplay.crossup.canceled += ctx => DPadUp.SetActive(false);

        //buttons
        controls.gameplay.jump.performed += ctx => SouthButton.SetActive(true);
        controls.gameplay.jump.canceled += ctx => SouthButton.SetActive(false);
        controls.gameplay.dodge.performed += ctx => EastButton.SetActive(true);
        controls.gameplay.dodge.canceled += ctx => EastButton.SetActive(false);
        controls.gameplay.attack.performed += ctx => WestButton.SetActive(true);
        controls.gameplay.attack.canceled += ctx => WestButton.SetActive(false);
        controls.gameplay.NorthButton.performed += ctx => NorthButton.SetActive(true);
        controls.gameplay.NorthButton.canceled += ctx => NorthButton.SetActive(false);

        //Triggers and shoulder buttons
        controls.gameplay.LeftShoulder.performed += ctx => LB.SetActive(true);
        controls.gameplay.LeftShoulder.canceled += ctx => LB.SetActive(false);
        controls.gameplay.RightShoulder.performed += ctx => RB.SetActive(true);
        controls.gameplay.RightShoulder.canceled += ctx => RB.SetActive(false);
        controls.gameplay.LeftTrigger.performed += ctx => LT.SetActive(true);
        controls.gameplay.LeftTrigger.canceled += ctx => LT.SetActive(false);
        controls.gameplay.RightTrigger.performed += ctx => RT.SetActive(true);
        controls.gameplay.RightTrigger.canceled += ctx => RT.SetActive(false);

        //start et select
        controls.gameplay.menu.performed += ctx => StartButton.SetActive(true);
        controls.gameplay.menu.canceled += ctx => StartButton.SetActive(false);
        controls.gameplay.Inventory.performed += ctx => SelectButton.SetActive(true);
        controls.gameplay.Inventory.canceled += ctx => SelectButton.SetActive(false);

        //sticks

        controls.gameplay.moveleft.performed += ctx => leftstickoffset.x = -1;
        controls.gameplay.moveright.performed += ctx => leftstickoffset.x = 1;
        controls.gameplay.moveleft.canceled += ctx => leftstickoffset.x = 0;
        controls.gameplay.moveright.canceled += ctx => leftstickoffset.x = 0;
        controls.gameplay.down.performed += ctx => leftstickoffset.y = -1;
        controls.gameplay.down.canceled += ctx => leftstickoffset.y = 0;
        controls.gameplay.up.performed += ctx => leftstickoffset.y = 1;
        controls.gameplay.up.canceled += ctx => leftstickoffset.y = 0;

        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        controls.Enable();

    }

    private void Update()
    {
        if(leftstickoffset!=Vector2.zero)
        {
            LeftStick.SetActive(true);
            LeftStick.transform.position = leftstickoffset*10 + leftstickbasepos;
        }
        else
        {
            LeftStick.SetActive(false);
        }
    }
}
