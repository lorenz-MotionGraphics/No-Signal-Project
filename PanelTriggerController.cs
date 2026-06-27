using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTriggerController : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject uiPanel; // Drag your UI Panel GameObject here

    [Header("Trigger Zone Settings")]
    [Tooltip("If true, entering the zone opens it. If false, entering closes it.")]
    [SerializeField] private bool openOnEnter = true; 
    
    [Tooltip("Check this if you want the opposite action to happen when the player EXITS the collider (e.g., walk in to open, walk out to close).")]
    [SerializeField] private bool useExitTrigger = false;

    // --- TRIGGER ZONE FUNCTIONALITY ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && uiPanel != null)
        {
            SetPanelState(openOnEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If you enabled useExitTrigger, walking out will do the exact opposite of entering
        if (useExitTrigger && other.CompareTag("Player") && uiPanel != null)
        {
            SetPanelState(!openOnEnter);
        }
    }

    // --- PUBLIC FUNCTIONS FOR BUTTONS OR EXTERNAL CALLS ---

    /// <summary>
    /// Forces the panel to turn ON. Link this to an Open Button.
    /// </summary>
    public void OpenPanel()
    {
        SetPanelState(true);
    }

    /// <summary>
    /// Forces the panel to turn OFF. Link this to a Close/X Button.
    /// </summary>
    public void ClosePanel()
    {
        SetPanelState(false);
    }

    /// <summary>
    /// Inverts the current active state (Open if closed, Close if open). Great for a toggle map key/button!
    /// </summary>
    public void TogglePanel()
    {
        if (uiPanel != null)
        {
            SetPanelState(!uiPanel.activeSelf);
        }
    }

    // Helper method to keep code clean
    private void SetPanelState(bool state)
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(state);
        }
    }
}