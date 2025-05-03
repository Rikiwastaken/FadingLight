using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicmanager : MonoBehaviour
{
    public AudioSource musicexp;
    public AudioSource musiccbt;
    public AudioSource musicboss;
    public AudioSource musicshop;

    public float musicshopbasevolume;

    public AudioSource musicsave;

    public float musicsavebasevolume;

    public AudioSource MainMenuMusic;

    public float MainMenuMusicbasevolume;

    public bool playcbt;
    public float musiccbtvol;
    public float musicexpvol;
    private float musiccbtvolwithoptions;
    private float musicexpvolwithoptions;

    private Global Global;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Global =FindAnyObjectByType<Global>();
        musiccbtvolwithoptions = musiccbtvol * Global.GetComponent<optionvalues>().options.musicvol;
        musicexpvolwithoptions = musicexpvol * Global.GetComponent<optionvalues>().options.musicvol;
    }
    void FixedUpdate()
    {

        if(SceneManager.GetActiveScene().name== "MainMenu")
        {
            MainMenuMusic.volume = MainMenuMusicbasevolume * Global.GetComponent<optionvalues>().options.musicvol;
            if(!MainMenuMusic.isPlaying)
            {
                MainMenuMusic.Play();
            }
            
            return;
        }
        else
        {
            MainMenuMusic.Stop();
            
        }




        musiccbtvolwithoptions = musiccbtvol * Global.GetComponent<optionvalues>().options.musicvol;
        musicexpvolwithoptions = musicexpvol * Global.GetComponent<optionvalues>().options.musicvol;


        if (SceneManager.GetActiveScene().name!="MainMenu" && SceneManager.GetActiveScene().name != "Start" && !musiccbt.isPlaying)
        {
            musiccbt.Play();
            musiccbt.volume = 0;
            musicexp.Play();
        }

        if(!Global.inbossfight)
        {
            if (playcbt)
            {
                if (musiccbt.volume < musiccbtvolwithoptions)
                {
                    musiccbt.volume += 0.01f;
                    
                }
                else
                {
                    musiccbt.volume = musiccbtvolwithoptions;
                }
                if(musicexp.volume > 0f)
                {
                    musicexp.volume -= 0.01f;
                }
                else
                {
                    musicexp.volume = 0f;
                }
            }
            else
            {
                if (musicexp.volume < musicexpvolwithoptions)
                {
                    musicexp.volume += 0.01f;
                }
                else
                {
                    musicexp.volume = musicexpvolwithoptions;
                }

                if (musiccbt.volume > 0f)
                {
                    musiccbt.volume -= 0.01f;
                }
                else
                {
                    musiccbt.volume = 0f;
                }
            }
        }
        if(Global.inshop)
        {
            if(!musicshop.isPlaying)
            {
                musicshop.volume = musicshopbasevolume * Global.GetComponent<optionvalues>().options.musicvol;
                musicshop.Play();
                musicexp.volume = 0;
                musiccbt.volume = 0;
            }
            else
            {
                musicexp.volume = 0;
                musiccbt.volume = 0;
            }
        }
        else
        {
            musicshop.volume *= 0.99f;
        }

        if (Global.atsavepoint)
        {
            if (!musicsave.isPlaying)
            {
                musicsave.volume = musicsavebasevolume * Global.GetComponent<optionvalues>().options.musicvol;
                musicsave.Play();
                musicexp.volume = 0;
                musiccbt.volume = 0;
            }
            else
            {
                musicexp.volume = 0;
                musiccbt.volume = 0;
            }
        }
        else
        {
            musicsave.volume*=0.99f;
        }

    }

    public void EnterBossMusic()
    {
        musicboss.Play();
        musicboss.volume = musiccbtvol;
        musicexp.Stop();
        musiccbt.Stop();
    }

    public void ExitBossMusic()
    {
        musicboss.Stop();
        musicexp.Play();
        musiccbt.Play();
    }

}
