using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicmanager : MonoBehaviour
{
    public AudioSource musicexp;
    public AudioSource musiccbt;
    public AudioSource musicboss;
    public bool playcbt;
    public float musiccbtvol;
    public bool musiccbtinc;
    public float musicexpvol;
    public bool musicexpinc;
    public float cbtactvol;
    public float expactvol;
    public float timeincr;

    private Global Global;
    private void Start()
    {
        Global =FindAnyObjectByType<Global>();
    }
    void FixedUpdate()
    {
        if(!Global.inbossfight)
        {
            if (musiccbt.volume < musiccbtvol + 0.001 && musiccbt.volume > musiccbtvol - 0.001 && musiccbtinc)
            {
                musiccbtinc = false;
            }

            if (musicexp.volume < musicexpvol + 0.001 && musicexp.volume > musicexpvol - 0.001 && musicexpinc)
            {
                musicexpinc = false;
            }

            if (musiccbtinc & musicexpinc)
            {
                musiccbtinc = false;
            }

            if (musiccbtinc & !musicexpinc)
            {
                if (musiccbt.volume <= musiccbtvol)
                {
                    musiccbt.volume = musiccbt.volume + musiccbtvol * timeincr;
                    musicexp.volume = musicexp.volume - musicexpvol * timeincr;
                }
                else
                {
                    musiccbtinc = false;
                    musicexp.volume = 0;
                }
            }


            if (musicexpinc & !musiccbtinc)
            {
                if (musicexp.volume <= musicexpvol)
                {
                    musicexp.volume = musicexp.volume + musicexpvol * timeincr;
                    musiccbt.volume = musiccbt.volume - musiccbtvol * timeincr;
                }
                else
                {
                    musicexpinc = false;
                    musiccbt.volume = 0;
                }
            }
            cbtactvol = musiccbt.volume;
            expactvol = musicexp.volume;
            if (playcbt)
            {
                if (musiccbt.volume < 0.0001f)
                {
                    musiccbtinc = true;
                }
            }
            else
            {
                if (musicexp.volume < 0.0001f)
                {
                    musicexpinc = true;
                }
            }
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
