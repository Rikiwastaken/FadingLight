using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CreateItemWindow;

public class CreationItemPrefab : MonoBehaviour
{
    public TextMeshProUGUI itemname;
    public TextMeshProUGUI metal;
    public TextMeshProUGUI cores;
    public TextMeshProUGUI electronic;
    public Image image;

    public CreationItem item;

    private PlayerHP playerHP;
    private AugmentsScript AugmentsScript;
    private GadgetScript GadgetScript;
    private ShopScript ShopScript;

    // Start is called before the first frame update
    void Start()
    {
        playerHP = FindAnyObjectByType<PlayerHP>();
        AugmentsScript = playerHP.GetComponent<AugmentsScript>();
        GadgetScript = playerHP.GetComponent<GadgetScript>();
        ShopScript = FindAnyObjectByType<ShopScript>();
    }

    public void InitialSetup(CreationItem newitem)
    {
        item = newitem;
        if (item.augment != null)
        {
            itemname.text = item.augment.name;
            image.sprite = item.augment.image;
        }
        else if(item.gadget != null)
        {
            itemname.text = item.gadget.name;
            image.sprite = item.gadget.image;   
        }
        
        metal.text = " : " + item.metalprice;
        cores.text = " : " + item.coreprice;
        electronic.text = " : " + item.electronicprice;
    }

    public void TryToCreate()
    {
        if(item.augment!=null)
        {
            if (playerHP.MetalScrap >= item.metalprice && playerHP.CorePieces >= item.coreprice && playerHP.ElectronicComponents >= item.electronicprice && AugmentsScript.Augmentlist[item.augment.ID].locked)
            {
                playerHP.MetalScrap -= item.metalprice;
                playerHP.CorePieces -= item.coreprice;
                playerHP.ElectronicComponents -= item.electronicprice;
                AugmentsScript.Augmentlist[item.augment.ID].locked = false;
            }
            else
            {
                ShopScript.PlayNotEnoughCash();
            }
        }
        else if (item.gadget != null)
        {
            if (playerHP.MetalScrap >= item.metalprice && playerHP.CorePieces >= item.coreprice && playerHP.ElectronicComponents >= item.electronicprice && GadgetScript.GadgetList[item.gadget.ID].locked)
            {
                playerHP.MetalScrap -= item.metalprice;
                playerHP.CorePieces -= item.coreprice;
                playerHP.ElectronicComponents -= item.electronicprice;
                GadgetScript.GadgetList[item.gadget.ID].locked = false;
            }
            else
            {
                ShopScript.PlayNotEnoughCash();
            }
        }
        
    }
}
