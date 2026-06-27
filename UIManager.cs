using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject aboutPanel;
    public GameObject mainMenu;
    public GameObject AchievementsPanel;
    public GameObject Guide;
    public void ShowAbout()
    {
        mainMenu.SetActive(false);
        AchievementsPanel.SetActive(false);
        aboutPanel.SetActive(true);
        Guide.SetActive(false);
    }

    public void HideAbout()
    {
        aboutPanel.SetActive(false);
        mainMenu.SetActive(true);
        AchievementsPanel.SetActive(false);
        Guide.SetActive(false);
    }

    public void ShowAchievements()
    {
        aboutPanel.SetActive(false);
        mainMenu.SetActive(false);
        AchievementsPanel.SetActive(true);
        Guide.SetActive(false);
    }

    public void HideAchievements()
    {
        aboutPanel.SetActive(false);
        mainMenu.SetActive(true);
        AchievementsPanel.SetActive(false);
        Guide.SetActive(false);
    }

    // GUIDE PANEL
    public void ShowGuide()
    {
        aboutPanel.SetActive(false);
        mainMenu.SetActive(false);
        AchievementsPanel.SetActive(false);
        Guide.SetActive(true);
    }

    public void HideGuide()
    {
        aboutPanel.SetActive(false);
        mainMenu.SetActive(true);
        AchievementsPanel.SetActive(false);
        Guide.SetActive(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

        Debug.Log("Game is exiting...");
    }
}