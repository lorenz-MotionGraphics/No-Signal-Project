using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Look for the script on the object that entered the box
        // We look in children too in case the script is on the Camera
        FlashlightToggle playerScript = other.GetComponentInChildren<FlashlightToggle>();

        if (playerScript != null)
        {
            playerScript.flashlight.enabled = false; // Kill the light
            playerScript.canUseFlashlight = false;   // Lock the 'F' key
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FlashlightToggle playerScript = other.GetComponentInChildren<FlashlightToggle>();

        if (playerScript != null)
        {
            playerScript.canUseFlashlight = true;    // Unlock the 'F' key
            playerScript.flashlight.enabled = true;  // Turn light back on
        }
    }
}