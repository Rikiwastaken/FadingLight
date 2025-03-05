using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Resolutionscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Toggle>() != null)
        {
            GetComponent<Toggle>().isOn = Screen.fullScreen;
            GetComponent<Toggle>().onValueChanged.AddListener(delegate { changefullscreen(); });
        }

        
    }

    public void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        Debug.Log(Screen.currentResolution);
        switch (dropdown.value)
        {
            case 0:
                Screen.SetResolution(1024, 576, Screen.fullScreenMode);
                break;

            case 1:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case 2:
                Screen.SetResolution(1600, 900, Screen.fullScreenMode);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case 4:
                Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
                break;
        }
        
        
    }

    public void changefullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
