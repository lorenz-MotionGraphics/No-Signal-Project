using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveChainTrigger : MonoBehaviour
{
    [Header("Current Step")]
    [TextArea] [SerializeField] private string objectiveMessage = "Move your mouse to look";
    
    [Header("Next Step Configuration")]
    [Tooltip("If checked, walking into this will also clear the old objective completely.")]
    [SerializeField] private bool isFinishingAnObjective = false;
    
    [Tooltip("Leave empty if this is the final step. Drag the next objective object here if there is a next step.")]
    [SerializeField] private GameObject nextObjectiveTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Handle finishing the previous objective if necessary
            if (isFinishingAnObjective)
            {
                ObjectiveManager.Instance.CompleteObjective();
            }

            // 2. Display the new objective text (if provided)
            if (!string.IsNullOrEmpty(objectiveMessage))
            {
                ObjectiveManager.Instance.DisplayObjective(objectiveMessage);
            }

            // 3. Turn on the next objective trigger in line so it's ready for the player
            if (nextObjectiveTrigger != null)
            {
                nextObjectiveTrigger.SetActive(true);
            }

            // 4. Self-destruct this step so it can't be triggered again
            Destroy(gameObject);
        }
    }
}