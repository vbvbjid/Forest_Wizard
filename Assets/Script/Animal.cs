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
    public List<Material> materialsToAnimate;
    private ShaderMethod pulseUtility;
    public Flower flower;
    public List<Material> GetAllMaterials()
    {
        List<Material> materials = new List<Material>();

        // Get all Renderers (including MeshRenderer and SkinnedMeshRenderer)
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true); // true includes inactive objects

        foreach (Renderer renderer in renderers)
        {
            // Add all materials from each renderer
            materials.AddRange(renderer.materials);
        }

        return materials;
    }
    public void Start()
    {
        scheduledStartTime = 1e6;
        audioSource = gameObject.GetComponent<AudioSource>();
        if (animator == null) animator = GetComponent<Animator>();
        materialsToAnimate = GetAllMaterials();
        pulseUtility = gameObject.AddComponent<ShaderMethod>();
        pulseUtility.enabled = true;
    }
    void Update()
    {
        if (AudioSettings.dspTime >= scheduledStartTime && trigger)
        {
            pulseUtility.StopPulsing(true);
            animator.SetBool("sing", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.end) return;
        if (!collision.gameObject.CompareTag("Wand") && !collision.gameObject.CompareTag("hand")) return;
        if (!firstTouch)
        {
            firstTouch = true;
            if(AnimalCode == 2 || AnimalCode == 0)
                flower.Grow();
            GameManager.Instance.ShowAnimal(++AnimalCode);
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
            pulseUtility.StartPulsing(materialsToAnimate);
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
            pulseUtility.StartPulsing(materialsToAnimate);
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