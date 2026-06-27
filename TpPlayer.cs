using System.Collections;
using UnityEngine;

public class TpPlayer : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject loadingText; 

    [Header("Teleport Settings")]
    public Transform targetAnchor; 
    public float waitTime = 1; // How long the loading screen stays visible

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            StartCoroutine(TriggerTeleportRoutine(other.gameObject));
        }
    }

    private IEnumerator TriggerTeleportRoutine(GameObject player)
    {
        triggered = true;

        // 1. Show the loading text
        if (loadingText != null)
        {
            loadingText.SetActive(true);
        }

        // 2. Disable Character Controller & Teleport
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        if (targetAnchor != null)
        {
            player.transform.position = targetAnchor.position;
            player.transform.rotation = targetAnchor.rotation;
            
            // Forces Unity to immediately update the physics position
            Physics.SyncTransforms(); 
        }
        else
        {
            Debug.LogWarning("Target Anchor is missing from the script Inspector!");
        }

        // 3. Re-enable Character Controller
        if (cc != null)
        {
            cc.enabled = true;
        }

        // 4. Wait for a moment so the player actually sees the loading screen
        yield return new WaitForSeconds(waitTime);

        // 5. Hide the loading text
        if (loadingText != null)
        {
            loadingText.SetActive(false);
        }

        // Reset trigger
        triggered = false;
    }
}