using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIModified : MonoBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private GameObject player;      // Drag your Player GameObject here
    [SerializeField] private Transform respawnAnchor; // Drag your Empty GameObject (Spawn Point) here

    public void RespawnPlayer()
    {
        // 1. Reset time in case the game was paused on Game Over
        Time.timeScale = 1f; 

        // 2. Lock and hide the cursor for 3D gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 3. Check if both references are assigned
        if (player != null && respawnAnchor != null)
        {
            // If your player uses a CharacterController, it must be temporarily disabled to teleport
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // 4. Teleport the player to the anchor's position and rotation
            player.transform.position = respawnAnchor.position;
            player.transform.rotation = respawnAnchor.rotation; // Matches the direction the anchor faces

            // Re-enable CharacterController if it exists
            if (cc != null) cc.enabled = true;

            // 5. Hide the Game Over UI screen
            gameObject.SetActive(false); 
        }
        else
        {
            Debug.LogError("Please assign both the Player and Respawn Anchor in the Inspector!");
        }
    }
}