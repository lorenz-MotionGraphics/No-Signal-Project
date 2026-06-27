using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
using UnityEngine.UI;

public class RegisterPauseMenu : MonoBehaviour
{
    // Changed from Awake to Start to give the GameManager time to spawn the Player first!
    void Start()
    {
        // 1. Find the dynamically spawned player in the scene
        SimplePlayer player = FindObjectOfType<SimplePlayer>();
        
        if (player != null)
        {
            // 2. Link this panel to the player's empty slot
            player.pauseMenuUI = this.gameObject;
            
            // 3. Configure the buttons
            Button resumeBtn = transform.Find("ResumeButton")?.GetComponent<Button>();
            Button quitBtn = transform.Find("QuitButton")?.GetComponent<Button>();

            if (resumeBtn != null)
            {
                resumeBtn.onClick.RemoveAllListeners();
                resumeBtn.onClick.AddListener(player.Resume);
            }
            else
            {
                Debug.LogError("RegisterPauseMenu: Missing a child object named 'ResumeButton' with a Button component!");
            }

            if (quitBtn != null)
            {
                quitBtn.onClick.RemoveAllListeners();
                quitBtn.onClick.AddListener(player.QuitGame);
            }
            else
            {
                Debug.LogError("RegisterPauseMenu: Missing a child object named 'QuitButton' with a Button component!");
            }

            // 4. Shut the panel off safely until the player hits Escape
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("RegisterPauseMenu: Could not find SimplePlayer in the scene to register buttons!");
        }
    }
}
*/