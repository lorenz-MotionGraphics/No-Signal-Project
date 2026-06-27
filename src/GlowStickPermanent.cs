using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowStickPermanent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the zone is the player
        FlashlightToggle playerLighting = other.GetComponent<FlashlightToggle>();

        if (playerLighting != null)
        {
            // 1. Block the player's ability to use/toggle the flashlight permanently
            playerLighting.canUseFlashlight = false;

            // 2. Ensure the glowstick component references exist safely and turn it on
            if (playerLighting.glowstick != null)
            {
                playerLighting.glowstick.enabled = true;
            }

            // 3. Force the flashlight completely off
            if (playerLighting.flashlight != null)
            {
                playerLighting.flashlight.enabled = false;
            }

            // 4. Destroy this trigger zone so the effect cannot be altered or re-triggered
            Destroy(gameObject); 
        }
    }
    
    // OnTriggerExit has been completely removed so the player keeps the glowstick forever.
}