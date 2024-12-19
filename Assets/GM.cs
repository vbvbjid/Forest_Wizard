using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GM : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject fadeScreen;
    public Camera targetCamera; // Assign the camera in the Inspector
    public CameraClearFlags backgroundType = CameraClearFlags.Skybox; // Default background type
    // Coroutine to gradually change the alpha of a material
    public void Bonttonswitch()
    {
        StartCoroutine(SwitchScene(1, 2, "op"));
    }
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "morning"){
            targetCamera.clearFlags = CameraClearFlags.Skybox;
            StartCoroutine(SwitchScene(0, 2, "morning"));
        }
        StartCoroutine(Guide());
    }
    public IEnumerator Guide(){
        yield return new WaitForSeconds(5.0f);
        audioSource.Play();
    }
    public IEnumerator SwitchScene(float targetAlpha, float duration, string sceneName)
    {
        if (fadeScreen == null)
        {
            Debug.LogWarning("Target object is null.");
            yield break;
        }

        Renderer renderer = fadeScreen.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("No Renderer found on the target object.");
            yield break;
        }

        Material material = renderer.material;
        if (!material.HasProperty("_Color"))
        {
            Debug.LogWarning("Material does not have a _Color property.");
            yield break;
        }

        Color currentColor = material.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            material.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        // Ensure the final alpha value is set
        material.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        if(sceneName != SceneManager.GetActiveScene().name){
            SceneManager.LoadScene(sceneName);
        }
        
    }
}
