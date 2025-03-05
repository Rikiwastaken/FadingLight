using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class playgame : MonoBehaviour
{
    PlayerControls controls;

    public Transform LoadMenu;

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

