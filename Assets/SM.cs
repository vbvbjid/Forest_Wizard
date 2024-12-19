using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSourceRow
{
    [SerializeField] public AudioSource[] audioSources = new AudioSource[4];
}

[System.Serializable]
public class SoloBufferRow
{
    [SerializeField] public bool[] SoloBuffers = new bool[4];
}

public class SM : MonoBehaviour
{
    public int animal;
    public bool mute = false;
    public bool Solo = false;
    [SerializeField] public AudioSourceRow[] audioSourceGrid = new AudioSourceRow[4];
    [SerializeField] public SoloBufferRow[] SoloBufferGrid = new SoloBufferRow[4];

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wand"))
        {
            if (gameObject.name == "Mute")
            {
                ToggleMute();
            }
            else
            {
                ToggleSolo();
            }
        }
    }

    private void ToggleMute()
    {
        mute = !mute;

        foreach (var audio in audioSourceGrid[animal].audioSources)
        {
            if (audio != null)
            {
                audio.mute = mute;
            }
        }
    }

    private void ToggleSolo()
    {
        if (Solo)
        {
            // Restore previous mute states from SoloBufferGrid
            for (int i = 0; i < SoloBufferGrid.Length; i++)
            {
                for (int j = 0; j < SoloBufferGrid[i].SoloBuffers.Length; j++)
                {
                    if (audioSourceGrid[i].audioSources[j] != null)
                    {
                        audioSourceGrid[i].audioSources[j].mute = SoloBufferGrid[i].SoloBuffers[j];
                    }
                }
            }
        }
        else
        {
            // Save current mute states to SoloBufferGrid
            for (int i = 0; i < SoloBufferGrid.Length; i++)
            {
                for (int j = 0; j < SoloBufferGrid[i].SoloBuffers.Length; j++)
                {
                    if (audioSourceGrid[i].audioSources[j] != null)
                    {
                        SoloBufferGrid[i].SoloBuffers[j] = audioSourceGrid[i].audioSources[j].mute;
                        audioSourceGrid[i].audioSources[j].mute = true; // Mute all initially
                    }
                }
            }

            // Unmute the selected row
            foreach (var audio in audioSourceGrid[animal].audioSources)
            {
                if (audio != null)
                {
                    audio.mute = false;
                }
            }
        }

        Solo = !Solo;
    }
}