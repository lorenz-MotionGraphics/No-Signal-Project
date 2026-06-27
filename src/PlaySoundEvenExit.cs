using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEvenExit : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        // We disable looping because you want the sound to "end" 
        // rather than repeat forever while the player is away.
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // We check if it's already playing so we don't 
            // "restart" the sound every time the player jiggles 
            // inside the trigger.
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    // OnTriggerExit is removed entirely so the audio 
    // keeps playing until the file reaches the end.
}