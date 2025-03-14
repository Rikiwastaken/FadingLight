using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{

    PlayerControls controls;

    public Transform pages;
    public int currentpageid;
    private int maxid;
    void Start()
    {
        maxid = pages.childCount - 1;
        SetPage(0);
    }

    public void SetPage(int pageid)
    {
        currentpageid=pageid;
        for (int i = 0; i < maxid; i++)
        {
            pages.GetChild(i).gameObject.SetActive(false);
        }
        pages.GetChild(currentpageid).gameObject.SetActive(true);
    }

    void OnDodge()
    {
        if (currentpageid==0||currentpageid>=3)
        {
            FindAnyObjectByType<Global>().CloseInventory();
        }
    }

    void OnRightShoulder()
    {
        pages.GetChild(currentpageid).gameObject.SetActive(false);
        if(currentpageid<maxid)
        {
            currentpageid++;
        }
        else
        {
            currentpageid = 0;
        }
        pages.GetChild(currentpageid).gameObject.SetActive(true);
    }

    void OnLeftShoulder()
    {
        pages.GetChild(currentpageid).gameObject.SetActive(false);
        if (currentpageid > 0)
        {
            currentpageid--;
        }
        else
        {
            currentpageid = maxid;
        }
        pages.GetChild(currentpageid).gameObject.SetActive(true);
    }
}
