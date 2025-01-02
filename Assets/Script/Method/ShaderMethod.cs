using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderMethod : MonoBehaviour
{
    private Dictionary<Material, float> originalValues;
    private List<Material> activeMaterials;
    private bool isPulsing = false;
    private float frequency = 0.5f;
    private float darkFactor = 0.3f;
    private float currentValue = 1.0f;
    private bool isRecovering = false;

    void Awake()
    {
        // Initialize collections
        originalValues = new Dictionary<Material, float>();
        activeMaterials = new List<Material>();
        enabled = true;  // Ensure component is enabled on creation
    }

    public void StartPulsing(List<Material> materials, float pulseFrequency = 0.5f, float darkenAmount = 0.3f)
    {
        Debug.Log("pulse");
        frequency = pulseFrequency;
        darkFactor = darkenAmount;
        isRecovering = false;
        
        originalValues.Clear();
        activeMaterials = new List<Material>(materials);
        
        // Store original values and set current value to match
        foreach (Material mat in activeMaterials)
        {
            if (mat != null)
            {
                float h, s, v;
                Color.RGBToHSV(mat.color, out h, out s, out v);
                originalValues[mat] = v;
            }
        }

        currentValue = 1.0f;
        isPulsing = true;
        enabled = true;  // Ensure enabled when starting pulse
    }

    public void StopPulsing(bool restoreValues = true)
    {
        isPulsing = false;
        
        if (restoreValues && originalValues != null)
        {
            foreach (Material mat in activeMaterials)
            {
                if (mat != null && originalValues.ContainsKey(mat))
                {
                    float h, s, v;
                    Color.RGBToHSV(mat.color, out h, out s, out v);
                    Color newColor = Color.HSVToRGB(h, s, originalValues[mat]);
                    mat.color = new Color(newColor.r, newColor.g, newColor.b, mat.color.a);
                }
            }
        }

        originalValues.Clear();
        activeMaterials.Clear();
    }

    void Update()
    {
        if (!isPulsing || activeMaterials == null || activeMaterials.Count == 0)
            return;

        float deltaTime = Time.deltaTime * frequency;

        if (!isRecovering)
        {
            // Darkening phase - quick drop
            currentValue = Mathf.MoveTowards(currentValue, darkFactor, deltaTime * 2f);
            if (currentValue <= darkFactor)
            {
                isRecovering = true;
            }
        }
        else
        {
            // Recovery phase - gradual brighten
            currentValue = Mathf.MoveTowards(currentValue, 1.0f, deltaTime * 0.5f);
            if (currentValue >= 1.0f)
            {
                isRecovering = false;
            }
        }

        foreach (Material mat in activeMaterials)
        {
            if (mat != null && originalValues.ContainsKey(mat))
            {
                float h, s, v;
                Color.RGBToHSV(mat.color, out h, out s, out v);
                float targetValue = originalValues[mat] * currentValue;
                Color newColor = Color.HSVToRGB(h, s, targetValue);
                mat.color = new Color(newColor.r, newColor.g, newColor.b, mat.color.a);
            }
        }
    }

    void OnDisable()
    {
        if (isPulsing)
        {
            foreach (Material mat in activeMaterials)
            {
                if (mat != null && originalValues.ContainsKey(mat))
                {
                    float h, s, v;
                    Color.RGBToHSV(mat.color, out h, out s, out v);
                    Color newColor = Color.HSVToRGB(h, s, originalValues[mat]);
                    mat.color = new Color(newColor.r, newColor.g, newColor.b, mat.color.a);
                }
            }
        }
    }
}