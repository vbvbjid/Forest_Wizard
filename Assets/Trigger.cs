using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SegmentSoundEffects
{
    [Tooltip("Sound effects for this segment")]
    public AudioClip[] soundEffects = new AudioClip[4]; // 4 sound effect types
}

[System.Serializable]
public class BGMSoundEffectMapping
{
    [Tooltip("Segments for this BGM type")]
    public SegmentSoundEffects[] segments = new SegmentSoundEffects[4]; // 4 segments
}

public class Trigger : MonoBehaviour
{

    public AudioSource audioSource;
    public int State = 0;
    public BGM bgm;
    public int numberOfStates = 8;

    [Tooltip("Mapping for all BGM types")]
    public BGMSoundEffectMapping[] bgmMappings = new BGMSoundEffectMapping[2]; // 2 BGM types

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void AccessSound(int BGM, int Segment, int type)
    {
        // Access an AudioClip: BGM 0, Segment 2, Sound Effect Type 1
        AudioClip clip = bgmMappings[0].segments[2].soundEffects[1];
        Debug.Log(clip != null ? "Clip found!" : "Clip not assigned.");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hand"))
        {
            State = (State + 1) % numberOfStates;
        }
    }
}
