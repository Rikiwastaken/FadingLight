using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class playgame : MonoBehaviour
{
    PlayerControls controls;

    public void Playgame()
    {
        FindAnyObjectByType<Global>().clickednewgame=true;
    }
    public void Quitgame()
    {
        Debug.Log("Ciao");
        Application.Quit();
    }

    public void continuegame()
    {
        FindAnyObjectByType<Global>().clickednewgame = false;
    }

    

}

