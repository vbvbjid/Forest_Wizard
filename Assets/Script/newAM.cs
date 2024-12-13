using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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
    void Start()
    {
        if (musicStart)
        {
            scheduleAudio();
        }
    }
    void Update()
    {

    }
    public void scheduleAudio()
    {
        int index = 0;
        double nextStartTime = AudioSettings.dspTime + 0.2f;
        if (AudioSettings.dspTime < nextStartTime - 1.0f)
        {
            audioSource[index].PlayScheduled(nextStartTime);
            if (index > 2)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            nextStartTime += 2.0f;
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
                        audioSource[objectIndex].clip = audioClipGrid[objectIndex].clips[1];
                        break;
                    case 1:
                        currentState[objectIndex] = 2;
                        audioSource[objectIndex].clip = null;
                        break;
                    case 2:
                        currentState[objectIndex] = 0;
                        audioSource[objectIndex].clip = audioClipGrid[objectIndex].clips[0];
                        break;
                }
            }
        }
    }
}