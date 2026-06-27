using UnityEngine;

public class LinkHandler : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Add all panels here. If ANY are active, this button hides.")]
    [SerializeField] private GameObject[] uiPanels; 
    
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        if (uiPanels == null || uiPanels.Length == 0) return;

        // Check if at least one panel in the list is active
        bool anyPanelOpen = false;
        foreach (GameObject panel in uiPanels)
        {
            if (panel != null && panel.activeSelf)
            {
                anyPanelOpen = true;
                break; // One is enough to hide the button, so we stop checking
            }
        }

        // Apply visibility based on the check
        if (anyPanelOpen)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OpenURL(string url)
    {
        // Safety check: Don't open link if button is hidden
        if (canvasGroup.alpha < 0.1f) return;

        Application.OpenURL(url);
    }
}