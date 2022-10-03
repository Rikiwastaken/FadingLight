using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playervalues : MonoBehaviour
{
    public static float EldonHP;
    public static float EldonmaxHP;
    public static float EldonNRG;
    public static float EldonmaxNRG;

    // Update is called once per frame
    void Awake()
    {
        EldonmaxHP = 100;
        EldonHP = 100;
        EldonmaxNRG = 100;
        EldonNRG = 100;
        DontDestroyOnLoad(this);
    }
}
