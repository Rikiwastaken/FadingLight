using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private optionvalues optionvalues;

    public AudioSource GotItemSource;
    public float GotItemBaseVolume;

    public AudioSource ConsoleButtonSource;
    public float ConsoleButtonBaseVolume;

    private void Start()
    {
        optionvalues = FindAnyObjectByType<optionvalues>();
    }


    public void PlayGotItem()
    {
        GotItemSource.volume = GotItemBaseVolume * optionvalues.options.soundvol;
        GotItemSource.Play();
    }

    public void PlayConsoleButton()
    {
        ConsoleButtonSource.volume = ConsoleButtonBaseVolume * optionvalues.options.soundvol;
        ConsoleButtonSource.Play();
    }

}
