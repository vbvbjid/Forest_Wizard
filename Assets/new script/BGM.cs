using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public int State = 0;
    public int numberOfStates = 2;
    public float[] ClipLengths;
    private int currentSegment = 0; // Tracks the current segment

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the AudioSource and clips are properly set up
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
            return;
        }

        if (clips == null || clips.Length == 0)
        {
            Debug.LogError("No audio clips assigned!");
            return;
        }

        // Set the initial clip based on the current State
        audioSource.clip = clips[State];

        // Play the audio if it's not already playing
        if (!audioSource.isPlaying)
        {
            Debug.Log("Starting BGM...");
            audioSource.Play();
        }

        // Initialize ClipLengths and calculate durations
        ClipLengths = new float[numberOfStates];
        for (int i = 0; i < numberOfStates; i++)
        {
            if (clips[i] != null)
            {
                ClipLengths[i] = (float)clips[i].samples / clips[i].frequency;
                Debug.Log("BGM " + i + ": " + ClipLengths[i]);
            }
            else
            {
                Debug.LogWarning($"Clip at index {i} is null.");
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    public float getPlayTime()
    {
        int timeSamples = audioSource.timeSamples;
        float clipFrequency = audioSource.clip.frequency;
        float preciseTime = timeSamples / (float)clipFrequency;
        Debug.Log($"Playback Time: {preciseTime} seconds (calculated from timeSamples)");
        return preciseTime;
    }
    public int GetSegmentAndResetCounter()
    {
        // Get the total play time and divide into four parts
        float totalDuration = audioSource.clip.length; // Total length of the clip
        float segmentDuration = totalDuration / 4;    // Duration of each segment
        float currentPlayTime = getPlayTime();       // Current playback time

        // Determine which segment the playback is currently in
        int newSegment = Mathf.FloorToInt(currentPlayTime / segmentDuration);

        // If the segment changes, reset a counter or perform another action
        if (newSegment != currentSegment)
        {
            Debug.Log($"Segment changed from {currentSegment} to {newSegment}");
            currentSegment = newSegment; // Update the current segment
        }

        return currentSegment;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hand"))
        {
            StartCoroutine(ChangeBGM(audioSource, 5.0f));
        }


    }
    public void click()
    {
        StartCoroutine(ChangeBGM(audioSource, 2.0f));
    }
    public IEnumerator ChangeBGM(AudioSource source, float duration)
    {
        source.volume = 1f; // Ensure the final volume is full
        audioSource.clip = clips[State];
        float startVolume = source.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        source.volume = 0f; // Ensure the volume is completely zero
        State = (State + 1) % numberOfStates;
        audioSource.clip = clips[State];
        audioSource.Play();

        startVolume = source.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        source.volume = 1;
    }
}
