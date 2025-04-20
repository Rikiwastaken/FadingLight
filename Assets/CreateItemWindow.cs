using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AugmentsScript;
using static GadgetScript;

public class CreateItemWindow : MonoBehaviour
{

    [System.Serializable]
    public class CreationItem
    {
        public Augment augment;
        public Gadget gadget;
        public int metalprice;
        public int coreprice;
        public int electronicprice;
    }

    public bool createaugments;

    public List<CreationItem> CreatableList;

    private AugmentsScript AugmentsScript;
    private GadgetScript GadgetScript;

    public Transform CreationItemContainer;

    public Button selected;

    private int IndexOfTopCratable;

    public TextMeshProUGUI descriptionwindow;
    private bool eventalreadycalled;

    private PlayerHP playerhp;

    // Start is called before the first frame update
    private void Start()
    {
        playerhp = FindAnyObjectByType<PlayerHP>();
        SetupList();
    }

    private void Update()
    {
        eventalreadycalled = false;
        if (selected != null)
        {
            selected.Select();
            if (createaugments)
            {
                descriptionwindow.text = selected.transform.parent.GetComponent<CreationItemPrefab>().item.augment.description;
            }
            else
            {
                descriptionwindow.text = selected.transform.parent.GetComponent<CreationItemPrefab>().item.gadget.description;
            }
        }
        else if (CreatableList.Count == 0)
        {
            descriptionwindow.text = "Nothing to create.";
        }
        else
        {
            descriptionwindow.text = "";
        }
    }

    private void OnDown()
    {
        if(eventalreadycalled)
        {
            return;
        }
        else
        {
            eventalreadycalled=true;
        }
        DownInput();
    }

    private void OnUp()
    {
        if (eventalreadycalled)
        {
            return;
        }
        else
        {
            eventalreadycalled = true;
        }
        UpInput();
    }

    private void OnCrossdown()
    {
        if (eventalreadycalled)
        {
            return;
        }
        else
        {
            eventalreadycalled = true;
        }
        DownInput();
    }

    private void OnCrossup()
    {
        if (eventalreadycalled)
        {
            return;
        }
        else
        {
            eventalreadycalled = true;
        }
        UpInput();
    }

    private void DownInput()
    {
        if(selected == null && CreatableList.Count>0)
        {
            selected = CreationItemContainer.GetChild(0).GetChild(0).GetComponent<Button>();
        }
        else
        {
            int rankofselected = GetRankOfSelected();
            if(rankofselected <2 && CreatableList.Count>rankofselected+1+IndexOfTopCratable)
            {
                selected = CreationItemContainer.GetChild(rankofselected+1).GetChild(0).GetComponent<Button>();
            }
            else if(rankofselected ==2 && CreatableList.Count > rankofselected + 1 + IndexOfTopCratable)
            {
                IndexOfTopCratable += 1;
                UpdateShown();
            }
            else
            {
                IndexOfTopCratable =0;
                selected = CreationItemContainer.GetChild(0).GetChild(0).GetComponent<Button>();
                UpdateShown();
            }

        }
    }

    private void UpInput()
    {
        if (selected == null && CreatableList.Count > 0)
        {
            selected = CreationItemContainer.GetChild(0).GetChild(0).GetComponent<Button>();
        }
        else
        {
            int rankofselected = GetRankOfSelected();
            if (rankofselected > 0 && CreatableList.Count > rankofselected - 1 + IndexOfTopCratable)
            {
                selected = CreationItemContainer.GetChild(rankofselected - 1).GetChild(0).GetComponent<Button>();
            }
            else if (rankofselected == 0 && IndexOfTopCratable>0)
            {
                IndexOfTopCratable -= 1;
                UpdateShown();
            }
            else
            {
                if(CreatableList.Count >= 3)
                {
                    IndexOfTopCratable = CreatableList.Count - 3;
                    selected = CreationItemContainer.GetChild(2).GetChild(0).GetComponent<Button>();
                }
                else if (CreatableList.Count == 2)
                {
                    IndexOfTopCratable = 0;
                    selected = CreationItemContainer.GetChild(1).GetChild(0).GetComponent<Button>();
                }
                else
                {
                    selected = CreationItemContainer.GetChild(0).GetChild(0).GetComponent<Button>();
                }
                
                UpdateShown();
            }

        }
    }

    private void OnJump()
    {
        if(selected!=null)
        {
            TryToCreate();
            
        }
    }

    public void SetupList()
    {
        if(createaugments)
        {
            AugmentsScript = FindAnyObjectByType<AugmentsScript>();

            CreatableList = new List<CreationItem>();

            foreach(Augment Augment in AugmentsScript.Augmentlist)
            {
                if(Augment.craftable && Augment.locked)
                {
                    CreationItem creationItem = new CreationItem();
                    creationItem.augment = Augment;
                    creationItem.metalprice = Augment.craftingmaterials.metalprice;
                    creationItem.coreprice = Augment.craftingmaterials.coreprice;
                    creationItem.electronicprice = Augment.craftingmaterials.electronicprice;
                    CreatableList.Add(creationItem);
                }
            }
        }
        else
        {
            GadgetScript = FindAnyObjectByType<GadgetScript>();

            CreatableList = new List<CreationItem>();

            foreach (Gadget Gadget in GadgetScript.GadgetList)
            {
                if (Gadget.craftable && Gadget.locked)
                {
                    CreationItem creationItem = new CreationItem();
                    creationItem.gadget = Gadget;
                    creationItem.metalprice = Gadget.craftingmaterials.metalprice;
                    creationItem.coreprice = Gadget.craftingmaterials.coreprice;
                    creationItem.electronicprice = Gadget.craftingmaterials.electronicprice;
                    CreatableList.Add(creationItem);
                }
            }
        }
        UpdateShown();
    }

    private void UpdateShown()
    {
        if(CreatableList.Count==0)
        {
            for(int i = 0; i<CreationItemContainer.childCount;i++)
            {
                CreationItemContainer.GetChild(i).gameObject.SetActive(false);
            }
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }
        else
        {
            if(CreatableList.Count<3)
            {
                GameObject myEventSystem = GameObject.Find("EventSystem");
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            }

            for (int i = 0; i < CreationItemContainer.childCount; i++)
            {
                CreationItemContainer.GetChild(i).gameObject.SetActive(false);
            }
            CreationItemContainer.GetChild(0).gameObject.SetActive(true);
            CreationItemContainer.GetChild(0).GetComponent<CreationItemPrefab>().InitialSetup(CreatableList[IndexOfTopCratable]);
            if (CreatableList.Count > IndexOfTopCratable + 1)
            {
                CreationItemContainer.GetChild(1).gameObject.SetActive(true);
                CreationItemContainer.GetChild(1).GetComponent<CreationItemPrefab>().InitialSetup(CreatableList[IndexOfTopCratable + 1]);
            }
            if (CreatableList.Count > IndexOfTopCratable + 2)
            {
                CreationItemContainer.GetChild(2).gameObject.SetActive(true);
                CreationItemContainer.GetChild(2).GetComponent<CreationItemPrefab>().InitialSetup(CreatableList[IndexOfTopCratable + 2]);
            }
        }

        
    }

    private int GetRankOfSelected()
    {
        int rank = 0;

        if(selected!=null)
        {
            if (selected == CreationItemContainer.GetChild(0).GetChild(0).GetComponent<Button>())
            {
                rank = 0;
            }
            if (selected == CreationItemContainer.GetChild(1).GetChild(0).GetComponent<Button>())
            {
                rank = 1;
            }
            else if (selected == CreationItemContainer.GetChild(2).GetChild(0).GetComponent<Button>())
            {
                rank = 2;
            }
        }

        return rank;
    }

    public void TryToCreate()
    {
        if(selected != null)
        {
            CreationItem tocreate = selected.GetComponentInParent<CreationItemPrefab>().item;
            if(tocreate.augment!=null)
            {
                if (playerhp.MetalScrap >= tocreate.metalprice && playerhp.CorePieces >= tocreate.coreprice && playerhp.ElectronicComponents >= tocreate.electronicprice && tocreate.augment.locked)
                {
                    playerhp.MetalScrap -= tocreate.metalprice;
                    playerhp.CorePieces -= tocreate.coreprice;
                    playerhp.ElectronicComponents -= tocreate.electronicprice;
                    AugmentsScript.Augmentlist[tocreate.augment.ID].locked = false;
                    SetupList();
                    GameObject myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    selected = null;
                }
                else
                {
                    FindAnyObjectByType<ShopScript>().PlayNotEnoughCash();
                }

            }
            else if (tocreate.gadget != null)
            {
                if (playerhp.MetalScrap >= tocreate.metalprice && playerhp.CorePieces >= tocreate.coreprice && playerhp.ElectronicComponents >= tocreate.electronicprice && tocreate.gadget.locked)
                {
                    playerhp.MetalScrap -= tocreate.metalprice;
                    playerhp.CorePieces -= tocreate.coreprice;
                    playerhp.ElectronicComponents -= tocreate.electronicprice;
                    GadgetScript.GadgetList[tocreate.gadget.ID].locked = false;
                    SetupList();
                    GameObject myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    selected = null;
                }
                else
                {
                    FindAnyObjectByType<ShopScript>().PlayNotEnoughCash();
                }
            }
            

        }
    }

}
