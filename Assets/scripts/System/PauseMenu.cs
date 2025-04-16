using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        FindAnyObjectByType<Global>().inpause = true;
    }
    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
        if(FindAnyObjectByType<PlayerJumpV3>())
        {
            FindAnyObjectByType<PlayerJumpV3>().pressedjump = false;
        }
        if (FindAnyObjectByType<Global>())
        {
            FindAnyObjectByType<Global>().inpause = false;
        }
        
    }
    private void OnDodge()
    {
        Destroy(gameObject);
    }
}

