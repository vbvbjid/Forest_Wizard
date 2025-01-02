using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderMethod : MonoBehaviour
{
    private static ShaderMethod instance;
    public static ShaderMethod Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ShaderMethod");
                instance = go.AddComponent<ShaderMethod>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private Dictionary<string, PulseData> activePulses = new Dictionary<string, PulseData>();

    private class PulseData
    {
        public List<Material> materials;
        public List<Color> originalColors;
        public float pulseDuration;
        public float minBrightness;
        public float maxBrightness;
        public Coroutine coroutine;
        
        public PulseData(List<Material> mats, float duration, float minBright, float maxBright)
        {
            materials = mats;
            originalColors = new List<Color>();
            foreach (Material mat in materials)
            {
                originalColors.Add(mat.color);
            }
            pulseDuration = duration;
            minBrightness = minBright;
            maxBrightness = maxBright;
        }
    }

    public void StartPulse(
        string pulseId,
        List<Material> materials, 
        float pulseDuration = 2f,
        float minBrightness = 0.2f,
        float maxBrightness = 1.5f)
    {
        // Stop existing pulse if it exists
        if (activePulses.ContainsKey(pulseId))
        {
            StopPulse(pulseId);
        }

        PulseData pulseData = new PulseData(materials, pulseDuration, minBrightness, maxBrightness);
        pulseData.coroutine = StartCoroutine(PulseEffect(pulseData));
        activePulses[pulseId] = pulseData;
    }

    public void StopPulse(string pulseId)
    {
        if (activePulses.TryGetValue(pulseId, out PulseData pulseData))
        {
            if (pulseData.coroutine != null)
            {
                StopCoroutine(pulseData.coroutine);
            }
            
            // Reset materials to original colors
            for (int i = 0; i < pulseData.materials.Count; i++)
            {
                if (pulseData.materials[i] != null)
                {
                    pulseData.materials[i].color = pulseData.originalColors[i];
                }
            }
            
            activePulses.Remove(pulseId);
        }
    }

    public void StopAllPulses()
    {
        List<string> pulseIds = new List<string>(activePulses.Keys);
        foreach (string pulseId in pulseIds)
        {
            StopPulse(pulseId);
        }
    }

    private IEnumerator PulseEffect(PulseData pulseData)
    {
        float elapsedTime = 0f;
        
        while (true)
        {
            elapsedTime += Time.deltaTime;
            
            float brightness = Mathf.Lerp(pulseData.minBrightness, pulseData.maxBrightness, 
                (Mathf.Sin(elapsedTime * (2f * Mathf.PI) / pulseData.pulseDuration) + 1f) / 2f);
            
            for (int i = 0; i < pulseData.materials.Count; i++)
            {
                if (pulseData.materials[i] != null)
                {
                    Color originalColor = pulseData.originalColors[i];
                    pulseData.materials[i].color = new Color(
                        originalColor.r * brightness,
                        originalColor.g * brightness,
                        originalColor.b * brightness,
                        originalColor.a
                    );
                }
            }
            
            yield return null;
        }
    }
}