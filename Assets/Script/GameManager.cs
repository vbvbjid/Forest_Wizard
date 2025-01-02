using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Meta.Voice.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public AudioManager[] AudioManagers;
    public int currentAnimal = 0;
    public AudioSource BGM;
    public int animalNumber;
    public GameObject[] Animal = new GameObject[4];
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
        if (code >= 0 && code < 4)
        {
            Animal[code].SetActive(true);
        }
    }
    void Start()
    {
        animalNumber = 3;
        for(int i = 0; i <= animalNumber; i++){
            Animal[i].SetActive(false);
        }
    }
}