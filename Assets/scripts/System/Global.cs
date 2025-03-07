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

    public bool ininventory;
    public bool indialogue;

    public bool clickednewgame;

    public List<bool> worldflags;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        DontDestroyOnLoad(gameObject);
        if(SceneManager.GetActiveScene().name == "Start")
        {
            SceneManager.LoadScene("MainMenu");
        }

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

        if(currentinventory != null && !indialogue)
        {
            if(ininventory)
            {
                Time.timeScale = 1f;
                CloseInventory();
            }
            else if(!atsavepoint)
            {
                Time.timeScale = 0f;
                OpenInventory();
            } 
        }
    }

    void OnDodge()
    {

        if (currentinventory != null && !indialogue)
        {
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
                
            }
            if(ininventory)
            {
                CloseInventory();
            }

        }
        if(indialogue)
        {
            FindAnyObjectByType<DialogueManager>().AccelerateOrClose();
        }
    }

    void OnJump()
    {
        if (indialogue)
        {
            FindAnyObjectByType<DialogueManager>().AccelerateOrClose();
        }
    }

    public void CloseInventory()
    {
        currentinventory.SetActive(false);
        ininventory=false;
        closedmenu = true;
    }

    public void OpenInventory()
    {
        currentinventory.SetActive(true);
        ininventory=true;
    }
    public void OpenInventory(int page)
    {
        currentinventory.SetActive(true);
        ininventory=true;
        currentinventory.GetComponent<InventoryScript>().SetPage(page);
    }

    public void LoadSave()
    {
        
    }

    private void OnEnable()
    {
        Global[] globlist = FindObjectsByType<Global>(FindObjectsSortMode.InstanceID);
        for(int i = 1; i < globlist.Length; i++)
        {
            Destroy(globlist[i]);
        }
    }

    public void TriggerWorldFlag(int flagID)
    {
        worldflags[flagID] = true;
    }
}
