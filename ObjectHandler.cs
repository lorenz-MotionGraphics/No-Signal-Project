using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public float interactionDistance = 5f;
    public LayerMask interactableLayer;
    
    private GameObject lastHighlighted;
    private GameObject grabbedObject;
    private Color originalColor;
    private Transform holdPoint;

    private Vector3 defaultHoldPosition = new Vector3(0, 0, 3f);

    void Start()
    {
        // Create a point in front of the camera to hold the object
        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(transform);
        holdPoint.localPosition = new Vector3(0, 0, 3f); 
    }

    void Update()
    {
        HandleHighlight();
        HandlePickup();

        if (grabbedObject != null)
        {
            PreventWallClipping();
        }
    }

    void PreventWallClipping()
    {
        // Raycast from the camera center to where the hold point wants to be
        Vector3 origin = transform.position;
        Vector3 targetWorldPos = transform.TransformPoint(defaultHoldPosition);
        Vector3 direction = targetWorldPos - origin;
        float distance = direction.magnitude;

        // Ignore the object we are currently holding by masking it out
        int layerMaskWithoutInteractables = ~interactableLayer;

        // Linecast/Raycast to see if an interior mesh wall/floor is in between the player and the hold point
        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance, layerMaskWithoutInteractables))
        {
            // A wall/floor was hit! Push the hold point back slightly in front of the surface hit point
            holdPoint.position = hit.point + (hit.normal * 0.25f);
        }
        else
        {
            // Path is clear, keep it at its standard distance
            holdPoint.localPosition = defaultHoldPosition;
        }
    }

    void HandleHighlight()
    {
        if (grabbedObject != null) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            GameObject currentObj = hit.collider.gameObject;

            if (lastHighlighted != currentObj)
            {   
                ResetHighlight();
                lastHighlighted = currentObj;
                Renderer rend = lastHighlighted.GetComponent<Renderer>();
            
                if (rend != null)
                {
                    originalColor = GetMaterialColor(rend.material);
                    SetMaterialColor(rend.material, Color.white);
                }
            }
        }
        else
        {
            ResetHighlight();
        }
    }

    // Helper to handle different shader property names
    Color GetMaterialColor(Material mat) {
        if (mat.HasProperty("_BaseColor")) return mat.GetColor("_BaseColor");
        return mat.color;
    }

    void SetMaterialColor(Material mat, Color col) {
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", col);
        else mat.color = col;
    }
    void ResetHighlight()
{
    if (lastHighlighted != null)
    {
        Renderer rend = lastHighlighted.GetComponent<Renderer>();
        if (rend != null)
        {
            SetMaterialColor(rend.material, originalColor);
        }
        lastHighlighted = null;
    }
}

    void HandlePickup()
{
    if (Input.GetMouseButtonDown(1)) // Right Click
    {
        if (grabbedObject == null && lastHighlighted != null)
        {
            // 1. Check for the Audio/Subtitle Trigger
            InteractableTrigger audioTrigger = lastHighlighted.GetComponent<InteractableTrigger>();
            if (audioTrigger != null)
            {
                audioTrigger.TriggerInteraction();
                return; 
            }

            // 2. Check for the Movement Trigger
            InteractableMover moveTrigger = lastHighlighted.GetComponent<InteractableMover>();
            if (moveTrigger != null)
            {
                moveTrigger.TriggerMovement();
                return; // Stop execution here so we don't pick it up
            }

            // --- Default Physics Grab Code ---
            grabbedObject = lastHighlighted;
            ResetHighlight();
            
            if (grabbedObject.GetComponent<Rigidbody>() != null)
            {
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            grabbedObject.transform.SetParent(holdPoint);
        }
        else if (grabbedObject != null)
        {
            // --- Default Physics Drop Code ---
            if (grabbedObject.GetComponent<Rigidbody>() != null)
            {
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
            holdPoint.localPosition = defaultHoldPosition;
        }
    }
}
}