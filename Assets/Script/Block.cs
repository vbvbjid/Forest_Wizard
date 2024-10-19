using UnityEngine;

public class Block : MonoBehaviour
{
    public AudioSource audioSource; // Assign this in the Inspector
    public Vector3 newPosition; // Set this to the desired position

    void Start()
    {
        // Change the position of the Audio Source
        if (audioSource != null)
        {
            audioSource.transform.position = newPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wand"))
        {
            if (gameObject.GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            }
            else
            {
                GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            }

        }
    }
}