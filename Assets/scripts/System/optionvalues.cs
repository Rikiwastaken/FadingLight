using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveManager;
using static soundoptions;

public class optionvalues : MonoBehaviour
{

    [System.Serializable]
    public class OptionSave
    {
        public float musicvol; //volume of music
        public float soundvol; //volume of music
        public soundoptions.Resolution resolution;
        public bool fullscreen;
    }

    public OptionSave options;

    void Awake()
    {
        DontDestroyOnLoad(this);
        LoadOrCreateOptionsSave();
        Screen.SetResolution(options.resolution.width, options.resolution.height, Screen.fullScreenMode);
        Screen.fullScreen=options.fullscreen;
    }


    private void LoadOrCreateOptionsSave()
    {
        if(System.IO.File.Exists(Application.persistentDataPath + "/Options.txt"))
        {
            string optionsfile = System.IO.File.ReadAllText(Application.persistentDataPath + "/Options.txt");
            options = JsonUtility.FromJson<OptionSave>(optionsfile);
        }
        else
        {
            options = new OptionSave();
            options.musicvol = 0.5f;
            options.soundvol = 0.5f;
            options.resolution = new soundoptions.Resolution();
            options.resolution.width = Screen.currentResolution.width;
            options.resolution.height = Screen.currentResolution.height;
            options.fullscreen = Screen.fullScreen;
            string optionJSON = JsonUtility.ToJson(options);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/Options.txt", optionJSON);
        }
    }
    public void SaveOptions()
    {
        if(options != null)
        {
            string optionJSON = JsonUtility.ToJson(options);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/Options.txt", optionJSON);
        }
    }
}
