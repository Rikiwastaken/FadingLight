using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class DisplayInputImage : MonoBehaviour
{
    public Sprite AButton;
    public Sprite BButton;
    public Sprite XButton;
    public Sprite YButton;
    public Sprite Arrow;

    public Sprite KeyBoard;

    private bool displaykeyboard;

    public int buttontodisplay; // 1 A, 2 B, 3 X, 4 Y, 5 Down, 6 Up, 7 Left, 8 Right

    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro TMP = transform.GetChild(0).GetComponent<TextMeshPro>();
        TMP.text = "";
        SR = GetComponent<SpriteRenderer>();
        if(displaykeyboard)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            switch (buttontodisplay)
            {
                case 1:
                    TMP.text = "spc";
                    SR.sprite = KeyBoard;
                    break;
                case 2:
                    TMP.text = "Z";
                    SR.sprite = KeyBoard;
                    break;
                case 3:
                    TMP.text = "A";
                    SR.sprite = KeyBoard;
                    break;
                case 4:
                    TMP.text = "Q";
                    SR.sprite = KeyBoard;
                    break;
                case 5:
                    SR.sprite = Arrow;
                    break;
                case 6:
                    SR.sprite = Arrow;
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case 7:
                    SR.sprite = Arrow;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case 8:
                    SR.sprite = Arrow;
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;

            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            switch (buttontodisplay)
            {
                case 1:
                    SR.sprite = AButton;
                    break;
                case 2:
                    SR.sprite = BButton;
                    break;
                case 3:
                    SR.sprite = XButton;
                    break;
                case 4:
                    SR.sprite = YButton;
                    break;
                case 5:
                    SR.sprite = Arrow;
                    break;
                case 6:
                    SR.sprite = Arrow;
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case 7:
                    SR.sprite = Arrow;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case 8:
                    SR.sprite = Arrow;
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;

            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
