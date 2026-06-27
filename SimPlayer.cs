using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class SimPlayer : MonoBehaviour
{
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
                guidePanel = null; 
                Debug.LogWarning("SimplePlayer: Optional GuidePanel tag not found. Ignoring guide panel features for this scene.");
            }
        }

        // Auto-assign GroundCheck from hierarchy setup
        if (groundCheck == null)
        {
            Transform foundCheck = transform.Find("groundCheck");
            if (foundCheck != null) groundCheck = foundCheck;
        }
        // ------------------------------------------------------------------

        // Hide UI safely
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
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
}