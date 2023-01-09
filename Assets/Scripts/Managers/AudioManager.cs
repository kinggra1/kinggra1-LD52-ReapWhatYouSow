using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    public AudioClip backgroundMusic;

    public AudioClip scytheSwing;
    public AudioClip scytheHit;

    public AudioClip soulCollected;

    public AudioClip plantingSound;

    private AudioSource playerSfxAudioSource;
    private AudioSource generalSfxAudioSource;
    private AudioSource musicAudioSource;

    private void Awake() {
        playerSfxAudioSource = gameObject.AddComponent<AudioSource>();
        generalSfxAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource = gameObject.AddComponent<AudioSource>();

        musicAudioSource.volume = 0.1f;
        musicAudioSource.clip = backgroundMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlaySoulCollected() {
        generalSfxAudioSource.volume = (Random.Range(0.05f, 0.1f));
        generalSfxAudioSource.pitch = (Random.Range(0.8f, 1f));
        generalSfxAudioSource.PlayOneShot(soulCollected);
    }

    public void PlayScytheSwing() {
        playerSfxAudioSource.volume = (Random.Range(0.5f, 0.7f));
        playerSfxAudioSource.pitch = (Random.Range(0.9f, 1.1f));
        playerSfxAudioSource.PlayOneShot(scytheSwing);
    }

    public void PlayScytheHit() {
        playerSfxAudioSource.volume = 0.1f;
        playerSfxAudioSource.pitch = (Random.Range(0.8f, 1.1f));
        playerSfxAudioSource.PlayOneShot(scytheHit);
    }

    public void PlayPlantingSound() {
        generalSfxAudioSource.volume = (Random.Range(0.2f, 0.3f));
        generalSfxAudioSource.pitch = (Random.Range(0.7f, 1.1f));
        generalSfxAudioSource.PlayOneShot(plantingSound);
    }


}
