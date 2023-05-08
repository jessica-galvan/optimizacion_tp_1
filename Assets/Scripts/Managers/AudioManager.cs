using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public SoundReferences soundReferences;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(AudioClip audio)
    {
        if (audio == null) return;

        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }

        musicAudioSource.PlayOneShot(audio);
    }

    public void PlayPlayerSound(AudioClip audio)
    {
        if (audio == null) return;
        playerAudioSource.PlayOneShot(audio);
    }

    public void PlaySFXSound(AudioClip audio)
    {
        if (audio == null) return;
        sfxAudioSource.PlayOneShot(audio);
    }

}
