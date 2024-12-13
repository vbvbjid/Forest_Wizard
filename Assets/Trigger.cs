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
    public int numberOfStates = 4;

    [Tooltip("Mapping for all BGM types")]
    public BGMSoundEffectMapping[] bgmMappings = new BGMSoundEffectMapping[2]; // 2 BGM types

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public AudioClip AccessSound(int BGM, int Segment, int type)
    {
        // Access an AudioClip: BGM 0, Segment 2, Sound Effect Type 1
        AudioClip clip = bgmMappings[BGM].segments[Segment].soundEffects[type];
        Debug.Log(clip != null ? "Clip found!" : "Clip not assigned.");
        return clip;
    }
    public void switchEffect()
    {
        State = (State + 1) % numberOfStates;
        int CurrentBGM = bgm.State;

        float playTime = bgm.getPlayTime(); // Current play time of the BGM
        float bgmLength = bgm.ClipLengths[CurrentBGM]; // Total length of the BGM in seconds
        int segment = Mathf.FloorToInt(playTime / (bgmLength / 4)); // Calculate the segment

        // Clamp the segment to ensure it's in the valid range [0, 3]
        segment = Mathf.Clamp(segment, 0, 3);
        Debug.Log("CurrentBGM: " + CurrentBGM + ", segment: " + segment + ", type: " + State);
        AudioClip soundEffect = AccessSound(CurrentBGM, segment, State);
        Debug.Log("clip: " + soundEffect.name);
        audioSource.PlayOneShot(soundEffect);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hand"))
        {
            switchEffect();
        }
    }
}
