using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantSceneTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Type the exact name of the scene you want to load.")]
    [SerializeField] private string sceneToLoad;

    [Header("Script-Based UI Settings")]
    [Tooltip("The text that will display on screen during the transition.")]
    [SerializeField] private string loadingText = "Loading next zone... Please wait.";
    
    [Tooltip("How long to wait (in seconds) before the scene changes.")]
    [SerializeField] private float transitionDelay = 2.5f;

    [Tooltip("Size of the text font.")]
    [SerializeField] private int fontSize = 30;

    private bool showText = false;
    private bool isTransitioning = false;
    private Texture2D backgroundTex;

    private void Start()
    {
        // Pre-create the background texture so we aren't generating it every frame in OnGUI
        backgroundTex = new Texture2D(1, 1);
        backgroundTex.SetPixel(0, 0, new Color(0, 0, 0, 0.6f)); // 0.6f is the dark overlay opacity
        backgroundTex.Apply();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;

        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                StartCoroutine(TransitionSequence());
            }
            else
            {
                Debug.LogWarning("InstantSceneTrigger: No scene name specified in the inspector!");
            }
        }
    }

    private IEnumerator TransitionSequence()
    {
        isTransitioning = true;
        showText = true; 

        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnGUI()
    {
        if (showText)
        {
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontSize = fontSize;
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.normal.textColor = Color.white; 
            textStyle.alignment = TextAnchor.MiddleCenter;

            // FIXED: The texture must be assigned to textStyle.normal.background, not textStyle.background
            if (backgroundTex != null)
            {
                textStyle.normal.background = backgroundTex;
            }

            // Draw the fullscreen layout and text
            Rect boxRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.Label(boxRect, loadingText, textStyle);
        }
    }
}