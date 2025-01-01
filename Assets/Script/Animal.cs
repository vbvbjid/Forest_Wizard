using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine.Rendering;
public class Animal : MonoBehaviour
{
    public Animator animator;
    public int AnimalCode;
    public AudioSource audioSource;
    public bool trigger = false;
    public bool processing = false;
    private double scheduledStartTime;
    public Material material;
    private Color originalColor;

    [SerializeField] private float breathingSpeed = 2f; // Adjust speed in inspector
    [SerializeField] [Range(0f, 1f)] private float darkAmount = 0.5f;

    public void Start()
    {
        scheduledStartTime = 1e6;
        audioSource = gameObject.GetComponent<AudioSource>();
        if (animator == null) animator = GetComponent<Animator>();
    }
    public void Sparkle(float duration)
    {
        StartCoroutine(SparkleCoroutine(duration));
    }

    private IEnumerator SparkleCoroutine(float duration)
    {
        float elapsedTime = 0f;
        float h, s, v;
        Color.RGBToHSV(originalColor, out h, out s, out v);
        
        while (elapsedTime < duration)
        {
            float breath = Mathf.Sin(elapsedTime * breathingSpeed * Mathf.PI) * 0.5f + 0.5f;
            float newV = Mathf.Lerp(v, v * (1 - darkAmount), breath);
            
            material.color = Color.HSVToRGB(h, s, newV);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        material.color = originalColor;
    }

    void OnDestroy()
    {
        if (material != null)
        {
            material.color = originalColor;
            Destroy(material);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wand")) return;
        if (processing)
        {
            Debug.Log("processing");
            return;
        }
        processing = true;
        if (!trigger)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "touch" && param.type == AnimatorControllerParameterType.Trigger)
                {
                    // Parameter exists and is a trigger, safe to set
                    animator.SetTrigger("touch");
                    break;
                }
            }
            // Get current playback time in seconds
            double currentTime = GameManager.Instance.BGM.time;
            double remainTime = GameManager.Instance.BGM.clip.length - currentTime;
            scheduledStartTime = AudioSettings.dspTime + remainTime;
            //Sparkle((float)remainTime);
            audioSource.PlayScheduled(scheduledStartTime);
            StartCoroutine(ProcessTimer(remainTime + 2.0f));
            Debug.Log("bgm time: " + currentTime);
            trigger = true;
        }
        else
        {
            StartCoroutine(ProcessTimer(2.0f));
            animator.SetBool("sing", false);
            audioSource.Stop();
            trigger = false;
        }
    }
    public IEnumerator ProcessTimer(double time)
    {
        yield return new WaitForSeconds((float)time);
        processing = false;
    }
    public void touchBotton()
    {
        if (processing)
        {
            Debug.Log("processing");
            return;
        }
        processing = true;
        if (!trigger)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "touch" && param.type == AnimatorControllerParameterType.Trigger)
                {
                    // Parameter exists and is a trigger, safe to set
                    animator.SetTrigger("touch");
                    break;
                }
            }
            // Get current playback time in seconds
            double currentTime = GameManager.Instance.BGM.time;
            double remainTime = GameManager.Instance.BGM.clip.length - currentTime;
            scheduledStartTime = AudioSettings.dspTime + remainTime;
            Sparkle((float)remainTime);
            audioSource.PlayScheduled(scheduledStartTime);
            StartCoroutine(ProcessTimer(remainTime + 2.0f));
            Debug.Log("bgm time: " + currentTime);
            trigger = true;
        }
        else
        {
            StartCoroutine(ProcessTimer(2.0f));
            animator.SetBool("sing", false);
            audioSource.Stop();
            trigger = false;
        }
    }
    void Update()
    {
        if (AudioSettings.dspTime >= scheduledStartTime && trigger)
        {
            animator.SetBool("sing", true);
        }
    }
}