using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnMovement : MonoBehaviour
{
    [Tooltip("How fast the object needs to move to trigger the sound.")]
    public float movementThreshold = 0.001f;

    private AudioSource audioSource;
    private Vector3 lastPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate distance moved this frame
        float movementDistance = Vector3.Distance(transform.position, lastPosition);

        // If it moved more than the threshold, play/keep playing
        if (movementDistance > movementThreshold)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // If it's not moving, stop the sound immediately
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); 
            }
        }

        // Store position for the next frame
        lastPosition = transform.position;
    }
}