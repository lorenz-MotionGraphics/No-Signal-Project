using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string CaveEntrancePass; //CaveEntrancePass
    public float holdDuration = 1.5f;

    [Header("UI")]
    public GameObject indicatorUI;

    private bool isInside = false;
    private float holdTimer = 0f;

    void Start()
    {
        if (indicatorUI) indicatorUI.SetActive(false);
    }

    void Update()
    {
        if (isInside)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= holdDuration)
                {
                    SceneManager.LoadScene(CaveEntrancePass);
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            if (indicatorUI) indicatorUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
            holdTimer = 0f;
            if (indicatorUI) indicatorUI.SetActive(false);
        }
    }
}