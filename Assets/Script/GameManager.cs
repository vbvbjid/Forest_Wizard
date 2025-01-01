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
    }
}