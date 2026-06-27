using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Remove or swap to TMPro if using TextMeshPro

public class RadioRadar : MonoBehaviour
{
    [Header("UI Display Elements")]
    public Image signalPanelScreen;       // The colored background panel on your UI
    public Text radioSignalText;         // The "RADIO SIGNAL:" UI Text string

    [Header("Zone Trigger Colliders")]
    public Collider lowRangeTrigger;     // Inner zone (Closest)
    public Collider midRangeTrigger;     // Middle zone
    public Collider highRangeTrigger;    // Outer zone (Farthest)

    [Header("Target Settings")]
    public string artifactTag = "Interactable";

    [Header("Signal Colors")]
    public Color defaultOffColor = Color.black;
    public Color farColor = Color.red;
    public Color mediumColor = new Color(1f, 0.5f, 0f); // Orange
    public Color closeColor = Color.green;

    // Track state of what artifacts are inside which zones
    private HashSet<Collider> itemsInLow = new HashSet<Collider>();
    private HashSet<Collider> itemsInMid = new HashSet<Collider>();
    private HashSet<Collider> itemsInHigh = new HashSet<Collider>();

    void Update()
    {
        // Only calculate and display the tracking frequency if Left Click is held down
        if (Input.GetButton("Fire1")) 
        {
            UpdateRadarDisplay();
        }
        else
        {
            TurnOffRadar();
        }
    }

    void UpdateRadarDisplay()
    {
        // Priority 1: Check closest proximity zone
        if (itemsInLow.Count > 0)
        {
            signalPanelScreen.color = closeColor;
            radioSignalText.text = "RADIO SIGNAL: STRONG";
        }
        // Priority 2: Check middle proximity zone
        else if (itemsInMid.Count > 0)
        {
            signalPanelScreen.color = mediumColor;
            radioSignalText.text = "RADIO SIGNAL: MEDIUM";
        }
        // Priority 3: Check furthest proximity zone
        else if (itemsInHigh.Count > 0)
        {
            signalPanelScreen.color = farColor;
            radioSignalText.text = "RADIO SIGNAL: WEAK";
        }
        // No artifacts are anywhere nearby
        else
        {
            signalPanelScreen.color = defaultOffColor;
            radioSignalText.text = "RADIO SIGNAL: SEARCHING...";
        }
    }

    void TurnOffRadar()
    {
        // Darken the UI screen panel and reset text when not active
        signalPanelScreen.color = defaultOffColor;
        radioSignalText.text = "RADIO SIGNAL: OFF";
    }

    // These public methods will be called by simple child trigger sensors
    public void ObjectEnteredZone(Collider zone, Collider artifact)
    {
        if (!artifact.CompareTag(artifactTag)) return;

        if (zone == lowRangeTrigger) itemsInLow.Add(artifact);
        if (zone == midRangeTrigger) itemsInMid.Add(artifact);
        if (zone == highRangeTrigger) itemsInHigh.Add(artifact);
    }

    public void ObjectExitedZone(Collider zone, Collider artifact)
    {
        if (!artifact.CompareTag(artifactTag)) return;

        if (zone == lowRangeTrigger) itemsInLow.Remove(artifact);
        if (zone == midRangeTrigger) itemsInMid.Remove(artifact);
        if (zone == highRangeTrigger) itemsInHigh.Remove(artifact);
    }
}