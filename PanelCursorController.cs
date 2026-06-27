using UnityEngine;

public class PanelCursorController : MonoBehaviour
{
    private void OnEnable()
    {
        // When this panel becomes ACTIVE:
        // Free the cursor so it can move around, and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        // When this panel becomes INACTIVE:
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}