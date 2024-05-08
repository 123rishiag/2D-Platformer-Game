using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource soundEffect;
    public AudioSource soundMusic;

    public Sound[] sounds;

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
        PlayMusic(SoundType.BackgroundMusic);
    }

    public void PlayMusic(SoundType soundType)
    {
        AudioClip soundClip = GetSoundClip(soundType);
        if (soundClip != null)
        {
            soundMusic.clip = soundClip;
            soundMusic.Play();
        }
        else
        {
            Debug.Log("Didnot find any Sound Clip for selected Sound Type");
        }
    }

    public void PlayEffect(SoundType soundType)
    {
        AudioClip soundClip = GetSoundClip(soundType);
        if (soundClip != null) {
            for (int i = 0; i < 2; i++)
            {
                soundEffect.PlayOneShot(soundClip);
            }
        }
        else
        {
            Debug.Log("Didnot find any Sound Clip for selected Sound Type");
        }
    }

    private AudioClip GetSoundClip(SoundType soundType)
    {
        Sound sound = Array.Find(sounds, item=>item.soundType == soundType);
        if(sound != null)
        {
            return sound.soundClip;
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip soundClip;
}

public enum SoundType
{
    ButtonClick,
    ButtonLock,
    ButtonQuit,
    BackgroundMusic,
    ItemPickup,
    PlayerMove,
    PlayerDeath,
    PlayerHurt,
    PlayerJump,
    ChomperMove
}
