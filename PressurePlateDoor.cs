using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateDoor : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Settings")]
    public float openDistance = 2.5f;
    public float slidingSpeed = 2f;
    public string targetTag = "Interactable"; // Make sure your cube has this tag

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool isPlayerHoldingTarget = false;
    private bool doorShouldOpen = false;

    void Start()
    {
        // Record starting positions
        leftClosedPos = leftDoor.localPosition;
        rightClosedPos = rightDoor.localPosition;

        // Calculate open positions (moving on X axis)
        leftOpenPos = leftClosedPos + new Vector3(-openDistance, 0, 0);
        rightOpenPos = rightClosedPos + new Vector3(openDistance, 0, 0);
    }

    void Update()
    {
        // Smoothly move doors based on the state
        Vector3 targetLeft = doorShouldOpen ? leftOpenPos : leftClosedPos;
        Vector3 targetRight = doorShouldOpen ? rightOpenPos : rightClosedPos;

        leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, targetLeft, Time.deltaTime * slidingSpeed);
        rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, targetRight, Time.deltaTime * slidingSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            doorShouldOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            doorShouldOpen = false;
        }
    }
}