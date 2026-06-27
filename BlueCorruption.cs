using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCorruption : MonoBehaviour
{
    private bool playerIsInsideZone = false;

    void Update()
    {
        // Only allow clicking if the player is actually standing inside this trigger zone
        if (!playerIsInsideZone) return;

        if (Input.GetMouseButtonDown(1)) // Right Click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Interactable");

            // Shoot a ray that ONLY detects objects on the "Interactable" layer
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
            {
                // The object disappears instantly!
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInsideZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInsideZone = false;
        }
    }
}