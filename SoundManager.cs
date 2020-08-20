using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip win, lose;
    public AudioSource maudio;
    public AudioClip[] audi;

    public static SoundManager Instance { get; private set; }

    void Start()
    {
        win = Resources.Load<AudioClip>("Swordland");
        lose = Resources.Load<AudioClip>("lose");

        maudio = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        Instance = this;
        GameObject.FindGameObjectWithTag("sound").GetComponent<SoundManager>();
    }

    public void Playsound(string clip)
    {
        switch (clip)
        {
            case "Swordland":
                maudio.clip = win;
                maudio.volume = 1f;
                maudio.Play();
                break;

            case "lose":
                maudio.clip = lose;
                maudio.volume = 1f;
                maudio.Play();
                break;
        }
    }

    public void Playbase()
    {
        if (maudio)
        {
            int randIdx = Random.Range(0, audi.Length);
            if (audi[randIdx])
            {
                maudio.clip = audi[randIdx];
                maudio.volume = 0.5f;
                maudio.Play();
            }
        }  
    }

    public void Stop()
    {
        maudio.Stop();        
    }

    public void Pause()
    {
        maudio.Pause();
    }

    public void UnPause()
    {
        maudio.UnPause();
    }
}
