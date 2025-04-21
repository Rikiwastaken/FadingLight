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

    public bool inmenu_inv_shop;

    public bool inshop;
    public bool ininventory;
    public bool indialogue;
    public bool inbossfight;
    public bool zipping;
    public bool grappling;
    public bool inpause;

    public bool clickednewgame;

    public List<bool> worldflags;

    public float ManualClock;
    public bool usemanualclock;
    public SavePointScript activeSavePoint;

    public GameObject PauseMenu;

    public GameObject MattShopMenu;

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

    public void ResetVariables()
    {
        inbossfight = false;
        ininventory = false;
        inpause = false;
        atsavepoint = false;
        indialogue = false;
        zipping = false;
        grappling = false;
        Time.timeScale = 1;
        for (int i = 0; i < worldflags.Count; i++)
        {
            worldflags[i] = false;
        }
        currentinventory = null;
        lastinv = null;
        activeSavePoint = null;
    }
    private void Update()
    {
        if(FindAnyObjectByType<ShopScript>())
        {
            GameObject newshop = FindAnyObjectByType<ShopScript>().gameObject;
            if (newshop.GetComponent<ShopScript>().npc.name == "Matt" && !inshop && MattShopMenu !=newshop)
            {
                MattShopMenu = newshop;
                MattShopMenu.SetActive(false);
            }
        }
        

    inmenu_inv_shop = ininventory || indialogue || inpause || atsavepoint || indialogue || inshop;

        if(SceneManager.GetActiveScene().name == "Start" || SceneManager.GetActiveScene().name == "MainMenu")
        {
            ResetVariables();
        }
        if( inpause)
        {
            if(!FindAnyObjectByType<PauseMenu>())
            {
                inpause = false;
                Time.timeScale = 1;
            }
        }

        if(usemanualclock)
        {
            Time.timeScale = ManualClock;
        }


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

        if(currentinventory != null && !indialogue && FindAnyObjectByType<PlayerHP>().Eldonhp > 0 && FindAnyObjectByType<PauseMenu>() == null && !inpause)
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

    void OnMenu()
    {
        if (atsavepoint)
        {

            if (ininventory)
            {
                CloseInventory();
            }
            else if (activeSavePoint.fadetoblack.color.a <= 0.001f)
            {
                atsavepoint = false;
                Destroy(FindAnyObjectByType<SavePointMenu>().gameObject);
            }
        }
        if (SceneManager.GetActiveScene().name!="Start" && SceneManager.GetActiveScene().name != "MainMenu" && !inmenu_inv_shop && FindAnyObjectByType<PlayerHP>().Eldonhp>0 && !inmenu_inv_shop)
        {
            if(FindAnyObjectByType<PauseMenu>()==null)
            {
                Instantiate(PauseMenu, GameObject.Find("Canvas").transform);
            }
        }
        if (indialogue)
        {
            FindAnyObjectByType<DialogueManager>().AccelerateOrClose();
        }
        
    }

    void OnDodge()
    {

        
        if(indialogue)
        {
            FindAnyObjectByType<DialogueManager>().AccelerateOrClose();
        }
        if(atsavepoint)
        {
            
            if(ininventory)
            {
                CloseInventory();
            }
            else if(activeSavePoint.fadetoblack.color.a<=0.001f)
            {
                atsavepoint = false;
                Destroy(FindAnyObjectByType<SavePointMenu>().gameObject);
            }
        }
    }

    void OnNorthButton()
    {
        ShowSaveSlots showSaveSlots = FindAnyObjectByType<ShowSaveSlots>();
        if(showSaveSlots != null)
        {
            if(showSaveSlots.GetComponent<basicmenunav>().selected!=null)
            {
                string name = showSaveSlots.GetComponent<basicmenunav>().selected.transform.name;
                int SlotID = int.Parse(name[name.Length-1]+"");
                GetComponent<SaveManager>().DeleteSlot(SlotID);
                SceneManager.LoadScene("MainMenu");
            }
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
        if (currentinventory != null && !indialogue)
        {
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
            }
        }
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
