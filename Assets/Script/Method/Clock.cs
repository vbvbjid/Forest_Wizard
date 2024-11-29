using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public AudioSource audioSource; // Attach your audio source in the inspector
    public List<AudioManager> AudioManagers; // Attach your audio managers in the inspector

    private double lastTriggeredTime = 0.0f; // Last time an event was triggered
    private float interval = 2f; // Interval for triggering events (2 seconds)
    private float threshold = 0.05f; // Small threshold for precision (50ms)

    private double previousPlayTime = 0.0f; // To detect loop reset

    void Update()
    {
        if (audioSource.isPlaying)
        {
            double currentPlayTime = audioSource.time;

            // Detect if the audio has looped
            if (currentPlayTime < previousPlayTime)
            {
                // Audio has looped back to the beginning
                Debug.Log("Audio has looped back to the beginning.");
                lastTriggeredTime = 0.0f; // Reset the triggered time
            }

            // Check if it's time to trigger the event (considering a small threshold for precision)
            if (currentPlayTime >= lastTriggeredTime + interval - threshold)
            {
                // Trigger the event for each AudioManager
                Debug.Log("Triggered at time: " + currentPlayTime);
                foreach (var audioManager in AudioManagers)
                {
                    audioManager.CheckDelay(currentPlayTime);
                }

                // Update last triggered time to the next interval
                lastTriggeredTime += interval;

                // Ensure we don't miss multiple intervals due to frame skips
                while (currentPlayTime >= lastTriggeredTime + interval - threshold)
                {
                    lastTriggeredTime += interval;
                }
            }

            // Update the previous play time
            previousPlayTime = currentPlayTime;
        }
    }
}
