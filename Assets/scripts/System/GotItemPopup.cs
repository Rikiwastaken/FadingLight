using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GotItemPopup : MonoBehaviour
{
    public Image ItemImage;
    public TextMeshProUGUI ItemTypeTMP;
    public TextMeshProUGUI ItemNameTMP;

    public Vector2 Finalpos;
    public Vector2 Startpos;
    public float timetomove;
    public float timetodie;

    private int movecounter;
    private int diecounter;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(diecounter == 0 )
        {
            Destroy(gameObject);
        }
        else
        {
            diecounter--;
            if( diecounter < 1/Time.deltaTime )
            {
                
                float alpha =  Time.deltaTime* diecounter;
                ItemNameTMP.color = new Color( 1f, 1f, 1f, alpha );
                ItemTypeTMP.color = new Color(1f, 1f, 1f, alpha);
                ItemImage.color = new Color(1f, 1f, 1f, alpha);
                GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
            }
        }

        if( movecounter != 0 )
        {
            transform.localPosition = Vector2.Lerp( Finalpos, Startpos, movecounter/(timetomove/Time.deltaTime) );
            movecounter--;
        }
    }

    public void InitiatePopup(Sprite ItemSprite, string ItemType, string ItemName)
    {
        ItemImage.sprite = ItemSprite;
        ItemTypeTMP.text=ItemType;
        ItemNameTMP.text = ItemName;
        movecounter = (int)(timetomove/Time.deltaTime);
        diecounter = (int)(timetodie / Time.deltaTime);
        transform.localPosition = Startpos;
    }
}
