using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WandScript : MonoBehaviour
{
    public AudioSource audioSource;  // The AudioSource on the wand to hold the audio
    public bool hasAudioClip = false;  // Detect if wand has an audio clip
    private AudioClip currentClip;  // The currently held audio clip
    public AudioManager Fox;
    public AudioManager Thrush;
    public AudioManager Boar;
    public AudioManager Buck;
    public AudioManager Squirrel;
    public AudioManager Raccoon;
    public AudioManager Grasshopper;
    public AudioManager Dove;
    public bool isPlayingLoop = false;
    public GameObject MusicLightPrefab;
    public Vector3 offset = new Vector3(0f, 1f, 0f); // Define your offset
    public bool isPlaying = false;
    public GameObject parent;
    public Material OnMaterial;
    public Material OffMaterial;


    void Start()
    {
        // Automatically assign the AudioSource component if it's not manually assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    void Update()
    {

    }
    public void previewAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    public void stopAudioPreview(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
    // Detect collision with block to pass the audio clip to AudioBlocksManager
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        //AudioBlocksManager blockManager = AudioBlocksManager.gameObject.GetComponent<AudioBlocksManager>();
        if (collision.gameObject.CompareTag("OriginSoundObject"))
        {
            // Assuming the audio provider has an AudioSource with the appropriate clip
            AudioSource providerAudio = collision.gameObject.GetComponent<AudioSource>();
            Renderer collidedObjectRenderer = collision.gameObject.GetComponent<Renderer>();
            if (providerAudio != null && providerAudio.clip != null)
            {
                // Assign the clip from the audio provider to the wand
                if (!providerAudio.isPlaying)
                {
                    previewAudio(providerAudio);
                }
                else
                {
                    stopAudioPreview(providerAudio);
                }
            }
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            // Access the AudioBlocksManager on the block and fill it with the current audio clip
            int blockNumber = int.Parse(collision.gameObject.name);
            GameObject parent = collision.transform.parent.gameObject;
            Dictionary<string, int> stringToEnum = new Dictionary<string, int>()
            {
                { "fox", 0 },
                { "Boar", 1 },
                { "Squirrel&Cricket", 2 },
                { "Raccoon", 3 },
                { "thrush", 4 },
                { "grasshopper", 5 },
                { "Dove", 6 },
                { "Buck", 7 }
            };

            // Assuming `code` is the result of looking up the string from `stringToEnum`
            if (stringToEnum.TryGetValue(parent.name, out int code))
            {
                switch (code)
                {
                    case 0:
                        Fox.SwitchAudioState(blockNumber);
                        break;
                    case 1:
                        Boar.SwitchAudioState(blockNumber);
                        break;
                    case 2:
                        Squirrel.SwitchAudioState(blockNumber);
                        break;
                    case 3:
                        Raccoon.SwitchAudioState(blockNumber);
                        break;
                    case 4:
                        Thrush.SwitchAudioState(blockNumber);
                        break;
                    case 5:
                        Grasshopper.SwitchAudioState(blockNumber);
                        break;
                    case 6:
                        Dove.SwitchAudioState(blockNumber);
                        break;
                    case 7:
                        Buck.SwitchAudioState(blockNumber);
                        break;
                    default:
                        Debug.LogError("Unknown animal");
                        break;
                }
            }
            else
            {
                Debug.LogError("Invalid animal name");
            }
            /*if (blockManager != null)
            {
                // Example logic to assign the clip to a specific block (e.g., block 0)
                
                if (blockManager.isSequenceEnabled[blockNumber] == true)
                    blockManager.isSequenceEnabled[blockNumber] = false;
                else
                    blockManager.isSequenceEnabled[blockNumber] = true;
                //blockManager.FillBlockWithAudio(blockNumber, currentClip);
                //GameObject parent = GameObject.Find("Blocks");
                //string childName = ;
                //Transform childTransform = parent.transform.Find("ChildObjectName");
                //GameObject childObject = childTransform != null ? childTransform.gameObject : null;
                // Reset the wand's audio clip after filling the block
                //hasAudioClip = false;
                //audioSource.clip = null;
            }*/
        }
        /*else if (collision.gameObject.CompareTag("MusicPlayer"))
        {

            if (!isPlayingLoop)
            {
                isPlayingLoop = true;
                Debug.Log("startLoop");
                StartCoroutine(blockManager.PlayBlocksSequentially());
            }
            else
            {
                isPlayingLoop = false;
                Debug.Log("stopLoop");
                StopCoroutine(blockManager.PlayBlocksSequentially());
            }
        }*/
    }
}