using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TriplePressureDoor : MonoBehaviour
{
    [Header("Objects to Disappear")]
    [Tooltip("Drag the doors or meshes that should disappear/deactivate when the puzzle is solved.")]
    public GameObject[] objectsToRemove;

    [Header("Settings")]
    public string itemTag = "Interactable";
    
    [Tooltip("How many items need to be right-clicked to solve the puzzle")]
    public int requiredItemsCount = 7;

    [Header("UI / Indication")]
    public TextMeshPro textCounterDisplay;

    private int collectedItemsCount = 0;
    private bool puzzleSolved = false;
    private bool playerIsInsideZone = false;

    void Start()
    {
        UpdateVisualCounter();
    }

    void Update()
    {
        // STRICT ISOLATION: Right-click logic is completely dead if the player is outside the box
        if (!playerIsInsideZone || puzzleSolved) return;

        if (Input.GetMouseButtonDown(1)) // 1 = Right Mouse Button
        {
            TryClickItemInZone();
        }
    }

    void TryClickItemInZone()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Shoot a raycast that SPECIFICALLY targets the "Interactable" layer
        int layerMask = LayerMask.GetMask("Interactable");

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            GameObject clickedObject = hit.collider.gameObject;

            // Double check tag security
            if (clickedObject.CompareTag(itemTag))
            {
                // Verify the item is physically inside this specific trigger box's boundaries
                // This prevents clicking items belonging to other doors across the map
                if (GetComponent<Collider>().bounds.Contains(hit.point))
                {
                    // Remove the artifact
                    Destroy(clickedObject);
                    
                    collectedItemsCount++;
                    UpdateVisualCounter();

                    if (collectedItemsCount >= requiredItemsCount)
                    {
                        SolvePuzzle();
                    }
                }
                else
                {
                    Debug.Log("Clicked an artifact, but it belongs to a different zone!");
                }
            }
        }
    }

    void SolvePuzzle()
    {
        puzzleSolved = true;

        // Loop through the objectsToRemove array and turn them off completely
        foreach (GameObject obj in objectsToRemove)
        {
            if (obj != null)
            {
                obj.SetActive(false); 
            }
        }
        
        Debug.Log(gameObject.name + " Solved! Registered items removed.");
    }

    void UpdateVisualCounter()
    {
        if (textCounterDisplay != null)
        {
            if (collectedItemsCount >= requiredItemsCount)
            {
                textCounterDisplay.text = "Ok";
                textCounterDisplay.color = Color.green;
            }
            else
            {
                textCounterDisplay.text = $"{collectedItemsCount} / {requiredItemsCount}";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only accept the Player entering this box
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