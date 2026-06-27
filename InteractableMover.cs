using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Should the position change relative to its current spot, or move to an absolute world coordinate?")]
    public bool useRelativeMovement = true;
    
    [Tooltip("The destination offset (if relative) or absolute coordinates (if world space).")]
    public Vector3 targetPosition;

    [Header("Rotation Settings")]
    [Tooltip("Should the rotation change relative to its current angles, or match absolute world angles?")]
    public bool useRelativeRotation = true;
    
    [Tooltip("The target rotation angles (Euler angles like X, Y, Z).")]
    public Vector3 targetRotationAngles;

    [Header("Animation Settings")]
    [Tooltip("How long the movement and rotation animation takes in seconds.")]
    public float transitionDuration = 1.5f;

    [Header("Optional Subtitle")]
    [Tooltip("The text to display on screen when the object begins moving. Optional.")]
    [TextArea(3, 5)]
    public string subtitleText;
    
    [Tooltip("How long the subtitle stays on screen.")]
    public float subtitleDuration = 3f;

    private bool isMoving = false;

    // Static variables sharing the UI display with your other script
    private static string activeMoverSubtitle = "";
    private static float moverSubtitleTimer = 0f;

    /// <summary>
    /// Called by ObjectHandler when the player right-clicks the highlighted object.
    /// </summary>
    public void TriggerMovement()
    {
        // Prevent triggering multiple times if it is already animating
        if (isMoving) return;

        // 1. Trigger optional subtitle
        if (!string.IsNullOrEmpty(subtitleText))
        {
            activeMoverSubtitle = subtitleText;
            moverSubtitleTimer = subtitleDuration;
        }

        // 2. Calculate paths and start the smooth transition
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = useRelativeMovement ? startPos + targetPosition : targetPosition;
        Quaternion endRot = useRelativeRotation ? startRot * Quaternion.Euler(targetRotationAngles) : Quaternion.Euler(targetRotationAngles);

        StartCoroutine(AnimateMoveAndRotate(startPos, endPos, startRot, endRot));
    }

    private IEnumerator AnimateMoveAndRotate(Vector3 startP, Vector3 endP, Quaternion startR, Quaternion endR)
    {
        isMoving = true;
        float elapsedTime = 0f;

        // Temporarily disable physics if it has a Rigidbody so it doesn't fight the animation
        Rigidbody rb = GetComponent<Rigidbody>();
        bool originalKinematicState = false;
        if (rb != null)
        {
            originalKinematicState = rb.isKinematic;
            rb.isKinematic = true;
        }

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // Smoothstep curve for elegant acceleration and deceleration
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startP, endP, t);
            transform.rotation = Quaternion.Slerp(startR, endR, t);

            yield return null;
        }

        // Ensure final destination values are absolute
        transform.position = endP;
        transform.rotation = endR;

        // Restore physics status if needed
        if (rb != null)
        {
            rb.isKinematic = originalKinematicState;
        }

        isMoving = false;
    }

    private void Update()
    {
        if (moverSubtitleTimer > 0)
        {
            moverSubtitleTimer -= Time.deltaTime;
            if (moverSubtitleTimer <= 0)
            {
                activeMoverSubtitle = "";
            }
        }
    }

    private void OnGUI()
    {
        string textToShow = !string.IsNullOrEmpty(activeMoverSubtitle) ? activeMoverSubtitle : "";
        
        if (!string.IsNullOrEmpty(textToShow))
        {
            GUIStyle subtitleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 24
            };
            subtitleStyle.normal.textColor = Color.white;

            GUIStyle shadowStyle = new GUIStyle(subtitleStyle);
            shadowStyle.normal.textColor = Color.black;

            Rect rect = new Rect(0, Screen.height - 120, Screen.width, 50);
            
            GUI.Label(new Rect(rect.x + 2, rect.y + 2, rect.width, rect.height), textToShow, shadowStyle);
            GUI.Label(rect, textToShow, subtitleStyle);
        }
    }
}