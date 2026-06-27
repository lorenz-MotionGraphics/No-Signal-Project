using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowstickZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the zone is the player
        FlashlightToggle playerLighting = other.GetComponent<FlashlightToggle>();

        if (playerLighting != null)
        {
            // 1. Block the player's ability to use/toggle the flashlight
            playerLighting.canUseFlashlight = false;

            // 2. Ensure the glowstick component references exist safely
            if (playerLighting.glowstick != null)
            {
                playerLighting.glowstick.enabled = true;
            }

            // 3. Force the flashlight completely off
            if (playerLighting.flashlight != null)
            {
                playerLighting.flashlight.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the zone is the player
        FlashlightToggle playerLighting = other.GetComponent<FlashlightToggle>();

        if (playerLighting != null)
        {
            // 1. Give the player control back over their items
            playerLighting.canUseFlashlight = true;

            // 2. Optional: Turn off the glowstick and turn the flashlight back on automatically
            if (playerLighting.flashlight != null)
            {
                playerLighting.flashlight.enabled = true;
            }
            
            if (playerLighting.glowstick != null)
            {
                playerLighting.glowstick.enabled = false;
            }
        }
    }
}