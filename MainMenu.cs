using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject[] introPanels; // Assign your panels here in order
    public string sceneToLoad = "Outside";
    private int currentPanelIndex = 0;

    public void StartGameSequence()
    {
        if (introPanels.Length > 0)
        {
            ShowPanel(0);
        }
        else
        {
            LoadNextScene();
        }
    }

    public void ShowNextPanel()
    {
        currentPanelIndex++;

        if (currentPanelIndex < introPanels.Length)
        {
            ShowPanel(currentPanelIndex);
        }
        else
        {
            LoadNextScene();
        }
    }

    private void ShowPanel(int index)
    {
        foreach (GameObject panel in introPanels)
        {
            panel.SetActive(false);
        }
        introPanels[index].SetActive(true);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}