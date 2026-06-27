using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraSkipController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button skipButton;

    [Header("Look Target Anchor")]
    [SerializeField] private Transform lookAtTarget;

    [Header("Player Camera Components")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerBodyTransform;

    [Header("Player Mouse Look Reference")]
    [Tooltip("Drag the Main Camera here. This generic slot accepts any script style!")]
    [SerializeField] private MonoBehaviour mouseLookScript;

    void Start()
    {
        // Try to auto-grab references if left blank
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (playerBodyTransform == null && cameraTransform != null)
        {
            // Usually the Main Camera is a child of the Player Body
            playerBodyTransform = cameraTransform.parent; 
        }

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(OnSkipButtonClicked);
        }
    }

    void OnEnable()
    {
        // Disable the mouse look instantly when the UI panel opens
        if (mouseLookScript != null) mouseLookScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnSkipButtonClicked()
    {
        if (lookAtTarget == null || cameraTransform == null || playerBodyTransform == null)
        {
            Debug.LogWarning("Camera Skip Controller is missing target/camera transform assignments!");
            return;
        }

        // Run via a Coroutine to ensure the mouse controller doesn't override the positions on the same frame
        StartCoroutine(ExecuteSkipSequence());
    }

    private IEnumerator ExecuteSkipSequence()
    {
        // 1. Double check the mouse look script stays disabled while we calculate
        if (mouseLookScript != null) mouseLookScript.enabled = false;

        // 2. Point the player body horizontally at the target anchor
        Vector3 targetPostitionOnPlane = new Vector3(lookAtTarget.position.x, playerBodyTransform.position.y, lookAtTarget.position.z);
        playerBodyTransform.LookAt(targetPostitionOnPlane);

        // 3. Force the camera to look at the target (matches vertical alignment)
        cameraTransform.LookAt(lookAtTarget.position);

        // 4. Extract the clean vertical angle and completely strip out glitched horizontal axis data
        float localXAngle = cameraTransform.localEulerAngles.x;
        cameraTransform.localRotation = Quaternion.Euler(localXAngle, 0f, 0f);

        // 5. Wait 1 physics frame for Unity to register the new rotation data internally
        yield return new WaitForEndOfFrame();

        // 6. Reset UI states back to standard gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        skipButton.interactable = false;

        // 7. Re-enable the mouse look script. It will now wake up with the camera facing forward.
        if (mouseLookScript != null) mouseLookScript.enabled = true;
    }

    void OnDestroy()
    {
        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(OnSkipButtonClicked);
        }
    }
}