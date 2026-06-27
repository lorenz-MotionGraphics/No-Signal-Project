using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishOnTrigger : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Drag the GameObject here that you want to vanish.")]
    [SerializeField] private GameObject objectToVanish;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone is the Player
        if (other.CompareTag("Player"))
        {
            if (objectToVanish != null)
            {
                // Make the object vanish instantly
                objectToVanish.SetActive(false);
            }
            else
            {
                Debug.LogWarning("VanishOnTrigger: No object assigned to vanish!");
            }
        }
    }
}