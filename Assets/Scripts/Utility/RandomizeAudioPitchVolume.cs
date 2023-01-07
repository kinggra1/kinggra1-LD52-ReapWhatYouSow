using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAudioPitchVolume : MonoBehaviour {

    public float pitchMin = 0.6f;
    public float pitchMax = 1.1f;
    public float volumeMin = 0.4f;
    public float volumeMax = 0.6f;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, volumeMax);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
