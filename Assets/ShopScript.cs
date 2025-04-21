using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [System.Serializable]
    public class ShopNPC
    {
        public string name;
        public string EnterSentence;
        public string UpgradeSentence;
        public string BuyingSentence;
        public string NotEnoughMaterialsSentence;
        public string talksentence;
        public string AfterTransactionSentence;
        public Sprite[] spritelist;
    }

    public ShopNPC npc;

    public TextMeshProUGUI npcnametext;
    public TextMeshProUGUI npcquotetext;

    public TextMeshProUGUI metaltext;
    public TextMeshProUGUI coretext;
    public TextMeshProUGUI electext;

    public TextMeshProUGUI DescriptionText;

    public GameObject MainShopScreen;
    public GameObject CraftScreen;
    public GameObject UpgradeScreen;

    private Global global;
    private PlayerHP PlayerHP;

    private string FullText;
    private int charactercounter;
    private int numberofcharacterstoshow;
    public float characterpersecond;

    // Start is called before the first frame update
    void Start()
    {
        global = FindAnyObjectByType<Global>();
        PlayerHP = FindAnyObjectByType<PlayerHP>();
    }

    private void OnEnable()
    {
        FullText = npc.EnterSentence;
        charactercounter = 0;
        numberofcharacterstoshow = 0;
    }

    // Update is called once per frame
    void Update()
    {
        npcnametext.text = npc.name;
        if(MainShopScreen.activeSelf)
        {
            if(MainShopScreen.GetComponent<basicmenunav>().selected==null)
            {
                DescriptionText.text = "";
            }
            else
            {
                DescriptionText.text = MainShopScreen.GetComponent<basicmenunav>().selected.name;
            }
        }

        metaltext.text = " : " + PlayerHP.MetalScrap;
        coretext.text = " : " + PlayerHP.CorePieces;
        electext.text = " : " + PlayerHP.ElectronicComponents;
        global.inshop = true;

        if (FullText != null)
        {
            if (numberofcharacterstoshow < FullText.Length)
            {
                charactercounter++;
                if (charactercounter > 1 / (Time.deltaTime * characterpersecond))
                {
                    charactercounter = 0;
                    numberofcharacterstoshow++;
                }
            }
            string text = "";
            for (int i = 0; i < numberofcharacterstoshow; i++)
            {
                text += FullText[i];
            }
            npcquotetext.text = text;
        }
    }

    public void Leave()
    {
        global.inshop = false;
        gameObject.SetActive(false);
    }

    private void OnDodge()
    {
        if (MainShopScreen.activeSelf)
        {
            Leave();
        }
        else if(CraftScreen.activeSelf)
        {
            playAfterTransactionSentence();
            CraftScreen.SetActive(false);
            MainShopScreen.SetActive(true);
            transform.GetChild(0).GetComponent<basicmenunav>().resetselection();
        }
        else if (UpgradeScreen.activeSelf)
        {
            playAfterTransactionSentence();
            UpgradeScreen.SetActive(false);
            MainShopScreen.SetActive(true);
            transform.GetChild(0).GetComponent<basicmenunav>().resetselection();
        }
    }

    private void OnMenu()
    {
        if (MainShopScreen.activeSelf)
        {
            Leave();
        }
        else if (CraftScreen.activeSelf)
        {
            playAfterTransactionSentence();
            CraftScreen.SetActive(false);
            MainShopScreen.SetActive(true);
            transform.GetChild(0).GetComponent<basicmenunav>().resetselection();
        }
        else if (UpgradeScreen.activeSelf)
        {
            playAfterTransactionSentence();
            UpgradeScreen.SetActive(false);
            MainShopScreen.SetActive(true);
            transform.GetChild(0).GetComponent<basicmenunav>().resetselection();
        }
    }

    public void OpenUpgradeScreen()
    {
        playUpgradeSentence();
        UpgradeScreen.SetActive(true);
        MainShopScreen.SetActive(false);
    }

    public void OpenCreateAugmentsScreen()
    {
        PlayBuyingSentence();
        CraftScreen.SetActive(true);
        CraftScreen.GetComponent<CreateItemWindow>().createaugments = true;
        CraftScreen.GetComponent<CreateItemWindow>().SetupList();
        MainShopScreen.SetActive(false);
    }

    public void OpenCreateGadgetsScreen()
    {
        PlayBuyingSentence();
        CraftScreen.SetActive(true);
        CraftScreen.GetComponent<CreateItemWindow>().createaugments = false;
        CraftScreen.GetComponent<CreateItemWindow>().SetupList();
        MainShopScreen.SetActive(false);
    }

    public void playUpgradeSentence()
    {
        FullText = npc.UpgradeSentence;
        charactercounter = 0;
        numberofcharacterstoshow = 0;
    }

    public void PlayBuyingSentence()
    {
        FullText = npc.BuyingSentence;
        charactercounter = 0;
        numberofcharacterstoshow = 0;
    }

    public void Playtalksentence()
    {
        FullText = npc.talksentence;
        charactercounter = 0;
        numberofcharacterstoshow = 0;
    }

    public void playAfterTransactionSentence()
    {
        FullText = npc.AfterTransactionSentence;
        charactercounter = 0;
        numberofcharacterstoshow = 0;
    }

    public void PlayNotEnoughCash()
    {
        FullText = npc.NotEnoughMaterialsSentence;
        charactercounter = 0;
        numberofcharacterstoshow = 0;
    }

}
