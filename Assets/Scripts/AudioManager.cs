using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------Audio Source----------")]
    [SerializeField]
    public AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    
    [Header("---------Audio Clip----------")]
    public AudioClip background;
    public AudioClip gameOver;
    public AudioClip gameWin;
    public AudioClip nextPage;
    public AudioClip Notification;
    
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
