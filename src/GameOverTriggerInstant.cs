using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is based for instant game over lol obviuosly
public class GameOverTriggerInstant : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign in Inspector

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        triggered = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}