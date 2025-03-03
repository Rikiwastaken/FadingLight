using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{

    public GameObject currentinventory;

    public GameObject lastinv;

    public bool closedmenu;
    // Start is called before the first frame update

    public bool atsavepoint;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //SceneManager.LoadScene("MainMenu");
        Screen.SetResolution(1920, 1080,false);
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if(GameObject.FindAnyObjectByType<InventoryScript>())
        {
            
            currentinventory = GameObject.FindAnyObjectByType<InventoryScript>().gameObject;
        }
        
        if(currentinventory != lastinv && currentinventory!=null)
        {
            lastinv = currentinventory;
            currentinventory.SetActive(false);
        }
    }

    void OnInventory()
    {

        if(currentinventory != null)
        {
            if(Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
                currentinventory.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                currentinventory.SetActive(true);
            }
            
        }
    }

    void OnDodge()
    {

        if (currentinventory != null)
        {
            if (Time.timeScale == 0f)
            {
                closedmenu = true;
                Time.timeScale = 1f;
                currentinventory.SetActive(false);
            }

        }
    }
}
