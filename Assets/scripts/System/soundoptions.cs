using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class soundoptions : MonoBehaviour
{
    [System.Serializable]
    public class Resolution
    {
        public int width;
        public int height;
    }

    public float soundvolume;
    public float musicvolume;

    public Image SoundImage;
    public Image MusicImage;

    private int valuedown;
    private int valueup;
    private int valueleft;
    private int valueright;
    private int valueclick;
    private Vector2 lastinput;

    private bool pressedclick;
    public Transform selected;
    int activebuttonID;
    private Resolution resolution;
    public List<Resolution> Allresolutions;
    public TextMeshProUGUI resolutiontext;
    public TextMeshProUGUI FullscreenText;

    PlayerControls controls;

    // Start is called before the first frame update
    void OnEnable()
    {
        controls = new PlayerControls();



        controls.gameplay.crossdown.performed += ctx => valuedown = 1;
        controls.gameplay.crossdown.canceled += ctx => valuedown = 0;
        controls.gameplay.crossright.performed += ctx => valueright = 1;
        controls.gameplay.crossright.canceled += ctx => valueright = 0;
        controls.gameplay.crossleft.performed += ctx => valueleft = 1;
        controls.gameplay.crossleft.canceled += ctx => valueleft = 0;
        controls.gameplay.crossup.performed += ctx => valueup = 1;
        controls.gameplay.crossup.canceled += ctx => valueup = 0;
        controls.gameplay.jump.performed += ctx => valueclick = 1;
        controls.gameplay.jump.canceled += ctx => valueclick = 0;

        controls.gameplay.Enable();

        resolution=new Resolution();
        resolution.width = Screen.currentResolution.width;
        resolution.height = Screen.currentResolution.height;
        resolutiontext.text = resolution.width + "x" + resolution.height;
        soundvolume = FindAnyObjectByType<optionvalues>().soundvol;
        musicvolume = FindAnyObjectByType<optionvalues>().musicvol;
        if (Screen.fullScreen)
        {
            FullscreenText.text = "Fullscreen : On";
        }
        else
        {
            FullscreenText.text = "Fullscreen : Off";
        }
    }

    private void Awake()
    {
        SoundImage.fillAmount = soundvolume;
        MusicImage.fillAmount = musicvolume;
    }

    // Update is called once per frame
    void Update()
    {

        SoundImage.fillAmount = soundvolume;
        MusicImage.fillAmount = musicvolume;

        if (valueclick == 0)
        {
            pressedclick = false;
        }



        Vector2 input = Vector2.zero;
        if (valueup != 0 || valuedown != 0 || valueleft != 0 || valueright != 0)
        {
            if (valuedown != 0)
            {
                input.y = -1;
            }
            else if (valueup != 0)
            {
                input.y = 1;
            }
            if (valueleft != 0)
            {
                input.x = -1;
            }
            else if (valueright != 0)
            {
                input.x = 1;
            }
        }

        if (lastinput != input && input != Vector2.zero)
        {
            Direction((int)input.y);
        }
        

        for(int i = 0;i<transform.childCount;i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = new Color(1f,1f,1f,0f);
        }
        if (selected != null)
        {
            selected.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            if ((!pressedclick && valueclick == 1) || (input.x!=0 && input !=lastinput))
            {
                pressedclick = true;
                Selection(input.x, valueclick);
            }

        }

        lastinput = input;

    }

    public void Selection(float inputx, int valueclick)
    {
        switch(selected.name)
        {
            case "SoundSelected":
                if(inputx != 0)
                {
                    soundvolume += 0.05f * inputx;
                    if(soundvolume<0)
                    {
                        soundvolume = 0;
                    }
                    else if(soundvolume>1)
                    {
                        soundvolume=1;
                    }
                }
                
                break;
            case "MusicSelected":
                if (inputx != 0)
                {
                    musicvolume += 0.05f * inputx;
                    if (musicvolume < 0)
                    {
                        musicvolume = 0;
                    }
                    else if (musicvolume > 1)
                    {
                        musicvolume = 1;
                    }
                }
                break;
            case "ResolutionSelected":
                if (inputx < 0)
                {
                    resolution=GetClosestLower(resolution);
                }
                else if(inputx > 0)
                {
                    resolution = GetClosestHigher(resolution);
                }
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
                resolutiontext.text = resolution.width + "x" + resolution.height;
                break;
            case "FullScreenSelected":
                if (Screen.fullScreen==true)
                {
                    Screen.fullScreen = false;
                    FullscreenText.text = "Fullscreen : Off";
                }
                else
                {
                    Screen.fullScreen = true;
                    FullscreenText.text = "Fullscreen : On";
                }
                break;
            case "BackSelected":
                if (valueclick != 0)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                break;



        }
    }


    Resolution GetClosestLower(Resolution res)
    {
        Resolution result = new Resolution();
        result.width = 0;
        result.height = 0;
        int mindist = 1000000;
        for(int i = 0; i < Allresolutions.Count; i++)
        {
            if (Allresolutions[i].width < res.width && Mathf.Abs(Allresolutions[i].width - res.width)<mindist)
            {
                result = Allresolutions[i];
                mindist = Mathf.Abs(Allresolutions[i].width - res.width);
            }
        }
        if (result.width == 0)
        {
            return res;
        }
        return result;
    }


    Resolution GetClosestHigher(Resolution res)
    {
        Resolution result = new Resolution();
        result.width = 0;
        result.height = 0;
        int mindist = 1000000;
        for (int i = 0; i < Allresolutions.Count; i++)
        {
            if (Allresolutions[i].width > res.width && Mathf.Abs(Allresolutions[i].width - res.width) < mindist)
            {
                result = Allresolutions[i];
                mindist = Mathf.Abs(Allresolutions[i].width - res.width);
            }
        }
        if(result.width == 0)
        {
            return res;
        }
        return result;
    }


    public void Direction(int dirinput)
    {
        if (selected == null)
        {
            activebuttonID = 0;
            selected = transform.GetChild(0);
            return;
        }
        if (dirinput > 0)
        {
            if (activebuttonID > 0)
            {
                activebuttonID--;
            }
            else
            {
                activebuttonID = transform.childCount - 1;

            }
            selected = transform.GetChild(activebuttonID);
            return;
        }
        if (dirinput < 0)
        {
            if (activebuttonID < transform.childCount - 1)
            {
                activebuttonID++;
            }
            else
            {
                activebuttonID = 0;

            }
            selected = transform.GetChild(activebuttonID);
            return;
        }
    }

    
}
