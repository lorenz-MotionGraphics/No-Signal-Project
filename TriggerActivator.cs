using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public HiddenTrigger hiddenTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hiddenTrigger.ShowTrigger();
        }
    }
}