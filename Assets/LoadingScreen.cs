using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public Sprite Image;

    private Transform LeftPart;

    private Transform RightPart;

    private string scenetoload;

    public float movespeed;

    private bool unload;

    private int startframes=5;

    // Start is called before the first frame update
    void Start()
    {
        LeftPart = transform.GetChild(0);
        RightPart = transform.GetChild(1);
        unload = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(scenetoload != null)
        {
            LoadingLoadingScreen();
        }
        if(unload && startframes == 0)
        {
            UnLoadingLoadingScreen();
        }
        else if (startframes > 0)
        {
            startframes--;
        }
    }


    public void LoadSceneWithScreen(string scenename)
    {
        scenetoload = scenename;
    }

    void LoadingLoadingScreen()
    {
        if (transform.GetChild(0).localScale.x == 1)
        {
            SceneManager.LoadScene(scenetoload);
        }
        if (transform.GetChild(0).localScale.x <= 1)
        {
            foreach (Transform Child in transform)
            {
                Child.localScale += new Vector3(movespeed, 0f, 0f);
            }
        }
        else
        {
            foreach (Transform Child in transform)
            {
                Child.localScale = Vector3.one;
            }
        }
    }

    void UnLoadingLoadingScreen()
    {
        if (transform.GetChild(0).localScale.x == 0)
        {
            unload = false;
        }
        if (transform.GetChild(0).localScale.x >0)
        {
            foreach (Transform Child in transform)
            {
                Child.localScale -= new Vector3(movespeed, 0f, 0f);
            }
        }
        else
        {
            foreach (Transform Child in transform)
            {
                Child.localScale = new Vector3(0,1,1);
            }
        }
        
    }

}
