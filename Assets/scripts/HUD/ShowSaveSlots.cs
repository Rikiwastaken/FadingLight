using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowSaveSlots : MonoBehaviour
{
    private void OnDodge()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void OnEnable()
    {
        getallsaves();
    }

    void getallsaves()
    {
        for(int i = 0; i < transform.childCount-1; i++)
        {
            transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Slot" + i;
            string name =FindAnyObjectByType<SaveManager>().GetLastSave(i)[1];
            if(name != null)
            {
                transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text= convertToHours(int.Parse(name.Split('_')[1]+""));
                if (FindAnyObjectByType<Global>().clickednewgame)
                {
                    transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
                }
                else
                {
                    transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
                }
                
            }
            else
            {
                transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "EmptyFile";
                if (FindAnyObjectByType<Global>().clickednewgame)
                {
                    transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
                }
                else
                {
                    transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    string convertToHours(int seconds)
    {
        string res = "";
        int minutes = seconds / 60;
        int hours = minutes / 60;
        res += hours+"h";
        int remainingminutes = minutes % 60;
        if(remainingminutes <=9)
        {
            res+="0";
        }
        res += remainingminutes;
        return res;
    }


}
