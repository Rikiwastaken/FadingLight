using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionvalues : MonoBehaviour
{
    public float musicvol;
    public float soundvol;


    void Awake()
    {
        DontDestroyOnLoad(this);
        musicvol = 0.5f;
        soundvol = 0.5f;
    }

    void Update()
    {
        if (GameObject.Find("Options") != null && GameObject.Find("Options").activeSelf==true)
        {
            musicvol = GameObject.Find("Options").GetComponentInChildren<soundoptions>().musicvolume;
            musicvol = GameObject.Find("Options").GetComponentInChildren<soundoptions>().soundvolume;
        }
    }
}
