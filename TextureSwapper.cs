using UnityEngine;

public class TextureSwapper : MonoBehaviour
{
    [Header("Texture Swap Setup (Optional)")]
    [Tooltip("Drag the GameObject whose texture you want to change here.")]
    public GameObject targetObject;

    [Tooltip("Drag the new Texture2D asset here. If left empty, texture swapping is safely skipped.")]
    public Texture2D newTexture;

    [Header("Visibility Setup (Optional)")]
    [Tooltip("Drag the unchecked/hidden Cube or GameObject here. It will be turned ON when triggered.")]
    public GameObject objectToReveal;

    [Header("Audio Setup (Optional)")]
    [Tooltip("Drag your sound effect asset here.")]
    public AudioClip soundToPlay;

    [Header("Settings")]
    [Tooltip("The tag of the object that triggers this script (usually 'Player').")]
    public string targetTag = "Player";

    [Tooltip("If true, this will only trigger once per game simulation.")]
    public bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private Renderer objectRenderer;
    private AudioSource audioSource;

    void Start()
    {
        // Fail-safe: Only attempt to grab the renderer if a target object was provided
        if (targetObject != null)
        {
            objectRenderer = targetObject.GetComponent<Renderer>();
        }

        // Fail-safe: Check if an AudioSource exists, or add one dynamically if a sound is provided
        if (soundToPlay != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // Automatically adds an AudioSource at runtime so you don't break your workflow
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Turn off 3D spatial settings that might make it completely silent by default
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Fail-safe: Prevent triggering multiple times if set to true
        if (hasTriggered && triggerOnlyOnce) return;

        // Check if the entering object is the Player
        if (other.CompareTag(targetTag))
        {
            hasTriggered = true;

            // 1. TEXTURE SWAP LOGIC (With Fail-safes)
            if (targetObject != null && newTexture != null && objectRenderer != null)
            {
                objectRenderer.material.mainTexture = newTexture;
                Debug.Log($"[TriggerSystem] Successfully changed texture on {targetObject.name}");
            }

            // 2. VISIBILITY LOGIC (With Fail-safe)
            if (objectToReveal != null)
            {
                objectToReveal.SetActive(true);
                Debug.Log($"[TriggerSystem] Successfully revealed {objectToReveal.name}");
            }

            // 3. AUDIO LOGIC (With Fail-safe)
            if (soundToPlay != null && audioSource != null)
            {
                audioSource.PlayOneShot(soundToPlay);
                Debug.Log($"[TriggerSystem] Playing audio: {soundToPlay.name}");
            }
        }
    }
}