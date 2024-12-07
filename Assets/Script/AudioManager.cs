using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private AudioPlaybackMonitor playbackMonitor;
    public List<GameObject> audioObjects;  // Each object has two AudioSources

    public List<AudioSource> audioSource1 = new List<AudioSource>();
    public List<AudioSource> audioSource2 = new List<AudioSource>();
    public List<int> currentState = new List<int>(new int[4]);
    //public List<int> stateChangeQueued = new List<int>();

    public int currentObjectIndex = 0;
    public bool musicStarted = false;
    public BlockManager BlockManager;
    public int numberOfStates = 3;
    public GameManager GM;
    public bool Restart = false;
    public bool blockActive = false;
    public double duration1;
    public double duration2;
    void Start()
    {
        playbackMonitor = GetComponent<AudioPlaybackMonitor>();
        numberOfStates = 3;
        // Initialize audio sources, states, and play times
        for (int i = 0; i < audioObjects.Count; i++)
        {
            AudioSource[] sources = audioObjects[i].GetComponents<AudioSource>();
            audioSource1.Add(sources[0]); // First audio source
            audioSource2.Add(sources[1]); // Second audio source
            // Calculate a Clip’s exact duration
            duration1 = (double)audioSource1[i].clip.samples / audioSource1[i].clip.frequency;
            duration2 = (double)audioSource2[i].clip.samples / audioSource2[i].clip.frequency;

            //Debug.Log(transform.parent.name + " duration1: " + duration1);
            //Debug.Log(transform.parent.name + " duration2: " + duration2);
            currentState.Add(0); // Initial state is muted first audio source
        }
    }
    void Update()
    {
        if (Restart)
        {
            Restart = false;
            Debug.Log(gameObject.transform.parent.gameObject.name + ": restartAll");
            ResetAudioManager(true);
            StartMusic();
        }
    }
    public void CheckDelay(double playTime)
    {
        
    }
    // This function initializes the AudioManager (instead of Start)
    public void InitializeMusic()
    {
        currentObjectIndex = 0;
        musicStarted = false;
    }

    public void StartMusic()
    {
        // Start looping through the game objects' audio in sequence
        musicStarted = true;
        scheduleAudio();
        PlayNextAudio(); // Start with the first audio object
    }
    public void scheduleAudio()
    {
        audioSource1[currentObjectIndex].PlayScheduled(AudioSettings.dspTime);
    }

    void PlayNextAudio()
    {
        if (musicStarted)
        {
            //ResetAllAudio();
            BlockManager.SetMaterialEmission(currentObjectIndex, 2.0f);

            switch (currentState[currentObjectIndex])
            {
                case 0: // Muted first audio source
                    audioSource1[currentObjectIndex].mute = true;
                    audioSource1[currentObjectIndex].PlayScheduled(AudioSettings.dspTime);
                    break;
                case 1: // Unmuted first audio source
                    audioSource1[currentObjectIndex].mute = false;
                    audioSource1[currentObjectIndex].PlayScheduled(AudioSettings.dspTime);
                    break;
                case 2: // Unmuted second audio source
                    audioSource2[currentObjectIndex].mute = false;
                    audioSource2[currentObjectIndex].PlayScheduled(AudioSettings.dspTime);
                    break;
            }
            StartCoroutine(NextObject(2.0f));
        }
    }

    private IEnumerator NextObject(float t)
    {
        yield return new WaitForSeconds(t);
        currentObjectIndex = (currentObjectIndex + 1) % audioObjects.Count;
        PlayNextAudio();
    }

    void ResetAllAudio()
    {
        foreach (var source in audioSource1)
        {
            source.Stop();
        }

        foreach (var source in audioSource2)
        {
            source.Stop();
        }

    }
    void FadeAllVolume()
    {
        foreach (var source in audioSource1)
        {
            GM.FadeOutAudio(source);
        }

        foreach (var source in audioSource2)
        {
            GM.FadeOutAudio(source);
        }
    }

    public void SwitchAudioState(int objectIndex)
    {
        if (objectIndex >= 0 && objectIndex < audioObjects.Count)
        {
            if (!audioSource1[objectIndex].isPlaying && !audioSource2[objectIndex].isPlaying)
            {
                ApplyStateChange(objectIndex);
                StartCoroutine(BlockManager.PressEffect(1.0f, objectIndex));
                //Debug.Log((currentState[objectIndex] + 1) % numberOfStates);
                BlockManager.ChangeState(objectIndex, currentState[objectIndex]);
                //stateChangeQueued[objectIndex] = 0;
            }
        }
    }

    void ApplyStateChange(int objectIndex)
    {
        currentState[objectIndex] = (currentState[objectIndex] + 1) % numberOfStates;
    }

    // Public reset function to reinitialize and restart music
    public void ResetAudioManager(bool enableManager)
    {
        if (!enableManager) FadeAllVolume();
        //currentState.Clear();
        // Cancel any scheduled invokes to prevent them from running
        //CancelInvoke();

        // Stop all coroutines if any are running
        StopAllCoroutines();
        //ResetAllAudio();
        // Reinitialize AudioManager if desired
        InitializeMusic();

        // Deactivate this script
        this.enabled = enableManager; // Disable the AudioManager component
    }
}