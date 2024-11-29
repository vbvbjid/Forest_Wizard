using UnityEngine;
using System.Collections.Generic;

public class buffer_AM : MonoBehaviour
{
    // List of game objects, each containing two audio sources
    public List<GameObject> audioObjects;  // Each object has two AudioSources

    // Store audio sources of all game objects
    private List<AudioSource> audioSource1 = new List<AudioSource>();
    private List<AudioSource> audioSource2 = new List<AudioSource>();

    // Store the current state of each game object
    private List<int> currentState = new List<int>();

    // Track if a state change is queued for each object
    private List<bool> stateChangeQueued = new List<bool>();

    private int currentObjectIndex = 0; // Tracks which object is playing
    private bool musicStarted = false;
    public BlockManager BlockManager;
    public int numberOfStates = 3;

    void Start()
    {
        numberOfStates = 3;
        // Initialize audio sources, states, and play times
        for (int i = 0; i < audioObjects.Count; i++)
        {
            AudioSource[] sources = audioObjects[i].GetComponents<AudioSource>();
            audioSource1.Add(sources[0]); // First audio source
            audioSource2.Add(sources[1]); // Second audio source

            currentState.Add(2); // Initial state is muted first audio source
            stateChangeQueued.Add(false); // No state change queued initially
        }

        StartMusic(); // Begin music sequence
    }

    void StartMusic()
    {
        // Start looping through the game objects' audio in sequence
        musicStarted = true;
        PlayNextAudio(); // Start with the first audio object
    }

    void PlayNextAudio()
    {
        if (musicStarted)
        {
            // Reset all audio sources to avoid overlap
            ResetAllAudio();
            BlockManager.SetMaterialEmission(currentObjectIndex, 2.0f);            
            // Play the current audio based on the current state
            switch (currentState[currentObjectIndex])
            {
                case 0: // Unmuted first audio source
                    
                    audioSource1[currentObjectIndex].mute = false;
                    audioSource1[currentObjectIndex].Play();
                    break;
                case 1: // Unmuted second audio source
                    
                    audioSource2[currentObjectIndex].mute = false;
                    audioSource2[currentObjectIndex].Play();
                    break;
                case 2: // Muted first audio source
                    
                    audioSource1[currentObjectIndex].mute = true;
                    audioSource1[currentObjectIndex].Play();

                    break;
            }

            // Check if a state change was queued and apply it after the current sequence
            if (stateChangeQueued[currentObjectIndex])
            {
                stateChangeQueued[currentObjectIndex] = false; // Reset the queue flag
                Invoke("ApplyQueuedStateChange", 2f); // Apply state change after 4 seconds
                //Debug.Log("");
            }
            else
            {
                // Move to the next object after 4 seconds
                Invoke("NextObject", 2f);
            }
        }
    }

    void NextObject()
    {
        // Move to the next object in the sequence
        currentObjectIndex = (currentObjectIndex + 1) % audioObjects.Count;

        // Play the next audio
        PlayNextAudio();
    }

    void ResetAllAudio()
    {
        // Stop all audio to ensure only the current audio is playing
        foreach (var source in audioSource1)
            source.Stop();
        foreach (var source in audioSource2)
            source.Stop();
    }

    public void SwitchAudioState(int objectIndex)
    {
        // Ensure index is valid
        if (objectIndex >= 0 && objectIndex < audioObjects.Count)
        {
            BlockManager.ChangeState(objectIndex, (currentState[objectIndex] + 1) % numberOfStates);
            StartCoroutine(BlockManager.PressEffect(1.0f, objectIndex));
            // If the current sequence is playing, queue the state change
            if (audioSource1[objectIndex].isPlaying || audioSource2[objectIndex].isPlaying)
            {
                stateChangeQueued[objectIndex] = true; // Queue state change
            }
            else
            {
                // If the sequence is not playing, switch state immediately
                ApplyStateChange(objectIndex);
            }
        }
    }

    void ApplyQueuedStateChange()
    {
        // Apply the queued state change for the current object
        ApplyStateChange(currentObjectIndex);

        // Move to the next object in the sequence
        NextObject();
    }

    void ApplyStateChange(int objectIndex)
    {
        // Move to the next stat
        currentState[objectIndex] = (currentState[objectIndex] + 1) % numberOfStates;
        // Apply the new state immediately (no need to track play time anymore)
        /*switch (currentState[objectIndex])
        {
            case 0: // Muted first audio source
                audioSource1[objectIndex].mute = false;
                audioSource1[objectIndex].Play();
                break;
            case 1: // Unmuted first audio source
                audioSource2[objectIndex].mute = false;
                audioSource2[objectIndex].Play();
                break;
            /*case 2: // Unmuted second audio source
                audioSource1[objectIndex].mute = true;
                audioSource1[objectIndex].Play();
                break;
        }*/
    }
}