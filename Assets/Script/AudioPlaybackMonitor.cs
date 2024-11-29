using UnityEngine;

public class AudioPlaybackMonitor : MonoBehaviour
{
    public AudioSource[] audioSources;  // Array of AudioSources to monitor
    public float syncThreshold = 0.05f;  // Time difference threshold (in seconds) to consider audios as desynced

    void Start()
    {
        // Fetch all AudioSources in the scene
        //audioSources = FindObjectsOfType<AudioSource>();
    }
    
    void Update()
    {
        if (audioSources == null || audioSources.Length == 0)
        {
            Debug.LogWarning("No AudioSources assigned for monitoring.");
            return;
        }

        // Set the reference time from the first audio source in the list
        float referenceTime = audioSources[0].time;
        
        // Monitor and check the playback time of each audio source
        for (int i = 1; i < audioSources.Length; i++)
        {
            float currentTime = audioSources[i].time;
            float timeDifference = Mathf.Abs(currentTime - referenceTime);

            // Log if the audios are out of sync beyond the threshold
            if (timeDifference > syncThreshold)
            {
                Debug.LogWarning($"AudioSource {i} is out of sync by {timeDifference} seconds.");
            }
        }
    }
} 
