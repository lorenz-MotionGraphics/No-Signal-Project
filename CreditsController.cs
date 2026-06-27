using System.Collections;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    [Header("UI Target")]
    [SerializeField] private GameObject endCreditPanel;
    [SerializeField] private RectTransform creditsContent;

    [Header("Movement Configuration")]
    [SerializeField] private float autoScrollSpeed = 100f;
    [SerializeField] private float manualScrollSpeed = 800f;
    [SerializeField] private float maxScrollHeight = 8000f;

    private bool isScrolling = false;
    private float currentYPosition = 0f;
    private float viewportHeight = 1080f;

    void Start()
{
    // COMMENT THESE LINES OUT TEMPORARILY:
    // if (endCreditPanel != null)
    //     endCreditPanel.SetActive(false);

    currentYPosition = 0f;
    if (creditsContent != null)
    {
        creditsContent.anchoredPosition = new Vector2(0, currentYPosition);
    }

    // FORCE IT TO RUN IMMEDIATELY ON PLAY FOR TESTING:
    isScrolling = true; 
}

    // This fixes the 'StartCredits' compilation error!
    public void StartCredits()
    {
        if (endCreditPanel != null)
        {
            endCreditPanel.SetActive(true);
        }
        
        currentYPosition = 0f;
        isScrolling = true;
    }

    void Update()
    {
        if (!isScrolling || creditsContent == null) return;

        // 1. Read manual mouse scroll wheel input directly
        float mouseInput = Input.GetAxis("Mouse ScrollWheel");
        
        if (Mathf.Abs(mouseInput) > 0.01f)
        {
            currentYPosition += -mouseInput * manualScrollSpeed;
        }
        else
        {
            // 2. Otherwise, smoothly auto-scroll upward
            currentYPosition += autoScrollSpeed * Time.deltaTime;
        }

        // 3. Keep boundaries locked
        float maximumTrackLimit = maxScrollHeight - viewportHeight;
        currentYPosition = Mathf.Clamp(currentYPosition, 0f, maximumTrackLimit);

        creditsContent.anchoredPosition = new Vector2(0, currentYPosition);

        // 4. End check
        if (currentYPosition >= maximumTrackLimit)
        {
            isScrolling = false;
            Debug.Log("CREDITS COMPLETE!");
        }
    }
}