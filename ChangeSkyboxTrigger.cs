using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyboxTrigger : MonoBehaviour
{
    [Header("Skybox Material")]
    [Tooltip("Drag the new Skybox Material here.")]
    [SerializeField] private Material newSkyboxMaterial;

    [Header("Environment Lighting Settings")]
    [Tooltip("Check this if you want to change the ambient lighting intensity when triggered.")]
    [SerializeField] private bool changeLightingIntensity = false;
    [Tooltip("Controls the brightness of the ambient light (RenderSettings.ambientIntensity).")]
    [Range(0f, 8f)] [SerializeField] private float targetLightingIntensity = 1f;

    [Header("Environment Reflection Settings")]
    [Tooltip("Check this if you want to change the reflection intensity when triggered.")]
    [SerializeField] private bool changeReflectionIntensity = false;
    [Tooltip("Controls the strength of the skybox reflections (RenderSettings.reflectionIntensity).")]
    [Range(0f, 1f)] [SerializeField] private float targetReflectionIntensity = 1f;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure it's the player entering the zone
        if (other.CompareTag("Player"))
        {
            // 1. Swap the actual skybox material if assigned
            if (newSkyboxMaterial != null)
            {
                RenderSettings.skybox = newSkyboxMaterial;
            }
            else
            {
                Debug.LogWarning("ChangeSkyboxTrigger: No Skybox Material assigned!");
            }

            // 2. Adjust Environment Lighting Intensity
            if (changeLightingIntensity)
            {
                RenderSettings.ambientIntensity = targetLightingIntensity;
            }

            // 3. Adjust Reflection Intensity Multiplier
            if (changeReflectionIntensity)
            {
                RenderSettings.reflectionIntensity = targetReflectionIntensity;
            }

            // 4. Force Unity to update the scene's ambient lighting and reflections
            // This ensures the new intensities and HDRI colors take effect immediately!
            DynamicGI.UpdateEnvironment();
        }
    }
}