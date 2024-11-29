using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraFadeController : MonoBehaviour
{
    public float fadeDuration = 2f; // Duration of the fade-in effect
    public float delayBeforeFade = 1f; // Time to wait before starting the fade

    private CanvasGroup fadeCanvasGroup; // Canvas group to control the fade effect

    void Start()
    {
        // Create and initialize the fade canvas
        CreateFadeCanvas();

        // Start with the screen completely black
        fadeCanvasGroup.alpha = 1f;

        // Start the fade-in process after the delay
        Invoke(nameof(StartFadeIn), delayBeforeFade);
    }

    private void CreateFadeCanvas()
    {
        // Create a full-screen UI canvas
        GameObject canvasObject = new GameObject("FadeCanvas");
        canvasObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        // Add a CanvasGroup to control alpha
        fadeCanvasGroup = canvasObject.AddComponent<CanvasGroup>();

        // Create a black image to cover the screen
        GameObject blackImageObject = new GameObject("BlackImage");
        blackImageObject.transform.SetParent(canvasObject.transform, false);
        var image = blackImageObject.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;

        // Stretch the black image to fill the screen
        RectTransform rectTransform = blackImageObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - (elapsedTime / fadeDuration); // Reduce alpha over time
            yield return null;
        }

        // Ensure the alpha is set to 0 at the end
        fadeCanvasGroup.alpha = 0f;

        // Optionally, destroy the fade canvas after the fade-in completes
        Destroy(fadeCanvasGroup.gameObject);
    }
}