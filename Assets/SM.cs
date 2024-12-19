using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM : MonoBehaviour
{
    public int animal;
    public bool mute = false;
    public bool Solo = false;
    [SerializeField]
    public AudioSource[] audioSources;
    public bool[] SoloBuffer;
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wand"))
        {
            if (gamObject.name == "Mute")
            {
                if (mute)
                {
                    audioSources[animal].mute = true;
                    mute = false;
                }
                else
                {
                    audioSources[animal].mute = false;
                    mute = true;
                }
            }
            else
            {
                if (Solo)
                {
                    for(int i = 0; i < audioSources.length; i ++)
                    {
                        SoloBuffer[i] = audioSources[i].mute;
                    }
                    Solo = false;
                }
                else
                {
                    for(int i = 0; i < SoloBuffer.length; i ++)
                    {
                        audioSources[i].mute = SoloBuffer[i];
                    }
                    audioSources[animal].mute = false;
                    Solo = true;
                }
            }


        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
