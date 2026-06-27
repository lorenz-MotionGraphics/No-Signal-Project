using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    public enum TriggerType { AudioAndSubtitle, SubtitleOnly, AudioOnly }

    [Header("Mode Configuration")]
    [Tooltip("Choose what happens when this object is interacted with.")]
    public TriggerType interactionMode = TriggerType.AudioAndSubtitle;

    [Header("Audio Settings")]
    [Tooltip("The audio clip to play when right-clicked.")]
    public AudioClip interactionAudio;
    [HideInInspector] public AudioSource audioSource; // Hidden in inspector to avoid clutter

    [Header("Subtitle Settings")]
    [Tooltip("The text to display on screen.")]
    [TextArea(3, 5)]
    public string subtitleText;
    
    [Tooltip("How long the subtitle stays on screen (used if Mode is SubtitleOnly, or as fallback).")]
    public float subtitleDuration = 3f;

    // Static variables to keep track of the active subtitle across any object clicked
    private static string currentActiveSubtitle = "";
    private static float subtitleTimer = 0f;

    /// <summary>
    /// This is the method your ObjectHandler calls when the player right-clicks the highlighted object.
    /// </summary>
    public void TriggerInteraction()
    {
        bool useAudio = interactionMode == TriggerType.AudioAndSubtitle || interactionMode == TriggerType.AudioOnly;
        bool useSubtitle = interactionMode == TriggerType.AudioAndSubtitle || interactionMode == TriggerType.SubtitleOnly;

        float activeDuration = subtitleDuration;

        // 1. Handle Audio Logic safely
        if (useAudio && interactionAudio != null)
        {
            activeDuration = interactionAudio.length; // Sync text duration to audio length
            PlayAudioFallback();
        }

        // 2. Handle Subtitle Logic safely
        if (useSubtitle && !string.IsNullOrEmpty(subtitleText))
        {
            currentActiveSubtitle = subtitleText;
            subtitleTimer = activeDuration;
        }
    }

    private void Update()
    {
        // Countdown global subtitle timer if it's active
        if (subtitleTimer > 0)
        {
            subtitleTimer -= Time.deltaTime;
            if (subtitleTimer <= 0)
            {
                currentActiveSubtitle = "";
            }
        }
    }

    private void PlayAudioFallback()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.clip = interactionAudio;
            audioSource.Play();
        }
        else
        {
            // If no AudioSource is attached, play it dynamically at the object's position
            AudioSource.PlayClipAtPoint(interactionAudio, transform.position);
        }
    }

    // A lightweight, zero-setup UI fallback that draws text on screen automatically
    private void OnGUI()
    {
        if (!string.IsNullOrEmpty(currentActiveSubtitle))
        {
            GUIStyle subtitleStyle = new GUIStyle(GUI.skin.label);
            subtitleStyle.alignment = TextAnchor.MiddleCenter;
            subtitleStyle.fontSize = 24;
            subtitleStyle.normal.textColor = Color.white;

            // Simple black drop-shadow behind text for readability
            GUIStyle shadowStyle = new GUIStyle(subtitleStyle);
            shadowStyle.normal.textColor = Color.black;

            Rect rect = new Rect(0, Screen.height - 120, Screen.width, 50);
            
            // Draw shadow, then text
            GUI.Label(new Rect(rect.x + 2, rect.y + 2, rect.width, rect.height), currentActiveSubtitle, shadowStyle);
            GUI.Label(rect, currentActiveSubtitle, subtitleStyle);
        }
    }
}