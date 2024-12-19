using UnityEngine;
using System.Collections;
using Unity.EditorCoroutines.Editor;
public class Animal : MonoBehaviour
{
    public Animator animator;
    public int AnimalCode;
    public AudioSource audioSource;
    public BlockManager blockManager;
    public bool BGMscheduled = false;
    public newAM newAM;
    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (animator == null) animator = GetComponent<Animator>();
    }
    public void StartAnimation()//show audio block
    {
        StartCoroutine(blockManager.PlayAnimationsAndShowBlocks(AnimalCode));
    }
    public void PlayAudio()
    {
        //Stop to start form the beginning
        if (audioSource.isPlaying)
            audioSource.Stop();
        if (audioSource != null)
        {
            audioSource.volume = 1.0f;
            audioSource.Play();
        }
    }
    //All sing actions
    private IEnumerator ShoutCoroutine()
    {
        // Restart animation
        //animator.SetTrigger("Sing");
        //animator.SetBool("isSinging", true);
        // Start audio
        AudioMethod.Instance.PlayAudio(audioSource);

        yield return new WaitForEndOfFrame();

        // Update game state after animation/audio completion
        if (!GameManager.Instance.ActivedAnimal[AnimalCode])
        {
            GameManager.Instance.ActivedAnimal[AnimalCode] = true;
            if (AnimalCode < 4)
            {
                //StartAnimation(); // Assuming this involves UI or another Unity-specific call
            }
        }
    }
    public IEnumerator EnableBlock(float duration)
    {
        yield return new WaitForSeconds(duration);
        newAM.blockActive = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //ensure activeated
        //if (AnimalCode != GameManager.Instance.currentAnimal && GameManager.Instance.currentAnimal <= 3) return;
        //check collision object
        if (!collision.gameObject.CompareTag("Wand")) return;
        // Play audio and animation
        animator.SetTrigger("Touch");  
        StartCoroutine(ShoutCoroutine());
        double startTime;
        //GameManager.Instance.ShowBlocks(GameManager.Instance.currentAnimal);
        if (!BGMscheduled)
        {
            BGMscheduled = true;
            startTime = AudioSettings.dspTime + 3.0f;
            GameManager.Instance.BGM.PlayScheduled(startTime);
            GameManager.Instance.Drum.PlayScheduled(startTime);
            GameManager.Instance.Base.PlayScheduled(startTime);
            Debug.Log("BGM: " + startTime);
            newAM.nextStartTime = startTime;
            if (AnimalCode == 1 || AnimalCode == 0)
            {
                Debug.Log("Bloom");
                blockManager.ShowBlock();
            }
            StartCoroutine(EnableBlock((float)(startTime - AudioSettings.dspTime + 1)));
            newAM.musicStart = true;
        }
    }
}