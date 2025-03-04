using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogueManager;

public class CutsceneDialogueWindow : MonoBehaviour
{

    

    private Image characterfaceImage;
    private TextMeshProUGUI TMPText;
    private TextMeshProUGUI TMPName;
    private string FullText;
    public float characterpersecond;
    private int charactercounter;
    public int numberofcharacterstoshow;
    public Vector2 WindowPosition;


    private void Update()
    {
        if (FullText != null)
        {
            if (numberofcharacterstoshow < FullText.Length)
            {
                charactercounter++;
                if (charactercounter > 1 / (Time.deltaTime* characterpersecond))
                {
                    charactercounter = 0;
                    numberofcharacterstoshow++;
                }
            }
            string text = "";
            for(int i = 0; i < numberofcharacterstoshow; i++)
            {
                text += FullText[i];
            }
            TMPText.text = text;
        }
    }

    void Initialization()
    {
        characterfaceImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        TMPName = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TMPText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        transform.localPosition = WindowPosition;
    }

    public void InitiateDialogue(DialoguePart diag)
    {
        Initialization();
        FullText = diag.text;
        characterfaceImage.sprite = diag.Face;
        TMPName.text= diag.name;
        charactercounter = 0;
        numberofcharacterstoshow = 0;

    }

}
