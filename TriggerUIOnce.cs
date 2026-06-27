using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUIOnce : MonoBehaviour
{
    public GameObject canvasUI;
    public float displayTime = 3f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            canvasUI.SetActive(true);
            hasTriggered = true;
            Invoke(nameof(HideUI), displayTime);
        }
    }

    void HideUI()
    {
        canvasUI.SetActive(false);
    }
}