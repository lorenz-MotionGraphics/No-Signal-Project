using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeGame()
    {
        // This runs automatically the absolute second you hit "Play", no matter what scene you are on!
        if (instance == null)
        {
            GameObject go = new GameObject("GameManager");
            instance = go.AddComponent<GameManager>();
            DontDestroyOnLoad(go);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");

        if (existingPlayer == null)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Player");

            if (playerPrefab != null)
            {
                SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
                
                Vector3 spawnPos = Vector3.zero;
                Quaternion spawnRot = Quaternion.identity;

                if (spawnPoint != null)
                {
                    spawnPos = spawnPoint.transform.position;
                    spawnRot = spawnPoint.transform.rotation;
                }

                GameObject newPlayer = Instantiate(playerPrefab, spawnPos, spawnRot);
                DontDestroyOnLoad(newPlayer);

                // --- CAMERA ROTATION FIX ---
                // Find the main camera inside the newly spawned player and force its rotation straight
                Camera playerCam = newPlayer.GetComponentInChildren<Camera>();
                if (playerCam != null)
                {
                    playerCam.transform.localRotation = Quaternion.identity; 
                }
            }
            else
            {
                Debug.LogError("Could not find a Prefab named 'Player' inside an Assets/Resources folder!");
            }
        }
    }
}
*/