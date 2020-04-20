using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicTracks;

    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (!audioSource.isPlaying) {
            // Play a new song if not playing anything
            audioSource.PlayOneShot(musicTracks[Random.Range(0, musicTracks.Length)]);
        }
    }
}
