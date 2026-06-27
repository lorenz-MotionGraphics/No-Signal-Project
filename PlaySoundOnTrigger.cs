using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (other.CompareTag("Player") && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}