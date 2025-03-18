using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonscript : MonoBehaviour
{
    public int ObjectID;
    private AugmentsScript AugmentsScript;
    private AugmentsMenuWindow AugmentsMenuWindow;
    private EquipmentScript EquipmentScript;
    private EquipmentMenuWindow EquipmentMenuWindow;
    public int equipmentslotID;

    private void Start()
    {
        AugmentsScript = FindAnyObjectByType<AugmentsScript>();
        AugmentsMenuWindow = FindAnyObjectByType<AugmentsMenuWindow>();
        EquipmentScript = FindAnyObjectByType<EquipmentScript>();
        EquipmentMenuWindow = FindAnyObjectByType<EquipmentMenuWindow>();
    }
    public void EquipedAugmentButton()
    {
        if(FindAnyObjectByType<Global>().atsavepoint)
        {
            AugmentsScript.UnEquipAugment(ObjectID);
            AugmentsMenuWindow.selected = null;
        }
    }

    public void AllAugmentsButton()
    {
        if(!AugmentsScript.Augmentlist[ObjectID].locked && FindAnyObjectByType<Global>().atsavepoint)
        {
            AugmentsScript.EquipAugment(ObjectID);
        }
    }

    public void SelectEquipmentSection()
    {
        EquipmentMenuWindow.selectedsection = equipmentslotID;
    }

    public void EquipselectedEquipment()
    {
        if(FindAnyObjectByType<Global>().atsavepoint)
        {
            EquipmentScript.EquipItem(equipmentslotID, ObjectID);
        }
        
    }

    public void SavePointLeave()
    {
        FindAnyObjectByType<Global>().atsavepoint = false;
        Destroy(transform.parent.parent.gameObject);
    }

    public void SavePointAugment()
    {
        FindAnyObjectByType<Global>().OpenInventory(2);
    }
    public void SavePointEquipment()
    {
        FindAnyObjectByType<Global>().OpenInventory(1);
    }
    public void SavePointSave()
    {
        FindAnyObjectByType<SavePointMenu>().floppycounter = (int)(3/Time.deltaTime);
        FindAnyObjectByType<SaveManager>().SaveToFile();
    }

    public void LoadSaveSlot()
    {
        int slot = int.Parse(transform.name.Replace("Slot", ""));
        FindAnyObjectByType<SaveManager>().CurrentSlot = slot;
        FindAnyObjectByType<SaveManager>().LoadSave();
    }

    public void MainMenu()
    {
        FindAnyObjectByType<Global>().inbossfight = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void ClosePauseMenu()
    {
        Destroy(FindAnyObjectByType<PauseMenu>().gameObject);
    }

    public void GameOverLoadSaveSlot()
    {
        FindAnyObjectByType<SaveManager>().LoadSave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnlockAll()
    {
        foreach(EquipmentScript.Chain chain in EquipmentScript.Chainslist)
        {
            chain.locked = false;
        }
        foreach (EquipmentScript.Plate plate in EquipmentScript.Platelist)
        {
            plate.locked = false;
        }
        foreach (SupportDrone.HealerDrone drone in EquipmentScript.drone1.GetComponent<SupportDrone>().drones)
        {
            
            drone.locked = false;
        }
        foreach (SupportDrone.HealerDrone drone in EquipmentScript.drone2.GetComponent<SupportDrone>().drones)
        {
            drone.locked = false;
        }
        foreach(AugmentsScript.Augment augment in AugmentsScript.Augmentlist)
        {
            augment.locked = false;
        }
        foreach (GadgetScript.Gadget gadget in FindAnyObjectByType<GadgetScript>().GadgetList)
        {
            gadget.locked = false;
        }
        AugmentsScript.numberofSlotexpansionspickedup = 100;
    }

}
