using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip[] axeSounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PlayAxeChop() {
        // Play a random axe sound
        audioSource.PlayOneShot(axeSounds[Random.Range(0, axeSounds.Length)]);
    }

}
