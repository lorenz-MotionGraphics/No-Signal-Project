using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOffsetMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform cubeTransform; // The cube you want to move
    [SerializeField] private float speed = 2f;         // Speed of the movement

    [Header("Movement Direction (Offsets)")]
    [Tooltip("Positive = Right, Negative = Left")]
    [SerializeField] private float moveX = 0f;
    
    [Tooltip("Positive = Up, Negative = Down")]
    [SerializeField] private float moveY = -5f; // Defaults to moving down
    
    [Tooltip("Positive = Forward, Negative = Backward")]
    [SerializeField] private float moveZ = 0f;

    private Vector3 targetPosition;
    private bool shouldMove = false;

    void Start()
    {
        if (cubeTransform != null)
        {
            // Calculate exactly where the cube should end up based on your offsets
            targetPosition = cubeTransform.position + new Vector3(moveX, moveY, moveZ);
        }
    }

    void Update()
    {
        if (shouldMove && cubeTransform != null)
        {
            // Move the cube smoothly toward that final calculated position
            cubeTransform.position = Vector3.MoveTowards(
                cubeTransform.position, 
                targetPosition, 
                speed * Time.deltaTime
            );

            // Stop moving once it arrives
            if (Vector3.Distance(cubeTransform.position, targetPosition) < 0.001f)
            {
                shouldMove = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldMove = true;
        }
    }
}