using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class buttonscript : MonoBehaviour
{
    public int AugmentID;
    private AugmentsScript AugmentsScript;
    private AugmentsMenuWindow AugmentsMenuWindow;

    private void Start()
    {
        AugmentsScript = FindAnyObjectByType<AugmentsScript>();
        AugmentsMenuWindow = FindAnyObjectByType<AugmentsMenuWindow>();
    }
    public void EquipedAugmentButton()
    {
        AugmentsScript.UnEquipAugment(AugmentID);
        AugmentsMenuWindow.selected=null;
    }

    public void AllAugmentsButton()
    {
        AugmentsScript.EquipAugment(AugmentID);
    }

}
