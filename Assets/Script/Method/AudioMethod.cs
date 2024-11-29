using System.Collections;
using UnityEngine;

public class AudioMethod : MonoBehaviour
{
    // Singleton Instance
    public static AudioMethod Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Optional: Keep this object between scenes
        DontDestroyOnLoad(gameObject);
    }
    public void AdjustOtherAudioVolumes(AudioSource target)
    {
        // Get all audio sources in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            //DebugLogToCanvasTMP.Instance.UpdateLog(audioSource.ToString());
            audioSource.volume = 0.1f;
            //DebugLogToCanvasTMP.Instance.UpdateLog(audioSource.volume.ToString());
        }
    }
    // Restore volumes
    public IEnumerator RestoreAllVolumes(float duration)
    {
        // Get all AudioSources in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        // Store the initial volumes of all AudioSources
        float[] initialVolumes = new float[allAudioSources.Length];
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            initialVolumes[i] = allAudioSources[i].volume; // Store current volume
        }

        // Gradually restore volumes
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Calculate interpolation factor (0 to 1)

            // Set each AudioSource volume based on the interpolation
            for (int i = 0; i < allAudioSources.Length; i++)
            {
                allAudioSources[i].volume = Mathf.Lerp(initialVolumes[i], 1.0f, t); // Interpolate to target volume
            }

            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }

        // Ensure the final volume is set to the desired level
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = 1.0f; // Set to final target volume
        }
    }
    public void PlayAudio(AudioSource audioSource)
    {
        //Stop to start form the beginning
        if (audioSource.isPlaying)
            audioSource.Stop();
        //lower the volume of other audio
        AdjustOtherAudioVolumes(audioSource);
        if (audioSource != null)
        {
            audioSource.volume = 1.0f;
            audioSource.Play();
        }
        //Gradually restore other audio
        StartCoroutine(RestoreAllVolumes(audioSource.clip.length));
    }
}
