using UnityEngine;
using UnityEngine.UI;

public class PanelModTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelToShow;
    [SerializeField] private Button closeButton;

    [Header("Save Settings")]
    [Tooltip("Unique key to store in PlayerPrefs so this specific panel never triggers again.")]
    [SerializeField] private string uniquePanelKey = "HasTriggeredFirstTimePanel";

    private void Start()
    {
        // 1. Check if this panel has already been triggered and closed in a past session
        if (PlayerPrefs.GetInt(uniquePanelKey, 0) == 1)
        {
            // If it has already been used, destroy this trigger so it can't happen again
            Destroy(gameObject);
            return;
        }

        // 2. Ensure the panel is hidden at the start of the game
        if (panelToShow != null)
        {
            panelToShow.SetActive(false);
        }

        // 3. Set up the close button listener
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    // Triggered when the player enters the zone
    private void OnTriggerEnter(Collider other)
    {
        // Change "Player" to whatever tag your player object uses
        if (other.CompareTag("Player"))
        {
            OpenPanel();
        }
    }

    // Triggered when the player leaves the zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ClosePanel();
        }
    }

    private void OpenPanel()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }

    private void ClosePanel()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(false);

            // Save to PlayerPrefs that this panel has been completed
            PlayerPrefs.SetInt(uniquePanelKey, 1);
            PlayerPrefs.Save();

            // Destroy the trigger game object immediately so it never triggers again this session
            Destroy(gameObject);
        }
    }

    // Clean up listener if the object is destroyed
    private void OnDestroy()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(ClosePanel);
        }
    }
}