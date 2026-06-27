/*
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public string targetTag = "Interactable";
    public float interactDistance = 10f;
    
    // Drag and drop your door object here in the Inspector
    public PressurePlateDoor doorScript; 

    void Update()
    {
        // 1 = Right Mouse Button
        if (Input.GetMouseButtonDown(1)) 
        {
            PerformInteraction();
        }
    }

    void PerformInteraction()
    {
        // Shoot a raycast from the mouse position into the 3D world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Check if the object we right-clicked has the correct tag
            if (hit.collider.CompareTag(targetTag))
            {
                // 1. Make the object disappear
                Destroy(hit.collider.gameObject); 
                
                // 2. Tell the door to open
                if (doorScript != null)
                {
                    doorScript.InteractToOpen();
                }
                else
                {
                    Debug.LogWarning("Door Script is not assigned in the PlayerInteraction component!");
                }
            }
        }
    }
}
*/