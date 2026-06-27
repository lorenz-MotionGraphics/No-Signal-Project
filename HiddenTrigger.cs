using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTrigger : MonoBehaviour
{
    private Renderer rend;
    private Collider col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        // Hide the cube
        if (rend != null)
            rend.enabled = false;

        // Disable triggering
        if (col != null)
            col.enabled = false;
    }

    public void ShowTrigger()
    {
        if (rend != null)
            rend.enabled = true;

        if (col != null)
            col.enabled = true;
    }
}