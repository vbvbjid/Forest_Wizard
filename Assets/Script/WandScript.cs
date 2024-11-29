using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WandScript : MonoBehaviour
{
    public AudioManager Fox;
    public AudioManager Thrush;
    public AudioManager Boar;
    public AudioManager Buck;
    public AudioManager Squirrel;
    public AudioManager Raccoon;
    public AudioManager Grasshopper;
    public AudioManager Dove;
    public Collider selfCollider;
    public float disableDuration = 2;
    private float collisionCooldown = 0.1f;
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
                        case 4:
                            if (Thrush.blockActive)
                            {
                                Thrush.SwitchAudioState(blockNumber);
                            }
                            break;
                        case 5:
                            if (Grasshopper.blockActive)
                                Grasshopper.SwitchAudioState(blockNumber);
                            break;
                        case 6:
                            if (Dove.blockActive)
                                Dove.SwitchAudioState(blockNumber);
                            break;
                        case 7:
                            if (Buck.blockActive)
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
                StartCoroutine(DisableColliderTemporarily());
            }
            // Handle collision
            lastCollisionTime = Time.time;
        }

    }
    private IEnumerator DisableColliderTemporarily()
    {
        // Disable the collider
        selfCollider.enabled = false;

        // Wait for the specified duration
        yield return new WaitForSeconds(disableDuration);

        // Re-enable the collider
        selfCollider.enabled = true;
    }
}