using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }

    private string activeSubtitle = "";
    private float subtitleTimer = 0f;

    private void Awake()
    {
        // Simple singleton pattern to easily call this from anywhere
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisplaySubtitle(string text, float duration)
    {
        if (!string.IsNullOrEmpty(text))
        {
            activeSubtitle = text;
            subtitleTimer = duration;
        }
    }

    private void Update()
    {
        if (subtitleTimer > 0f)
        {
            subtitleTimer -= Time.deltaTime;

            if (subtitleTimer <= 0f)
            {
                activeSubtitle = "";
            }
        }
    }

    private void OnGUI()
    {
        if (string.IsNullOrEmpty(activeSubtitle))
            return;

        GUIStyle subtitleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 24
        };
        subtitleStyle.normal.textColor = Color.white;

        GUIStyle shadowStyle = new GUIStyle(subtitleStyle);
        shadowStyle.normal.textColor = Color.black;

        Rect rect = new Rect(0, Screen.height - 120, Screen.width, 50);

        // Draw shadow
        GUI.Label(
            new Rect(rect.x + 2, rect.y + 2, rect.width, rect.height),
            activeSubtitle,
            shadowStyle
        );

        // Draw main text
        GUI.Label(rect, activeSubtitle, subtitleStyle);
    }
}