using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    public AudioClip backgroundMusic;

    public AudioClip scytheSwing;
    public AudioClip scytheHit;

    public AudioClip soulCollected;


    private AudioSource playerSfxAudioSource;
    private AudioSource generalSfxAudioSource;
    private AudioSource musicAudioSource;

    private void Awake() {
        playerSfxAudioSource = gameObject.AddComponent<AudioSource>();
        generalSfxAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource = gameObject.AddComponent<AudioSource>();

        musicAudioSource.volume = 0.2f;
        musicAudioSource.clip = backgroundMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayScytheSwing() {
        playerSfxAudioSource.volume = (Random.Range(0.6f, 1f));
        playerSfxAudioSource.pitch = (Random.Range(0.6f, 1.1f));
        playerSfxAudioSource.PlayOneShot(scytheSwing);
    }

    public void PlayScytheHit() {
        playerSfxAudioSource.volume = 0.7f;
        playerSfxAudioSource.pitch = (Random.Range(0.6f, 1.1f));
        playerSfxAudioSource.PlayOneShot(scytheHit);
    }

  
}
