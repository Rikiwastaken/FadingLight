using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

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
        AugmentsScript.UnEquipAugment(ObjectID);
        AugmentsMenuWindow.selected=null;
    }

    public void AllAugmentsButton()
    {
        if(!AugmentsScript.Augmentlist[ObjectID].locked)
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
        EquipmentScript.EquipItem(equipmentslotID, ObjectID);
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
        Debug.Log("Save");
    }

}
