using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign in Inspector
    public float delay = 3f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(ShowGameOver());
        }
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(delay);

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}