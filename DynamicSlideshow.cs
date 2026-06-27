using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DynamicSlideshow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string folderName = "SlideshowImages";
    [SerializeField] private float timePerSlide = 3.0f;
    [SerializeField] private bool loop = true;

    private Image uiImage;
    private List<Sprite> slides = new List<Sprite>();
    private int currentSlideIndex = 0;
    private Coroutine slideshowCoroutine;

    void Start()
    {
        uiImage = GetComponent<Image>();
        LoadImagesFromFolder();

        if (slides.Count > 0)
        {
            slideshowCoroutine = StartCoroutine(PlaySlideshow());
        }
        else
        {
            Debug.LogWarning($"[Slideshow] No PNG images found in the target directory.");
        }
    }

    void LoadImagesFromFolder()
    {
        // Path points to Assets/StreamingAssets/SlideshowImages at edit time,
        // and safely resolves when the game is built.
        string targetFolder = Path.Combine(Application.streamingAssetsPath, folderName);

        if (!Directory.Exists(targetFolder))
        {
            Debug.LogError($"[Slideshow] Directory not found at: {targetFolder}. Creating it now.");
            Directory.CreateDirectory(targetFolder);
            return;
        }

        // Grab all PNG files
        string[] filePaths = Directory.GetFiles(targetFolder, "*.png");

        foreach (string path in filePaths)
        {
            byte[] fileData = File.ReadAllBytes(path);
            
            // Create a temporary texture (size will be overridden by LoadImage)
            Texture2D texture = new Texture2D(2, 2);
            
            if (texture.LoadImage(fileData))
            {
                // Convert Texture2D to Sprite for the UI Image component
                Sprite newSprite = Sprite.Create(
                    texture, 
                    new Rect(0, 0, texture.width, texture.height), 
                    new Vector2(0.5f, 0.5f)
                );
                
                slides.Add(newSprite);
            }
        }

        Debug.Log($"[Slideshow] Successfully loaded {slides.Count} slides.");
    }

    IEnumerator PlaySlideshow()
    {
        while (true)
        {
            // Assign current slide
            uiImage.sprite = slides[currentSlideIndex];

            yield return new WaitForSeconds(timePerSlide);

            // Move to next index
            currentSlideIndex++;

            if (currentSlideIndex >= slides.Count)
            {
                if (loop)
                {
                    currentSlideIndex = 0;
                }
                else
                {
                    break; // Stop if looping is disabled
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (slideshowCoroutine != null) StopCoroutine(slideshowCoroutine);
        
        // Clean up generated textures/sprites from memory
        foreach (var sprite in slides)
        {
            if (sprite != null)
            {
                Destroy(sprite.texture);
                Destroy(sprite);
            }
        }
    }
}