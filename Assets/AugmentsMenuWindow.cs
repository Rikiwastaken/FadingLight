using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AugmentsScript;

public class AugmentsMenuWindow : MonoBehaviour
{

    public Transform EquipedAugmentsContainer;
    public Transform DisplayAugmentsContainer;
    private List<Augment> Augmentlist;
    private List<bool> EquipedAugments;

    private AugmentsScript augmentscript;

    // Start is called before the first frame update
    void Start()
    {
        augmentscript = FindAnyObjectByType<AugmentsScript>();

        Augmentlist = augmentscript.Augmentlist;

    }

    // Update is called once per frame
    void Update()
    {
        int usedslot = 0;
        EquipedAugments = augmentscript.EquipedAugments;

        for(int i=0; i<14; ++i)
        {
            EquipedAugmentsContainer.GetChild(i).GetChild(0).GetComponent<Image>().color=new Color(1f,1f,1f,0f);
        }

        for (int i = 0; i < EquipedAugments.Count; i++)
        {
            if (EquipedAugments[i])
            {
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                EquipedAugmentsContainer.GetChild(usedslot).GetChild(0).GetComponent<Image>().sprite = Augmentlist[i].image;
                usedslot++;
            }
        }
    }


}
