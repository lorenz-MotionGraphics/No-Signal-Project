using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishMultipleOnTrigger : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Click the '+' button to add as many GameObjects as you want to vanish.")]
    [SerializeField] private List<GameObject> objectsToVanish = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone is the Player
        if (other.CompareTag("Player"))
        {
            bool atLeastOneObjectVanished = false;

            // Loop through the list and disable every assigned object
            foreach (GameObject obj in objectsToVanish)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    atLeastOneObjectVanished = true;
                }
            }

            // Warning if the list was empty or only contained empty slots
            if (!atLeastOneObjectVanished)
            {
                Debug.LogWarning($"VanishMultipleOnTrigger on [{gameObject.name}]: No valid objects were assigned to vanish!");
            }
        }
    }
}