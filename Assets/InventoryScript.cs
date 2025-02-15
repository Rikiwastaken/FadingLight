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
        currentpageid = 0;
        for(int i = 0; i < maxid; i++)
        {
            pages.GetChild(i).gameObject.SetActive(false);
        }
        pages.GetChild(currentpageid).gameObject.SetActive(true);
    }

    void OnRightShoulder()
    {
        Debug.Log("right");
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
        Debug.Log("left");
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
