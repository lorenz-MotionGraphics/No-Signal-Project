using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("The exact X, Y, Z coordinates where the player should land.")]
    [SerializeField] private Vector3 targetCoordinates; 

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the Player
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Check if the player is using a CharacterController
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            // Disable CC temporarily so it doesn't override the manual position change
            cc.enabled = false;
            player.transform.position = targetCoordinates;
            cc.enabled = true;
        }
        else
        {
            // If just using standard Transform/Rigidbody
            player.transform.position = targetCoordinates;
        }
    }

    // Visualizes the destination in the Unity Editor scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(targetCoordinates, new Vector3(1, 2, 1)); // Draws a player-sized box at destination
    }
}