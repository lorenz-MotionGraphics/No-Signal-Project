using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPuzzleController : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorTransform;
    public float lowerDistance = 3f;
    public float lowerSpeed = 2f;

    [Header("Puzzle Detection")]
    public string artifactTag = "Interactable"; // Matches your artifact's tag
    public int requiredArtifactCount = 4;

    // A HashSet keeps a unique list of objects so the same artifact can't be counted twice
    private HashSet<Collider> artifactsInTrigger = new HashSet<Collider>();
    
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    private bool puzzleSolved = false;

    void Start()
    {
        if (doorTransform != null)
        {
            doorClosedPos = doorTransform.localPosition;
            // Calculate downwards position on the Y axis
            doorOpenPos = doorClosedPos + new Vector3(0, -lowerDistance, 0);
        }
    }

    void Update()
    {
        // Smoothly slide the door down if solved, or stay up if not
        Vector3 targetPos = puzzleSolved ? doorOpenPos : doorClosedPos;
        
        if (doorTransform != null)
        {
            doorTransform.localPosition = Vector3.Lerp(doorTransform.localPosition, targetPos, Time.deltaTime * lowerSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering has the correct tag
        if (other.CompareTag(artifactTag))
        {
            // Add it to our unique list
            if (!artifactsInTrigger.Contains(other))
            {
                artifactsInTrigger.Add(other);
                CheckPuzzleState();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If an artifact is taken out of the zone, remove it from the list
        if (other.CompareTag(artifactTag))
        {
            if (artifactsInTrigger.Contains(other))
            {
                artifactsInTrigger.Remove(other);
                CheckPuzzleState();
            }
        }
    }

    private void CheckPuzzleState()
    {
        Debug.Log($"Artifacts in puzzle zone: {artifactsInTrigger.Count} / {requiredArtifactCount}");

        // If all 4 unique artifacts are resting inside the box
        if (artifactsInTrigger.Count >= requiredArtifactCount)
        {
            puzzleSolved = true;
            Debug.Log("Puzzle Solved! Lowering the door.");
        }
        else
        {
            // Optional: Re-close the door if the player pulls an item back out
            puzzleSolved = false; 
        }
    }
}