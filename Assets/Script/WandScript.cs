using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WandScript : MonoBehaviour
{
    public newAM[] newAM;
    public Collider selfCollider;
    private float collisionCooldown = 1.0f;
    private float lastCollisionTime;


    void Start()
    {
        selfCollider = GetComponent<Collider>();
    }
    void Update()
    {

    }
    // Detect collision with block to pass the audio clip to AudioBlocksManager
    private void OnCollisionEnter(Collision collision)
    {
        //collision cooldown timmer
        if (Time.time - lastCollisionTime > collisionCooldown)
        {
            if (collision.gameObject.CompareTag("Block"))
            {
                // Access the AudioBlocksManager on the block and fill it with the current audio clip
                int blockNumber = int.Parse(collision.gameObject.name);
                Debug.Log(blockNumber);
                GameObject parent = collision.transform.parent.gameObject;
                Dictionary<string, int> stringToEnum = new Dictionary<string, int>()
                {
                    { "thrush", 0 },
                    { "fox", 1 },
                    { "Squirrel&Cricket", 2 },
                    { "Buck", 3 }
                };

                // Assuming `code` is the result of looking up the string from `stringToEnum`
                if (stringToEnum.TryGetValue(parent.name, out int code) && newAM[code].blockActive)
                {
                    newAM[code].SwitchAudioState(blockNumber);
                    /*switch (code)
                    {
                        case 0:
                            if (Fox.blockActive)
                                Fox.SwitchAudioState(blockNumber);
                            break;
                        case 1:
                            if (Boar.blockActive)
                                Boar.SwitchAudioState(blockNumber);
                            break;
                        case 2:
                            if (Squirrel.blockActive)
                                Squirrel.SwitchAudioState(blockNumber);
                            break;
                        case 3:
                            if (Raccoon.blockActive)
                                Raccoon.SwitchAudioState(blockNumber);
                            break;
                        default:
                            Debug.LogError("Unknown animal");
                            break;
                    }*/
                }
                else
                {
                    Debug.LogError("Invalid animal name: " + parent.name );
                }
            }
            // Handle collision
            lastCollisionTime = Time.time;
        }

    }

}