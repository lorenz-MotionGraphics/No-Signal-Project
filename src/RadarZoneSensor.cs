using UnityEngine;

public class RadarZoneSensor : MonoBehaviour
{
    private RadioRadar mainRadarController;
    private Collider myCollider;

    void Start()
    {
        // Finds the main radar system on the parent hierarchy automatically
        mainRadarController = GetComponentInParent<RadioRadar>();
        myCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mainRadarController != null)
        {
            mainRadarController.ObjectEnteredZone(myCollider, other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (mainRadarController != null)
        {
            mainRadarController.ObjectExitedZone(myCollider, other);
        }
    }
}