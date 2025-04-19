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
    public Button absorbbutton;

    public List<TextMeshProUGUI> ChainPriceText;
    public List<TextMeshProUGUI> PlatePriceText;
    public List<TextMeshProUGUI> AbsorbPriceText;

    private prices ChainPrice;
    private prices PlatePrice;
    private prices AbsorbPrice;

    public List<prices> pricetiers;

    public GameObject MainShopMenu;

    private EquipmentScript EquipmentScript;
    private PlayerHP PlayerHP;

    private void Start()
    {
        EquipmentScript = FindAnyObjectByType<EquipmentScript>();
        PlayerHP = FindAnyObjectByType<PlayerHP>();
    }

    public void OnDodge()
    {
        MainShopMenu.SetActive(true);
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
        else if (selectedbutton == absorbbutton)
        {
            descriptionText.text = "Improves Eldon's prosthetic's energy draining capability and increases the absorption ratio by 10 %. Can be improved up to level V.";
        }
        else
        {
            descriptionText.text = "";
        }

        ChainPrice = pricetiers[EquipmentScript.ChainUpgradeLevel];
        PlatePrice = pricetiers[EquipmentScript.PlateUpgradeLevel];
        AbsorbPrice = pricetiers[EquipmentScript.AbsorbUpgradeLevel];

        ChainPriceText[0].text = " : " + (int)(ChainPrice.metalprice*0.75f);
        ChainPriceText[1].text = " : " + (int)(ChainPrice.coreprice * 0.75f);
        ChainPriceText[2].text = " : " + (int)(ChainPrice.electronicprice*1.5f);

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

        AbsorbPriceText[0].text = " : " + (int)(AbsorbPrice.metalprice * 0.75f);
        AbsorbPriceText[1].text = " : " + (int)(AbsorbPrice.coreprice * 1.5f);
        AbsorbPriceText[2].text = " : " + (int)(AbsorbPrice.electronicprice * 0.75f);



        switch (EquipmentScript.AbsorbUpgradeLevel)
        {
            case 0:
                AbsorbPriceText[3].text = "Absorption lvl I";
                break;
            case 1:
                AbsorbPriceText[3].text = "Absorption lvl II";
                break;
            case 2:
                AbsorbPriceText[3].text = "Absorption lvl III";
                break;
            case 3:
                AbsorbPriceText[3].text = "Absorption lvl IV";
                break;
            case 4:
                AbsorbPriceText[3].text = "Absorption lvl V";
                break;
            default:
                AbsorbPriceText[3].text = "Absorption at Max Level";
                break;
        }

    }

    public void TryUpgradeChain()
    {
        if(EquipmentScript.ChainUpgradeLevel<5)
        {
            if (PlayerHP.MetalScrap >= pricetiers[EquipmentScript.ChainUpgradeLevel].metalprice * 0.75f && PlayerHP.CorePieces >= pricetiers[EquipmentScript.ChainUpgradeLevel].coreprice * 0.75f && PlayerHP.ElectronicComponents >= pricetiers[EquipmentScript.ChainUpgradeLevel].electronicprice * 1.5f)
            {
                PlayerHP.MetalScrap -= (int)(pricetiers[EquipmentScript.ChainUpgradeLevel].metalprice * 0.75f);
                PlayerHP.CorePieces -= (int)(pricetiers[EquipmentScript.ChainUpgradeLevel].coreprice * 0.75f);
                PlayerHP.ElectronicComponents -= (int)(pricetiers[EquipmentScript.ChainUpgradeLevel].electronicprice * 1.5f);
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

    public void TryUpgradeAbsorb()
    {
        if (EquipmentScript.AbsorbUpgradeLevel < 5)
        {
            if (PlayerHP.MetalScrap >= pricetiers[EquipmentScript.AbsorbUpgradeLevel].metalprice * 0.75f && PlayerHP.CorePieces >= pricetiers[EquipmentScript.AbsorbUpgradeLevel].coreprice * 1.5f && PlayerHP.ElectronicComponents >= pricetiers[EquipmentScript.AbsorbUpgradeLevel].electronicprice * 0.75f)
            {
                PlayerHP.MetalScrap -= (int)(pricetiers[EquipmentScript.AbsorbUpgradeLevel].metalprice * 0.75f);
                PlayerHP.CorePieces -= (int)(pricetiers[EquipmentScript.AbsorbUpgradeLevel].coreprice * 1.5f);
                PlayerHP.ElectronicComponents -= (int)(pricetiers[EquipmentScript.AbsorbUpgradeLevel].electronicprice*0.75f);
                EquipmentScript.AbsorbUpgradeLevel++;
            }
            else
            {
                FindAnyObjectByType<ShopScript>().PlayNotEnoughCash();
            }
        }
    }

}
