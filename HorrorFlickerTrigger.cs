using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HorrorFlickerTrigger : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("Minimum time the light stays in its current state (on/off) during the flicker.")]
    public float minFlickerSpeed = 0.05f;
    [Tooltip("Maximum time the light stays in its current state (on/off) during the flicker.")]
    public float maxFlickerSpeed = 0.25f;
    
    [Header("Duration Settings")]
    [Tooltip("If true, the flicker lasts for 'flickerDuration' seconds. If false, it flickers infinitely until the player leaves.")]
    public bool useDuration = true;
    public float flickerDuration = 3.0f;

    [Header("Trigger Settings")]
    [Tooltip("Tag of your player object to ensure only the player trips the wire.")]
    public string playerTag = "Player";
    [Tooltip("Should this horror event only happen once per game?")]
    public bool triggerOnlyOnce = true;

    private bool isFlickering = false;
    private bool hasTriggered = false;
    private Coroutine flickerCoroutine;
    private FlashlightToggle playerFlashlightScript;
    private bool originalLightState;

    void Awake()
    {
        // Ensure the attached collider is set to a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered && triggerOnlyOnce) return;

        if (other.CompareTag(playerTag))
        {
            // Try to find the FlashlightToggle script on the object entering, or its children/parents
            playerFlashlightScript = other.GetComponentInChildren<FlashlightToggle>();
            
            if (playerFlashlightScript != null && playerFlashlightScript.flashlight != null)
            {
                hasTriggered = true;
                isFlickering = true;
                
                // Store the original state so we don't mess up player configuration afterwards
                originalLightState = playerFlashlightScript.flashlight.enabled;

                // Start the horror!
                flickerCoroutine = StartCoroutine(FlickerRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the player leaves the zone early and we aren't using a locked duration, stop flickering
        if (!useDuration && other.CompareTag(playerTag) && isFlickering)
        {
            StopFlickerEvent();
        }
    }

    private IEnumerator FlickerRoutine()
    {
        float elapsed = 0f;

        // Force flashlight ON initially if it was off to start the horror scare properly
        playerFlashlightScript.flashlight.enabled = true;

        while (!useDuration || elapsed < flickerDuration)
        {
            // Rapidly invert the current enabled state
            playerFlashlightScript.flashlight.enabled = !playerFlashlightScript.flashlight.enabled;

            // Wait a random chaotic split-second
            float randomTime = Random.Range(minFlickerSpeed, maxFlickerSpeed);
            yield return new WaitForSeconds(randomTime);

            if (useDuration)
            {
                elapsed += randomTime;
            }
        }

        StopFlickerEvent();
    }

    private void StopFlickerEvent()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
        }

        isFlickering = false;

        // Restore the flashlight to how the player originally had it before the scare
        if (playerFlashlightScript != null && playerFlashlightScript.flashlight != null)
        {
            playerFlashlightScript.flashlight.enabled = originalLightState;
        }

        // If it's a one-time event, we can destroy this trigger object to save memory
        if (triggerOnlyOnce)
        {
            Destroy(gameObject);
        }
    }
}