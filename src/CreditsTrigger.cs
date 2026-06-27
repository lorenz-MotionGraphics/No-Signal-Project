using UnityEngine;

public class CreditsTrigger : MonoBehaviour
{
    [SerializeField] private CreditsController creditsController;
    [SerializeField] private string playerTag = "Player";
    
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Prevent firing multiple times if the player bounces in/out of the trigger
        if (triggered) return;

        if (other.CompareTag(playerTag))
        {
            triggered = true;
            if (creditsController != null)
            {
                creditsController.StartCredits();
            }
        }
    }
}