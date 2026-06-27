using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayer : MonoBehaviour
{
    [Header("Radio Wave Settings")]
    public GameObject wave1;
    public GameObject wave2;
    public GameObject wave3;

    public float waveInterval = 0.2f;
    public float visibleDuration = 1.0f;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float sprintMultiplier = 1.5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.5f;
    public float normalHeight = 2f;
    public float crouchSpeedMultiplier = 0.5f;
    bool isCrouching = false;

    [Header("Jump Settings")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Pause Menu")]
    public GameObject pauseMenuUI;
    public GameObject guidePanel;

    [Header("Footstep Audio")]
    public AudioSource footstepSource;
    public AudioClip footstepClip;

    CharacterController controller;

    Vector3 velocity;
    bool isGrounded;
    bool isPaused = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Time.timeScale = 1f;
        isPaused = false;

        // --- TAG-BASED UI FINDER ---
        if (pauseMenuUI == null)
        {
            GameObject foundPause = GameObject.FindWithTag("PauseMenu");
            if (foundPause != null) pauseMenuUI = foundPause;
        }

        // --- FAILSAFE GUIDE PANEL FINDER ---
        if (guidePanel == null)
        {
            GameObject foundGuide = GameObject.FindWithTag("GuidePanel");
            if (foundGuide != null) 
            {
                guidePanel = foundGuide;
            }
            else 
            {
                // Failsafe: Explicitly set to null and log a gentle warning (optional)
                // This ensures Unity completely ignores it without breaking the script
                guidePanel = null; 
                Debug.LogWarning("SimplePlayer: Optional GuidePanel tag not found. Ignoring guide panel features for this scene.");
            }
        }

        // Auto-assign GroundCheck from your hierarchy image setup
        if (groundCheck == null)
        {
            Transform foundCheck = transform.Find("groundCheck"); // matching your lowercase 'g'
            if (foundCheck != null) groundCheck = foundCheck;
        }
        // ------------------------------------------------------------------

        // Hide UI safely now that they are reconnected
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        
        // This safe-check completely handles the failsafe. If guidePanel is null, it skips this entirely!
        if (guidePanel != null) guidePanel.SetActive(false);

        // Footstep setup
        if (footstepSource != null && footstepClip != null)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true;
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandlePause();

        if (isPaused)
            return;

        // Left Click = Radio Signal
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PlayRadioSignal());
        }

        HandleGroundCheck();
        HandleMovement();
    }

    void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = crouchHeight;
        }
        else
        {
            isCrouching = false;
            controller.height = normalHeight;
        }

    bool isSprinting = Input.GetKey(KeyCode.LeftShift);

    float currentSpeed = speed;

    if (isSprinting && !isCrouching)
        currentSpeed *= sprintMultiplier;

    if (isCrouching)
        currentSpeed *= crouchSpeedMultiplier;

    Vector3 move = transform.right * x + transform.forward * z;

    controller.Move(move * currentSpeed * Time.deltaTime);

    bool isMoving = move.magnitude > 0.1f;

    if (isMoving && isGrounded)
        {
        if (!footstepSource.isPlaying)
            footstepSource.Play();
        }
        else
        {
        if (footstepSource.isPlaying)
            footstepSource.Stop();
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        isPaused = true;

        Time.timeScale = 0f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;

        Time.timeScale = 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowGuide()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (guidePanel != null)
            guidePanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        if (guidePanel != null)
            guidePanel.SetActive(false);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    IEnumerator PlayRadioSignal()
    {
        // Show waves one by one
        if (wave1 != null)
            wave1.SetActive(true);

        yield return new WaitForSeconds(waveInterval);

        if (wave2 != null)
            wave2.SetActive(true);

        yield return new WaitForSeconds(waveInterval);

        if (wave3 != null)
            wave3.SetActive(true);

        // Keep visible
        yield return new WaitForSeconds(visibleDuration);

        // Hide all
        if (wave1 != null)
            wave1.SetActive(false);

        if (wave2 != null)
            wave2.SetActive(false);

        if (wave3 != null)
            wave3.SetActive(false);
    }
}