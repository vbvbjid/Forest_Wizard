using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Meta.Voice.Audio;
using Meta.WitAi.Data;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public AudioManager[] AudioManagers;
    public int currentAnimal = 0;
    public AudioSource BGM;
    public int animalNumber;
    public GameObject[] Animal = new GameObject[4];
    public AudioSource Ending;
    public Animator[] animators;
    double scheduledStartTime;
    public bool end = false;
    public ending ending;
    private void Awake()
    {
        // Implement Singleton Pattern logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep instance across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
    public void ShowAnimal(int code)
    {
        Debug.Log("code: " + code);
        if (code >= 0 && code <= animalNumber)
        {
            Animal[code].SetActive(true);
        }
    }
    void Start()
    {
        scheduledStartTime = 1e6;
        animalNumber = 5;
        for (int i = 0; i <= animalNumber; i++)
        {
            Animal[i].SetActive(false);
        }
    }
    void Update()
    {
        if (AudioSettings.dspTime >= scheduledStartTime)
        {
            ending.End();
            foreach (Animator animator in animators)
            {
                animator.SetBool("sing", true);
            }
        }
    }
    public void EndGame()
    {
        end = true;
        double currentTime = GameManager.Instance.BGM.time;
        double remainTime = GameManager.Instance.BGM.clip.length - currentTime;
        scheduledStartTime = AudioSettings.dspTime + remainTime;
        Ending.PlayScheduled(scheduledStartTime);
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            currentTime = GameManager.Instance.BGM.time;
            remainTime = GameManager.Instance.BGM.clip.length - currentTime;
            scheduledStartTime = AudioSettings.dspTime + remainTime;
            if (audioSource == Ending)
                continue;
            audioSource.SetScheduledEndTime(scheduledStartTime);
        }
    }
}