using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [Header("Light References")]
    public Light flashlight;    
    public Light glowstick;     
    
    [Header("Controls")]
    public KeyCode toggleKey = KeyCode.F;
    public KeyCode glowstickKey = KeyCode.G;
    public bool canUseFlashlight = true;

    [Header("Flashlight Range Levels")]
    public float shortRange = 10f;
    public float mediumRange = 20f;
    public float longRange = 40f;

    [Header("Glowstick Colors")]
    public Color color1 = Color.green;
    public Color color2 = Color.red;
    public Color color3 = Color.yellow;
    public Color color4 = Color.blue;

    private bool hasFlashlight;
    private bool hasGlowstick;
    
    // NEW: Track if the range controls are locked by a trigger zone
    private bool controlsLocked = false;
    private float normalRange;

    void Start()
    {
        hasFlashlight = flashlight != null;
        hasGlowstick = glowstick != null;

        if (hasFlashlight) 
        {
            flashlight.enabled = true;
            flashlight.range = mediumRange;
            normalRange = mediumRange; // Store the initial default range
        }

        if (hasGlowstick) 
        {
            glowstick.enabled = false;
            glowstick.color = color1;
        }
    }

    void Update()
    {
        if (!canUseFlashlight) return;

        if (hasFlashlight) HandleFlashlight();
        if (hasGlowstick) HandleGlowstick();
        
        // Only handle shared controls (1, 2, 3, 4) if they aren't locked
        if (!controlsLocked)
        {
            HandleSharedControls();
        }
    }

    void HandleFlashlight()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            flashlight.enabled = !flashlight.enabled;
            if (flashlight.enabled && hasGlowstick) glowstick.enabled = false;
        }
    }

    void HandleGlowstick()
    {
        if (Input.GetKeyDown(glowstickKey))
        {
            glowstick.enabled = !glowstick.enabled;
            if (glowstick.enabled && hasFlashlight) flashlight.enabled = false;
        }
    }

    void HandleSharedControls()
    {
        bool flashOn = hasFlashlight && flashlight.enabled;
        bool glowOn = hasGlowstick && glowstick.enabled;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (flashOn) SetFlashlightRange(shortRange);
            if (glowOn)  glowstick.color = color1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (flashOn) SetFlashlightRange(mediumRange);
            if (glowOn)  glowstick.color = color2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (flashOn) SetFlashlightRange(longRange);
            if (glowOn)  glowstick.color = color3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (glowOn)  glowstick.color = color4;
        }
    }

    // Helper to change range and remember it as the "normal" state
    private void SetFlashlightRange(float newRange)
    {
        flashlight.range = newRange;
        if (!controlsLocked) normalRange = newRange; 
    }

    // --- NEW PUBLIC FUNCTIONS FOR THE TRIGGER BOX ---

    /// <summary>
    /// Locks the flashlight to a specific range (e.g., shortRange) and disables player adjustments.
    /// </summary>
    public void LockFlashlightRange(float forcedRange)
    {
        if (!hasFlashlight) return;
        
        controlsLocked = true;
        flashlight.range = forcedRange;
    }

    /// <summary>
    /// Unlocks the flashlight and restores it to whatever level the player had it on before entering.
    /// </summary>
    public void UnlockFlashlightRange()
    {
        if (!hasFlashlight) return;

        controlsLocked = false;
        flashlight.range = normalRange; // Go back to what it was
    }
}