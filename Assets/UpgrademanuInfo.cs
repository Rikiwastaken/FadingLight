using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgrademanuInfo : MonoBehaviour
{
    [System.Serializable]
    public class prices
    {
        public int metalprice;
        public int coreprice;
        public int electronicprice;
    }


    public TextMeshProUGUI descriptionText;

    public Button selectedbutton;
    public Button chainbutton;
    public Button platebutton;
    public Button Slotbutton;

    public List<TextMeshProUGUI> ChainPriceText;
    public List<TextMeshProUGUI> PlatePriceText;
    public List<TextMeshProUGUI> SlotPriceText;

    private prices ChainPrice;
    private prices PlatePrice;
    private prices SlotPrice;

    public List<prices> pricetiers;

    public GameObject MainShopMenu;

    private EquipmentScript EquipmentScript;
    private AugmentsScript AugmentsScript;
    private PlayerHP PlayerHP;

    private void Start()
    {
        EquipmentScript = FindAnyObjectByType<EquipmentScript>();
        AugmentsScript = FindAnyObjectByType<AugmentsScript>();
        PlayerHP = FindAnyObjectByType<PlayerHP>();
    }

    public void OnDodge()
    {
        MainShopMenu.SetActive(true);
        MainShopMenu.GetComponent<basicmenunav>().resetselection();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        selectedbutton = GetComponent<basicmenunav>().selected;
        if(selectedbutton == null)
        {
            descriptionText.text = "";
        }
        else if (selectedbutton == chainbutton)
        {
            descriptionText.text = "Improves Eldon's chainsaw and increases the base damage by 10 %. Can be improved up to level V.";
        }
        else if (selectedbutton == platebutton)
        {
            descriptionText.text = "Improves Eldon's prosthetic by reiforcing its armor and increases the base defense by 10 %. Can be improved up to level V.";
        }
        else if (selectedbutton == Slotbutton)
        {
            descriptionText.text = "Improves Eldon's prosthetic's capacity and increases the number of Augment Slots by 2. Can be improved up to level V.";
        }
        else
        {
            descriptionText.text = "";
        }

        ChainPrice = pricetiers[EquipmentScript.ChainUpgradeLevel];
        PlatePrice = pricetiers[EquipmentScript.PlateUpgradeLevel];
        SlotPrice = pricetiers[AugmentsScript.SlotUpgradeLevel];

        ChainPriceText[0].text = " : " + (int)(ChainPrice.metalprice*0.75f);
        ChainPriceText[1].text = " : " + (int)(ChainPrice.coreprice * 1.5f);
        ChainPriceText[2].text = " : " + (int)(ChainPrice.electronicprice*0.75f);

        switch (EquipmentScript.ChainUpgradeLevel)
        {
            case 0:
                ChainPriceText[3].text = "Blade lvl I";
                break;
            case 1:
                ChainPriceText[3].text = "Blade lvl II";
                break;
            case 2:
                ChainPriceText[3].text = "Blade lvl III";
                break;
            case 3:
                ChainPriceText[3].text = "Blade lvl IV";
                break;
            case 4:
                ChainPriceText[3].text = "Blade lvl V";
                break;
            default:
                ChainPriceText[3].text = "Blade at Max Level";
                break;
        }


        PlatePriceText[0].text = " : " + (int)(PlatePrice.metalprice * 1.5f);
        PlatePriceText[1].text = " : " + (int)(PlatePrice.coreprice * 0.75f);
        PlatePriceText[2].text = " : " + (int)(PlatePrice.electronicprice * 0.75f);

        switch(EquipmentScript.PlateUpgradeLevel)
        {
            case 0:
                PlatePriceText[3].text = "Armor lvl I";
                break;
            case 1:
                PlatePriceText[3].text = "Armor lvl II";
                break;
            case 2:
                PlatePriceText[3].text = "Armor lvl III";
                break;
            case 3:
                PlatePriceText[3].text = "Armor lvl IV";
                break;
            case 4:
                PlatePriceText[3].text = "Armor lvl V";
                break;
            default:
                PlatePriceText[3].text = "Armor at Max Level";
                break;
        }

        SlotPriceText[0].text = " : " + (int)(SlotPrice.metalprice * 0.75f);
        SlotPriceText[1].text = " : " + (int)(SlotPrice.coreprice * 0.75f);
        SlotPriceText[2].text = " : " + (int)(SlotPrice.electronicprice * 1.5f);



        switch (AugmentsScript.SlotUpgradeLevel)
        {
            case 0:
                SlotPriceText[3].text = "Aug. Slots lvl I";
                break;
            case 1:
                SlotPriceText[3].text = "Aug. Slots lvl II";
                break;
            case 2:
                SlotPriceText[3].text = "Aug. Slots lvl III";
                break;
            case 3:
                SlotPriceText[3].text = "Aug. Slots lvl IV";
                break;
            case 4:
                SlotPriceText[3].text = "Aug. Slots lvl V";
                break;
            default:
                SlotPriceText[3].text = "Aug. Slots at Max Level";
                break;
        }

    }

    public void TryUpgradeChain()
    {
        if(EquipmentScript.ChainUpgradeLevel<5)
        {
            if (PlayerHP.MetalScrap >= pricetiers[EquipmentScript.ChainUpgradeLevel].metalprice * 0.75f && PlayerHP.CorePieces >= pricetiers[EquipmentScript.ChainUpgradeLevel].coreprice * 1.5f && PlayerHP.ElectronicComponents >= pricetiers[EquipmentScript.ChainUpgradeLevel].electronicprice * 0.75f)
            {
                PlayerHP.MetalScrap -= (int)(pricetiers[EquipmentScript.ChainUpgradeLevel].metalprice * 0.75f);
                PlayerHP.CorePieces -= (int)(pricetiers[EquipmentScript.ChainUpgradeLevel].coreprice * 1.5f);
                PlayerHP.ElectronicComponents -= (int)(pricetiers[EquipmentScript.ChainUpgradeLevel].electronicprice * 0.75f);
                EquipmentScript.ChainUpgradeLevel++;
            }
            else
            {
                FindAnyObjectByType<ShopScript>().PlayNotEnoughCash();
            }
        }
    }

    public void TryUpgradePlate()
    {
        if (EquipmentScript.PlateUpgradeLevel < 5)
        {
            if (PlayerHP.MetalScrap >= pricetiers[EquipmentScript.PlateUpgradeLevel].metalprice * 1.5f && PlayerHP.CorePieces >= pricetiers[EquipmentScript.PlateUpgradeLevel].coreprice * 0.75f && PlayerHP.ElectronicComponents >= pricetiers[EquipmentScript.PlateUpgradeLevel].electronicprice * 0.75f)
            {
                PlayerHP.MetalScrap -= (int)(pricetiers[EquipmentScript.PlateUpgradeLevel].metalprice * 1.5f);
                PlayerHP.CorePieces -= (int)(pricetiers[EquipmentScript.PlateUpgradeLevel].coreprice * 0.75f);
                PlayerHP.ElectronicComponents -= (int)(pricetiers[EquipmentScript.PlateUpgradeLevel].electronicprice * 0.75f);
                EquipmentScript.PlateUpgradeLevel++;
            }
            else
            {
                FindAnyObjectByType<ShopScript>().PlayNotEnoughCash();
            }
        }
    }

    public void TryUpgradeSlot()
    {
        if (AugmentsScript.SlotUpgradeLevel < 5)
        {
            if (PlayerHP.MetalScrap >= pricetiers[AugmentsScript.SlotUpgradeLevel].metalprice * 0.75f && PlayerHP.CorePieces >= pricetiers[AugmentsScript.SlotUpgradeLevel].coreprice * 0.75f && PlayerHP.ElectronicComponents >= pricetiers[AugmentsScript.SlotUpgradeLevel].electronicprice * 1.5f)
            {
                PlayerHP.MetalScrap -= (int)(pricetiers[AugmentsScript.SlotUpgradeLevel].metalprice * 0.75f);
                PlayerHP.CorePieces -= (int)(pricetiers[AugmentsScript.SlotUpgradeLevel].coreprice * 0.75f);
                PlayerHP.ElectronicComponents -= (int)(pricetiers[AugmentsScript.SlotUpgradeLevel].electronicprice*1.5f);
                AugmentsScript.SlotUpgradeLevel++;
            }
            else
            {
                FindAnyObjectByType<ShopScript>().PlayNotEnoughCash();
            }
        }
    }

}
