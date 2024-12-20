using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Meta.Voice.Audio;
[System.Serializable]
public class AudioClipArray
{
    public AudioClip[] clips = new AudioClip[2];
}
public class newAM : MonoBehaviour
{
    public bool musicStart = false;
    public List<int> currentState = new List<int>(new int[4]);
    [SerializeField]
    public AudioClipArray[] audioClipGrid = new AudioClipArray[4];
    public List<AudioSource> audioSource = new List<AudioSource>();
    public double nextStartTime;
    public int index = 0;
    public BlockManager BlockManager;
    public bool blockActive = false;
    void Start()
    {
        
    }
    void Update()
    {
        if (musicStart)
        {
            scheduleAudio();
        }
    }
    public void scheduleAudio()
    {
        if (AudioSettings.dspTime > nextStartTime - 1)
        {
            audioSource[index].PlayScheduled(nextStartTime);
            StartCoroutine(BlockManager.ScheduleEmit(index, (float)(nextStartTime - AudioSettings.dspTime)));
            Debug.Log("audio" + index + ": "+ nextStartTime);
            if (index > 2)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            float interval = audioSource[index].clip.length; 
            nextStartTime += interval;
        }
    }
    public void SwitchAudioState(int objectIndex)
    {
        if (objectIndex >= 0 && objectIndex < 4)
        {
            if (!audioSource[objectIndex].isPlaying)
            {
                switch (currentState[objectIndex])
                {
                    case 0:
                        currentState[objectIndex] = 1;
                        audioSource[objectIndex].clip = audioClipGrid[objectIndex].clips[0];
                        break;
                    case 1:
                        currentState[objectIndex] = 2;
                        audioSource[objectIndex].clip = audioClipGrid[objectIndex].clips[1];
                        break;
                    case 2:
                        currentState[objectIndex] = 0;
                        audioSource[objectIndex].clip = null;
                        break;
                }
                StartCoroutine(BlockManager.ChangeState(objectIndex, currentState[objectIndex]));
                
                StartCoroutine(BlockManager.PressEffect(1.0f, objectIndex));
            }
        }
    }
    public void ResetAudioManager(bool enableManager)
    {
        if (!enableManager) FadeAllVolume();

        // Stop all coroutines if any are running
        //StopAllCoroutines();
        index = 0;
        blockActive = false;
        // Deactivate this script
        this.enabled = enableManager; // Disable the AudioManager component
    }
    public void InitializeMusic()
    {
        index = 0;
       
    }
    void FadeAllVolume()
    {
        musicStart = false;
        foreach (var source in audioSource)
        {
            GameManager.Instance.FadeOutAudio(source);
        }
    }
}