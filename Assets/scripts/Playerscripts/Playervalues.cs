using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Playervalues : MonoBehaviour
{
    public static float EldonHP;
    public static float EldonmaxHP;
    public static float EldonNRG;
    public static float EldonmaxNRG;
    private float xcoord;
    private float ycoord;
    private int scenenbr;

    // Update is called once per frame
    void Awake()
    {
        EldonmaxHP = 100;
        EldonHP = 100;
        EldonmaxNRG = 100;
        EldonNRG = 100;
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        Savepos();
    }

    void Savepos()
    {
        string path = Application.persistentDataPath + "/save.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Save File \n");
        }

       // string content = "xcoord: " + xcoord + "\n";
        //File.AppendAllText(path, content);

    }
}
