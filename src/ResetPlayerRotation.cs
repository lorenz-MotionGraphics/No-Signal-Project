using System.Collections;
using UnityEngine;

public class ResetPlayerRotation : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject loadingText;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player") && !triggered)
        {
            StartCoroutine(ResetOrientationRoutine(other.gameObject));
        }
    }

    private IEnumerator ResetOrientationRoutine(GameObject player)
    {
        triggered = true;

        // 1. Show loading text if available
        if (loadingText != null)
        {
            loadingText.SetActive(true);
        }

        // 2. Temporarily disable the Character Controller to allow hard physics overrides
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        // 3. Force the player's rotation values to absolute 0
        player.transform.rotation = Quaternion.identity; // Absolute (0, 0, 0)

        // 4. Clear any physics velocity/momentum using the older Unity API property
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Fixed the error here!
            rb.angularVelocity = Vector3.zero;
        }

        // Force Unity's physics engine to instantly accept these rotation changes
        Physics.SyncTransforms();

        // 5. Re-enable the Character Controller
        if (cc != null)
        {
            cc.enabled = true;
        }

        // 6. Give the engine a brief moment to catch up, then turn off UI
        yield return new WaitForSeconds(0.5f);

        if (loadingText != null)
        {
            loadingText.SetActive(false);
        }

        triggered = false;
    }
}