using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTextureManager : MonoBehaviour
{
    // List of material game objects (which have Renderer components)
    public List<GameObject> materialGameObjects = new List<GameObject>();
    // Emission color and intensity settings
    public Color emissionColor = Color.white;
    public float emissionIntensity = 1.0f;
    public AudioManager AudioManager;
    public Material[] stateMaterials;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeStateMaterial(int BlockIndex, int StateIndex)
    {
        // Get the Renderer component of the object at the given BlockIndex
        Renderer renderer = materialGameObjects[BlockIndex].GetComponent<Renderer>();

        // Check if the renderer and its material exist
        if (renderer != null && renderer.material != null)
        {
            // Define the colors based on the StateIndex (0: Red, 1: Orange, 2: Yellow, etc.)
            Color colorToSet = Color.white; // Default color in case the index is out of bounds
            switch (StateIndex)
            {
                case 0: colorToSet = Color.red; break;
                case 1: colorToSet = new Color(1.0f, 0.5f, 0.0f); break; // Orange
                case 2: colorToSet = Color.yellow; break;
                case 3: colorToSet = Color.green; break;
                case 4: colorToSet = Color.blue; break;
                case 5: colorToSet = new Color(0.29f, 0.0f, 0.51f); break; // Indigo
                case 6: colorToSet = new Color(0.93f, 0.51f, 0.93f); break; // Violet
                default: colorToSet = Color.white; break; // Handle invalid indices
            }

            // Change the material's color
            renderer.material.color = colorToSet;
        }
    }
    // Function to enable or disable the emission of a material
    public void SetMaterialEmission(int index, float duration)
    {
        Renderer renderer = materialGameObjects[index].GetComponent<Renderer>();

        if (renderer != null && renderer.material != null)
        {
            StartCoroutine(EmitForFixedTime(renderer, duration, index));  // Start the emission for a fixed time
        }
    }
    // Coroutine to handle the emission for a fixed time
    private IEnumerator EmitForFixedTime(Renderer renderer, float duration, int index)
    {
        // Enable the emission
        renderer.material.EnableKeyword("_EMISSION");
        Color finalEmissionColor = emissionColor * Mathf.LinearToGammaSpace(emissionIntensity);
        renderer.material.SetColor("_EmissionColor", finalEmissionColor);

        // Wait for the emissionDuration
        yield return new WaitForSeconds(duration);

        // Disable the emission after the time has elapsed
        renderer.material.DisableKeyword("_EMISSION");
    }
}
