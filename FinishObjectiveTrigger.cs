using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObjectiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // This fades the text completely off the screen
            ObjectiveManager.Instance.CompleteObjective();
            
            Destroy(gameObject);
        }
    }
}