using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public int State = 0;
    public int numberOfStates = 2;
    // Start is called before the first frame update
    void Start()
    {
        if(audioSource == null && !audioSource.isPlaying){
            audioSource.Play();
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
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hand"))
        {
            State = (State + 1) % numberOfStates;
        }
        float time = getPlayTime();
        audioSource.clip = clips[State];
        audioSource.time = time;
    }
}
