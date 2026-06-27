using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleTrigger : MonoBehaviour
{
    [Header("Subtitle Settings")]
    [TextArea(3, 5)]
    public string subtitleText;
    public float subtitleDuration = 3f;

    public void TriggerSubtitle()
    {
        if (SubtitleManager.Instance != null)
        {
            SubtitleManager.Instance.DisplaySubtitle(subtitleText, subtitleDuration);
        }
        else
        {
            Debug.LogWarning("SubtitleManager instance missing from the scene!");
        }
    }
}