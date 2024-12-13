using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Meta.Voice.Audio;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }
    public GameObject[] lights;
    public AudioManager[] AudioManagers;
    public newAM[] newAM;
    public GameObject[] AnimalBlocks;
    public int currentAnimal = 0;
    public AudioSource BGM;
    public AudioSource Base;
    public AudioSource Drum;
    public BlockManager[] TextureManagers;
    public int animalNumber = 3;
    public bool[] ActivedAnimal = new bool[4] { false, false, false, false };
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
    void Start()
    {
        animalNumber = 3;
        //StartCoroutine(InitializeSceneWithDelay(3.0f));
    }
    void Update()
    {
    }

    private IEnumerator InitializeSceneWithDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (currentAnimal == 0) lights[currentAnimal].SetActive(true);
    }

    private void StartAllMusic()
    {
        foreach (var audioManager in AudioManagers)
        {
            if (audioManager != null)
            {
                audioManager.StartMusic();  // Start each audio manager’s music
            }
        }
    }


    public void SwitchScene()
    {
        newAM[currentAnimal].ResetAudioManager(false);
        //TextureManagers[currentAnimal].enabled = false;
        if (BGM.isPlaying) FadeOutAudio(BGM);
        //lights[currentAnimal].SetActive(false);
        if (currentAnimal < animalNumber)
        {
            currentAnimal++;
            lights[currentAnimal].SetActive(true);
        }
        else
        {
            currentAnimal++;
            StartCoroutine(DelayedReset(4.0f));
        }
    }
    public IEnumerator DelayedReset(float duration)
    {
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < AudioManagers.Length; i++)
        {
            ActivedAnimal[i] = true;
            // Activate light and block, and initialize audio manager without background tasks
            lights[i].SetActive(true);
            AnimalBlocks[i].SetActive(true);
            //StartCoroutine(TextureManagers[i].PlayAnimationsAndShowBlocks(i));
            newAM[i].enabled = true;
        }
        double startTime = AudioSettings.dspTime + 2.0f;
        for (int i = 0; i < newAM.Length; i++)
        {
            newAM[i].nextStartTime = startTime;
            newAM[i].musicStart = true;
            newAM[i].blockActive = true;
        }
        BGM.PlayScheduled(startTime);
        Base.PlayScheduled(startTime);
        Drum.PlayScheduled(startTime);
    }
    public void ShowBlocks(int currentAnimal)
    {
        if (currentAnimal > animalNumber) return;
        AudioManagers[currentAnimal].blockActive = true;
        AnimalBlocks[currentAnimal].SetActive(true);
        //AudioManagers[currentAnimal].InitializeMusic();
        if (!BGM.isPlaying)
            BGM.PlayScheduled(AudioSettings.dspTime);
        AudioManagers[currentAnimal].StartMusic();
    }
    public float fadeDuration = 2.0f; // Time in seconds for the fade out

    public void FadeOutAudio(AudioSource audio)
    {
        // Start fading out each audio source
        StartCoroutine(FadeOutAudioSource(audio, fadeDuration));
    }

    private IEnumerator FadeOutAudioSource(AudioSource audioSource, float duration)
    {
        // Get the initial volume
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            // Gradually decrease the volume
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
                yield return null;
            }
        }
        // Set the final volume to 0 and stop the audio
        audioSource.Stop();
        audioSource.volume = 1;
    }
}