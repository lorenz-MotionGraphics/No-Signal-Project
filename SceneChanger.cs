using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading

public class SceneChanger : MonoBehaviour
{
    /// <summary>
    /// Reloads the scene that is currently active.
    /// </summary>
    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Loads a specific scene by its name. 
    /// You can type the scene name directly in the Button's Inspector.
    /// </summary>
    /// <param name="sceneName">The exact name of the scene to load.</param>
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneChanger: Scene name is empty!");
        }
    }
}