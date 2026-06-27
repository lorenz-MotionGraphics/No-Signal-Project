using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public GameObject tooltipObject;   // Panel
    public Text tooltipText;           // UI Text

    void Awake()
    {
        current = this;
        tooltipObject.SetActive(false);
    }

    public static void Show(string content)
    {
        if (current == null) return;

        current.tooltipText.text = content;
        current.tooltipObject.SetActive(true);
    }

    public static void Hide()
    {
        if (current == null) return;

        current.tooltipObject.SetActive(false);
    }
}