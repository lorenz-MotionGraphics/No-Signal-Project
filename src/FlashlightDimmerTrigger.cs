using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightDimmerTrigger : MonoBehaviour
{
    public enum TargetLevel { Short, Medium, Long }

    [Header("Settings")]
    [SerializeField] private TargetLevel forcedLevel = TargetLevel.Short;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the zone is the player
        if (other.CompareTag(playerTag))
        {
            FlashlightToggle flashlightSystem = other.GetComponentInChildren<FlashlightToggle>();

            if (flashlightSystem != null)
            {
                // Determine which float value to send based on your dropdown selection
                float targetRange = flashlightSystem.shortRange;
                if (forcedLevel == TargetLevel.Medium) targetRange = flashlightSystem.mediumRange;
                if (forcedLevel == TargetLevel.Long) targetRange = flashlightSystem.longRange;

                // Lock it down!
                flashlightSystem.LockFlashlightRange(targetRange);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player is leaving the zone
        if (other.CompareTag(playerTag))
        {
            FlashlightToggle flashlightSystem = other.GetComponentInChildren<FlashlightToggle>();

            if (flashlightSystem != null)
            {
                // Restore their freedom
                flashlightSystem.UnlockFlashlightRange();
            }
        }
    }
}