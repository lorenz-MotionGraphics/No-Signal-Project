using UnityEngine;

public class ForceShowCursor : MonoBehaviour
{
    void Start()
    {
        RevealCursor();
    }

    void Update()
    {
        // Optional: Keeps the cursor unlocked if other scripts try to hijack it
        if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
        {
            RevealCursor();
        }
    }

    void RevealCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}