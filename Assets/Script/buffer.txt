using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBlocksManager : MonoBehaviour
{
    public GameObject[] blocks; // Assign the four block GameObjects in the editor
    public AudioClip[] blockClips = new AudioClip[4]; // Holds audio for each block
    public float blockDuration = 5f; // Duration of each block playback
    public AudioSource audioSource; // Assign in editor or via script
    public bool[] isSequenceEnabled;
    private int currentBlockIndex = 0;

    public int musicLength = 4;

    void Start()
    {
        // Ensure the blocks are initially empty
        /*for (int i = 0; i < blockClips.Length; i++)
        {
            blockClips[i] = null;
        }*/
        isSequenceEnabled = new bool[musicLength];
        for(int i = 0; i < musicLength; i ++){
            isSequenceEnabled[i] = true;
        }
    }

    void Update()
    {
        // Optional: Check player input or wand collision to fill the block with audio
    }

    public IEnumerator PlayBlocksSequentially()
    {
        /*bool hasClip = false;
        foreach (AudioClip clip in blockClips)
        {
            if (clip != null)
            {
                hasClip = true; // Set the flag to false if any block is empty
                break; // Exit loop since we only need to know if one block is empty
            }
        }
        if (!hasClip)
        {
            Debug.Log("No blocks have music!"); // Optional: Log a message if no block is filled
            yield break; // Exit the coroutine if no block has music
        }*/

        while (true)
        {
            // Play the current block's audio clip
            AudioClip currentClip = blockClips[currentBlockIndex];
            PlayCurrentBlock();

            // Get the clip's length and compare it to the block duration
            float clipLength = currentClip != null ? currentClip.length : 0f;
            float remainingTime = blockDuration - clipLength;

            // Wait for the clip to finish playing
            if (clipLength > 0f)
            {
                yield return new WaitForSeconds(clipLength); // Wait for the length of the audio clip
            }

            // If there's remaining time, wait in silence
            if (remainingTime > 0f)
            {
                yield return new WaitForSeconds(remainingTime); // Wait for the remaining block time
            }

            // Move to the next block
            currentBlockIndex = (currentBlockIndex + 1) % blockClips.Length;
        }
    }

    private void PlayCurrentBlock()
    {
        AudioClip clip = blockClips[currentBlockIndex];
        if (clip != null && isSequenceEnabled[currentBlockIndex])
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop(); // No audio for this block
        }
    }

    private void OnTriggerEnter(Collider other) { }

    public void FillBlockWithAudio(int blockIndex, AudioClip clip)
    {
        if (blockIndex >= 0 && blockIndex < blockClips.Length)
        {
            blockClips[blockIndex] = clip; // Fill the block with the selected audio
        }
    }
}
