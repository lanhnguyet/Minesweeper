using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip m_soundLogo;
    private AudioSource m_audio;

    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_audio.clip = m_soundLogo;
        m_audio.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_audio.isPlaying)
            {
                m_audio.Pause();
            }
            else m_audio.Play();
        }
    }
}
