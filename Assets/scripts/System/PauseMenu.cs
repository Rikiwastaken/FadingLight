using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }
    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}
