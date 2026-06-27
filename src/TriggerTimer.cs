using UnityEngine;
using UnityEngine.UI; // Required for standard UI Text
using TMPro;        // Required for 3D TextMeshPro

public class TriggerTimer : MonoBehaviour
{
    [Header("Detection Target")]
    [Tooltip("The 3D Mesh TextMeshPro object in the scene to monitor.")]
    [SerializeField] private TextMeshPro target3DTextMesh; 
    [SerializeField] private string stopWord = "Ok";

    [Header("UI Panels")]
    [Tooltip("The Canvas Panel/Object that holds your ticking countdown.")]
    [SerializeField] private GameObject timerDisplayPanel;
    
    [Tooltip("The Game Over / End Panel that shows up when the timer hits ZERO.")]
    [SerializeField] private GameObject endPanel;

    [Header("Timer Text Component")]
    [Tooltip("The legacy standard UI Text component displaying the countdown numbers.")]
    [SerializeField] private Text timerText; 

    [Header("Timer Settings")]
    [Tooltip("Countdown time in minutes.")]
    [SerializeField] private float countdownMinutes = 1.0f;

    private float timeRemaining;
    private bool isTimerRunning = false;
    private bool hasTriggered = false;

    void Start()
    {
        // Keep both panels hidden until needed
        if (timerDisplayPanel != null)
            timerDisplayPanel.SetActive(false);

        if (endPanel != null)
            endPanel.SetActive(false);

        timeRemaining = countdownMinutes * 60f;
        UpdateTimerText();
    }

    void Update()
    {
        // 1. Monitor the 3D Text Mesh Pro component continuously
        CheckTextCondition();

        // 2. Handle Countdown Logic if active
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimerText();
                OnTimerComplete(); // This activates the panel!
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // Verify if it's already "Ok" before starting
            if (target3DTextMesh != null && target3DTextMesh.text.Trim() == stopWord)
            {
                return; 
            }

            isTimerRunning = true;
            hasTriggered = true;
            
            if (timerDisplayPanel != null)
                timerDisplayPanel.SetActive(true);
        }
    }

    private void CheckTextCondition()
    {
        if (target3DTextMesh == null || !hasTriggered) return;

        if (target3DTextMesh.text.Trim() == stopWord)
        {
            StopAndHideTimer();
        }
    }

    private void StopAndHideTimer()
    {
        isTimerRunning = false;
        
        if (timerDisplayPanel != null)
            timerDisplayPanel.SetActive(false);
    }

    void OnTimerComplete()
    {
        // FIX: Explicitly open the End/Pause Panel!
        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }
        
        // Hide the active timer digits so they don't look stuck at 00:00
        if (timerDisplayPanel != null) 
        {
            timerDisplayPanel.SetActive(false);
        }

        // Pause the game mechanics completely
        Time.timeScale = 0f; 
    }

    // Linked to your UI Reset Button's onClick event
    public void ResetTimer()
    {
        Time.timeScale = 1f; // Unpause the game engine

        timeRemaining = countdownMinutes * 60f;
        isTimerRunning = false;
        hasTriggered = false;

        // Hide the End panel again
        if (endPanel != null)
            endPanel.SetActive(false);

        // Check if the 3D text is still saying "Ok" before allowing another run
        if (target3DTextMesh != null && target3DTextMesh.text.Trim() == stopWord)
        {
            StopAndHideTimer();
        }
        else
        {
            if (timerDisplayPanel != null)
                timerDisplayPanel.SetActive(false); // Hide until next box entry

            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60F);
        int seconds = Mathf.FloorToInt(timeRemaining % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}