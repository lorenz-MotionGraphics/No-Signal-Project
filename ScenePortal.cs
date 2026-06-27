using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class ScenePortal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Type the exact name of your next scene here

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the cube is the Player
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}