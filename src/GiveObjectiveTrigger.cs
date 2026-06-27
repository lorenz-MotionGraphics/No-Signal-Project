using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveObjectiveTrigger : MonoBehaviour
{
    [TextArea] [SerializeField] private string objectiveMessage = "Find a way out of the caverns.";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectiveManager.Instance.DisplayObjective(objectiveMessage);
            
            Destroy(gameObject); 
        }
    }
}