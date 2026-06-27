using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OpenBrowserLink : MonoBehaviour
{
    public enum LinkType { WebURL, LocalHtmlFile }

    [Header("Link Configuration")]
    [SerializeField] private LinkType linkType = LinkType.WebURL;

    [Tooltip("For Web URL: Paste link (e.g., https://github.com).\nFor Local HTML: Just enter the filename (e.g., license.html).")]
    [SerializeField] private string targetPath;

    public void OpenURL()
    {
        if (string.IsNullOrEmpty(targetPath))
        {
            Debug.LogWarning($"[OpenBrowserLink] Target Path on {gameObject.name} is empty!");
            return;
        }

        string finalUrl = "";

        if (linkType == LinkType.WebURL)
        {
            // Handle standard web links
            finalUrl = targetPath.Trim();
            if (!finalUrl.StartsWith("http://") && !finalUrl.StartsWith("https://"))
            {
                finalUrl = "https://" + finalUrl;
            }
        }
        else if (linkType == LinkType.LocalHtmlFile)
        {
            // Handle local HTML files out of StreamingAssets
            // Path resolves to: Assets/StreamingAssets/yourfile.html
            string fullPath = Path.Combine(Application.streamingAssetsPath, targetPath.Trim());

            // Convert to a valid local URI string format (adds file:// automatically)
            System.Uri fileUri = new System.Uri(fullPath);
            finalUrl = fileUri.AbsoluteUri;
        }

        Debug.Log($"[OpenBrowserLink] Launching Browser with: {finalUrl}");
        Application.OpenURL(finalUrl);
    }
}