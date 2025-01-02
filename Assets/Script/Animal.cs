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
    public bool firstTouch = false;
    public List<Material> myMaterials = new List<Material>();
    private string pulseEffectId;

    public void Start()
    {
        scheduledStartTime = 1e6;
        audioSource = gameObject.GetComponent<AudioSource>();
        if (animator == null) animator = GetComponent<Animator>();
        // Generate a unique ID for this instance's pulse effect
        pulseEffectId = $"pulse_{gameObject.GetInstanceID()}";
    }
    void Update()
    {
        if (AudioSettings.dspTime >= scheduledStartTime && trigger)
        {
            animator.SetBool("sing", true);
            StopPulsing();
        }
    }
    public void StartPulsing()
    {
        ShaderMethod.Instance.StartPulse(
            pulseEffectId,
            myMaterials,
            pulseDuration: 2f,
            minBrightness: 0.2f,
            maxBrightness: 1.5f
        );
    }

    public void StopPulsing()
    {
        ShaderMethod.Instance.StopPulse(pulseEffectId);
    }

    void OnDestroy()
    {
        // Clean up when the object is destroyed
        StopPulsing();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wand") && !collision.gameObject.CompareTag("hand")) return;
        if (!firstTouch)
        {
            firstTouch = true;
            GameManager.Instance.ShowAnimal(AnimalCode++);
        }
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
            StartPulsing();
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
}