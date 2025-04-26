using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWorldShopScript : MonoBehaviour
{

    public GameObject Arrow;
    public float rotperframe;
    private Global global;
    private GameObject Mattshop;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerHP>() != null)
        {
            Arrow.SetActive(true);
            Arrow.transform.rotation =  Quaternion.Euler(Arrow.transform.rotation.eulerAngles + new Vector3(0, rotperframe, 0));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHP>() != null)
        {
            Arrow.SetActive(false);
        }
    }

    private void Start()
    {
        global = FindAnyObjectByType<Global>();

    }

    private void OnDown()
    {
        if(Mattshop==null)
        {
            Mattshop = global.MattShopMenu;
        }
        if(Arrow.activeSelf && !Mattshop.activeSelf)
        {
            Mattshop.SetActive(true);
        }
    }
}
