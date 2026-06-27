using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theLargeTempleDoor : MonoBehaviour
{
    [Header("Door Target")]
    public Transform doorTransform;

    [Header("Settings")]
    public float movementSpeed = 3f;
    public string targetTag = "Interactable"; 

    [Header("Closed States (Static Default)")]
    public float closedPosX = -18.437f;
    public float closedRotZ = -1.5258f;

    [Header("Open States")]
    public float openPosX = -16.51f;
    public float openRotZ = -30.17f;

    // Hardcoded cached constants from your inspector inspector layout
    private float defaultPosY = -0.4124f;
    private float defaultPosZ = -127.48f;
    private float defaultRotX = -90.50f;
    private float defaultRotY = 0f;

    private Vector3 closedPosition;
    private Quaternion closedRotation;
    private Vector3 openPosition;
    private Quaternion openRotation;

    private bool doorShouldOpen = false;

    void Start()
    {
        if (doorTransform == null)
        {
            doorTransform = this.transform;
        }

        // Pre-calculate strict, static vectors for both states
        closedPosition = new Vector3(closedPosX, defaultPosY, defaultPosZ);
        closedRotation = Quaternion.Euler(defaultRotX, defaultRotY, closedRotZ);

        openPosition = new Vector3(openPosX, defaultPosY, defaultPosZ);
        openRotation = Quaternion.Euler(defaultRotX, defaultRotY, openRotZ);

        // Force snap to absolute resting position immediately at start
        doorTransform.localPosition = closedPosition;
        doorTransform.localRotation = closedRotation;
    }

    void Update()
    {
        if (doorShouldOpen)
        {
            // Smoothly swing and slide open
            doorTransform.localPosition = Vector3.Lerp(doorTransform.localPosition, openPosition, Time.deltaTime * movementSpeed);
            doorTransform.localRotation = Quaternion.Slerp(doorTransform.localRotation, openRotation, Time.deltaTime * movementSpeed);
        }
        else
        {
            // If it's already basically closed, snap it to perfect static coordinates and stop calculating
            if (Vector3.Distance(doorTransform.localPosition, closedPosition) < 0.01f && 
                Quaternion.Angle(doorTransform.localRotation, closedRotation) < 0.1f)
            {
                doorTransform.localPosition = closedPosition;
                doorTransform.localRotation = closedRotation;
            }
            else
            {
                // Smoothly return to close if it isn't there yet
                doorTransform.localPosition = Vector3.Lerp(doorTransform.localPosition, closedPosition, Time.deltaTime * movementSpeed);
                doorTransform.localRotation = Quaternion.Slerp(doorTransform.localRotation, closedRotation, Time.deltaTime * movementSpeed);
            }
        }
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