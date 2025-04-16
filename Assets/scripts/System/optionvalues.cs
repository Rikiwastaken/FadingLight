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
        public bool headphones;
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
        string path = Application.persistentDataPath + "/Options.txt";

        if (System.IO.File.Exists(path))
        {
            try
            {
                string optionsfile = System.IO.File.ReadAllText(path);
                options = JsonUtility.FromJson<OptionSave>(optionsfile);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Erreur lors du chargement des options, création de nouvelles options par défaut. Détail : " + e.Message);
                CreateDefaultOptions(path);
            }
        }
        else
        {
            CreateDefaultOptions(path);
        }
    }

    private void CreateDefaultOptions(string path)
    {
        options = new OptionSave();
        options.musicvol = 0.5f;
        options.soundvol = 0.5f;
        options.resolution = new soundoptions.Resolution();
        options.resolution.width = Screen.currentResolution.width;
        options.resolution.height = Screen.currentResolution.height;
        options.fullscreen = Screen.fullScreen;
        options.headphones = false;

        string optionJSON = JsonUtility.ToJson(options);
        System.IO.File.WriteAllText(path, optionJSON);
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
