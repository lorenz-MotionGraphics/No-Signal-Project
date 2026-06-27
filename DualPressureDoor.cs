using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// If you are using TextMeshPro (recommended), uncomment the line below:
using TMPro; 

public class DualPressureDoor : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform topDoor;
    public Transform bottomDoor;

    [Header("Door Settings")]
    public float moveDownDistance = 4f;
    public float moveSpeed = 2f;
    public string targetTag = "Interactable";
    
    [Header("Requirement Settings")]
    [Tooltip("How many items need to be right-clicked to open the door")]
    public int requiredItemsCount = 2;

    [Header("UI / Indication")]
    [Tooltip("Attach a TextMeshPro or standard TextMesh component here")]
    public TextMeshPro textCounterDisplay; // Change to 'TextMesh' if using old Unity text

    private List<GameObject> itemsInZone = new List<GameObject>();
    private int collectedItemsCount = 0;
    private bool shouldOpen = false;

    private Vector3 topClosedPos;
    private Vector3 bottomClosedPos;
    private Vector3 topOpenPos;
    private Vector3 bottomOpenPos;

    void Start()
    {
        // Store original positions
        topClosedPos = topDoor.localPosition;   
        bottomClosedPos = bottomDoor.localPosition;

        // Top door slides UP, Bottom door slides DOWN
        topOpenPos = topClosedPos + new Vector3(0, moveDownDistance, 0); 
        bottomOpenPos = bottomClosedPos + new Vector3(0, -moveDownDistance, 0);

        // Initialize the display text at start
        UpdateVisualCounter();
    }

    void Update()
    {
        // 1 = Right Mouse Button
        if (Input.GetMouseButtonDown(1))
        {
            TryClickItemInZone();
        }

        // Smoothly move the doors
        Vector3 targetTop = shouldOpen ? topOpenPos : topClosedPos;
        Vector3 targetBottom = shouldOpen ? bottomOpenPos : bottomClosedPos;

        topDoor.localPosition = Vector3.MoveTowards(topDoor.localPosition, targetTop, moveSpeed * Time.deltaTime);
        bottomDoor.localPosition = Vector3.MoveTowards(bottomDoor.localPosition, targetBottom, moveSpeed * Time.deltaTime);
    }

    void TryClickItemInZone()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (itemsInZone.Contains(clickedObject))
            {
                itemsInZone.Remove(clickedObject);
                Destroy(clickedObject);
                
                collectedItemsCount++;
                
                // Update the floating text every time an item is successfully clicked
                UpdateVisualCounter();

                if (collectedItemsCount >= requiredItemsCount)
                {
                    shouldOpen = true;
                }
            }
        }
    }

    // Handles changing the text in the world
    void UpdateVisualCounter()
    {
        if (textCounterDisplay != null)
        {
            if (collectedItemsCount >= requiredItemsCount)
            {
                textCounterDisplay.text = "Ok";
                textCounterDisplay.color = Color.green; // Optional: change text color to green
            }
            else
            {
                textCounterDisplay.text = $"{collectedItemsCount} / {requiredItemsCount}";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !itemsInZone.Contains(other.gameObject))
        {
            itemsInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (itemsInZone.Contains(other.gameObject))
        {
            itemsInZone.Remove(other.gameObject);
        }
    }
}