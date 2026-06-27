using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private TextMeshProUGUI objectiveTextDisplay;

    [Header("Fade Settings")]
    [SerializeField] private float fadeSpeed = 2f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        // Start hidden
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
        }
    }

    /// <summary>
    /// Call this to make a brand new objective pop up on screen.
    /// </summary>
    public void DisplayObjective(string newObjective)
    {
        if (objectiveTextDisplay == null || panelCanvasGroup == null) return;

        objectiveTextDisplay.text = $"<b>Objective:</b>\n{newObjective}";

        // Stop any running fade-outs so they don't fight each other
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        
        fadeCoroutine = StartCoroutine(FadePanel(1f)); // Fade In
    }

    /// <summary>
    /// Call this when the player completes the objective to make it disappear.
    /// </summary>
    public void CompleteObjective()
    {
        if (panelCanvasGroup == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        
        fadeCoroutine = StartCoroutine(FadePanel(0f)); // Fade Out
    }

    // Smoothly interpolates the alpha transparency of the UI panel
    private IEnumerator FadePanel(float targetAlpha)
    {
        while (!Mathf.Approximately(panelCanvasGroup.alpha, targetAlpha))
        {
            panelCanvasGroup.alpha = Mathf.MoveTowards(panelCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        panelCanvasGroup.alpha = targetAlpha;
    }
}