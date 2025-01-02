using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WandScript : MonoBehaviour
{
    public Collider selfCollider;
    private float collisionCooldown = 1.0f;
    private float lastCollisionTime;
    public bool firstGrab = false;


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
            if (gameObject.CompareTag("Wand") && collision.gameObject.CompareTag("hand") && !firstGrab)
            {
                firstGrab = true;
                GameManager.Instance.ShowAnimal(0);
            }
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
                if (stringToEnum.TryGetValue(parent.name, out int code))
                {

                }
                else
                {
                    Debug.LogError("Invalid animal name: " + parent.name);
                }
            }
            // Handle collision
            lastCollisionTime = Time.time;
        }

    }

}