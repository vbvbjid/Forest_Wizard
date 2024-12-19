using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.Animations;

public class BlockManager : MonoBehaviour
{
    // List of material game objects (which have Renderer components)
    public List<GameObject> materialGameObjects = new List<GameObject>();
    // Emission color and intensity settings
    public Color emissionColor = Color.white;
    public float emissionIntensity = 0.1f;
    public Animator[] animator = new Animator[4] { null, null, null, null };
    private bool[] Blocks = new bool[4] { false, false, false, false };
    private int InteractedBlock = 4;
    private Coroutine emitCoroutine;
    public List<GameObject> Acc_1 = new List<GameObject>(4);
    public List<GameObject> Acc_2 = new List<GameObject>(4);
    void Awake()
    {
        SetAllBlockColor(Color.gray);
    }
    public void SetAllBlockColor(Color color)
    {
        Debug.Log(color);
        // Iterate over each block in materialGameObjects
        foreach (var block in materialGameObjects)
        {
            // Get the Renderer component of the block
            Renderer renderer = block.GetComponent<Renderer>();

            // Check if the renderer exists
            if (renderer != null)
            {
                // Set the color to black
                renderer.material.color = color;
                // Disable emission by setting the emission color to black
                renderer.material.DisableKeyword("_EMISSION");
            }
        }
        InteractedBlock = 4;  // Reset the InteractedThrush counter
    }

    public IEnumerator ChangeState(int BlockIndex, int StateIndex)
    {
        SwitchAnimation(BlockIndex);
        yield return new WaitForSeconds(1.0f);
        // Get the Renderer component of the object at the given BlockIndex
        Renderer renderer = materialGameObjects[BlockIndex].GetComponent<Renderer>();
        int animalCode = 0;
        if(gameObject.name == "thrush"){
            animalCode = 0;
        }
        else if(gameObject.name == "thrush"){
            animalCode = 1;
        }
        else if(gameObject.name == "Squirrel&Cricket"){
            animalCode = 2;
        }
        else if(gameObject.name == "Buck"){
            animalCode = 3;
        }
        switch (StateIndex)
        {
            //State 0: gray out
            case 0:
                animator[animalCode].SetTrigger("1");
                if (gameObject.name == "thrush")
                {
                    Acc_1[BlockIndex].SetActive(false);
                    Acc_2[BlockIndex].SetActive(false);
                    renderer.material.color = Color.gray;
                }
                else
                {
                    renderer.material.color = Color.gray;
                }

                break;
            //State 1: switch the first accessory
            case 1:
                animator[animalCode].SetTrigger("2");
                if (gameObject.name == "thrush")
                {
                    Acc_2[BlockIndex].SetActive(false);
                    Acc_1[BlockIndex].SetActive(true);
                    renderer.material.color = Color.white;
                }
                else
                {
                    renderer.material.color = Color.red;
                }

                break;
            //State 1: switcht to the second accessory
            case 2:
                if (gameObject.name == "thrush")
                {
                    Acc_1[BlockIndex].SetActive(false);
                    Acc_2[BlockIndex].SetActive(true);
                    renderer.material.color = Color.white;
                }
                else
                {
                    renderer.material.color = Color.blue;
                }
                break;
            default: break; // Handle invalid indices
        }
    }
    public IEnumerator ScheduleEmit(int index, float time)
    {
        Debug.Log("wait" + time);
        yield return new WaitForSeconds(time);
        Renderer renderer = materialGameObjects[index].GetComponent<Renderer>();
        renderer.material.EnableKeyword("_EMISSION");
        Color finalEmissionColor = emissionColor * emissionIntensity;
        renderer.material.SetColor("_EmissionColor", finalEmissionColor);
        yield return new WaitForSeconds(2.0f);
        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    // Function to enable or disable the emission of a material
    public void SetMaterialEmission(int index, float duration)
    {
        Renderer renderer = materialGameObjects[index].GetComponent<Renderer>();
        emitCoroutine = StartCoroutine(EmitForFixedTime(renderer, duration, index));  // Start the emission for a fixed time
    }
    void SwitchAnimation(int index)
    {
        if (gameObject.name == "fox")
        {
            animator[index].SetTrigger("Switch");
        }
    }
    public void ShowBlock()
    {
        if (gameObject.name == "fox")
        {
            foreach (Animator anim in animator)
            {
                anim.SetBool("Show", true);
            }
        }
    }
    public IEnumerator PlayAnimationsAndShowBlocks(int currentAnimal)
    {
        /*switch (currentAnimal)
        {
            case 0:
                int birdCode = 0;
                foreach (Animator anim in animator)
                {
                    anim.SetBool("fly", true);
                    anim.SetBool("show", true);
                    anim.SetInteger("code", birdCode);
                    birdCode++;
                }
                yield return new WaitForSeconds(1);
                break;
            case 1:
                int foxCode = 0;
                foreach (Animator anim in animator)
                {
                    anim.SetBool("show", true);
                    anim.SetInteger("code", foxCode);
                    //anim.Play("Fox_Sit3_StandUp");
                    anim.Play(foxCode.ToString());
                    foxCode++;
                }
                break;
            default:
                break;
        }*/
        yield return new WaitForSeconds(1);
        // Now show blocks

    }
    public void ReturnAnimation()
    {
        switch (GameManager.Instance.currentAnimal)
        {
            case 0:
            case 1:
                int Code = 0;
                foreach (Animator anim in animator)
                {
                    anim.SetBool("show", false);
                    anim.SetInteger("code", Code);
                    string name = "r" + Code.ToString();
                    //anim.Play(name);
                    Code++;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator ResumeAnimationsAfterDelay(int index, float delay, string animationName)
    {
        yield return new WaitForSeconds(delay);
        animator[index].SetBool(animationName, false);
    }
    public IEnumerator PressEffect(float duration, int index)
    {

        Vector3 originalPosition = materialGameObjects[index].transform.localPosition;
        float bounceHeight = 0.05f;
        float bounceSpeed = 2.0f;

        // Handle the thrush interaction logic
        if (Blocks[index] == false)
        {
            Blocks[index] = true;
            if (--InteractedBlock <= 0)
            {

                //yield return new WaitForSeconds(15.0f);
                //SetAllBlockColor(Color.white);
                //ReturnAnimation();
                yield return new WaitForSeconds(3.0f);
                GameManager.Instance.SwitchScene();
                StopCoroutine(emitCoroutine);
            }
        }

        // Lerp to press down
        float elapsed = 0f;
        Vector3 targetPosition = originalPosition - new Vector3(0, bounceHeight, 0);

        while (elapsed < duration / 2)
        {
            materialGameObjects[index].transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsed / (duration / 2));
            elapsed += Time.deltaTime * bounceSpeed;
            yield return null;
        }

        // Ensure the object reaches the target position exactly
        materialGameObjects[index].transform.localPosition = targetPosition;

        // Lerp to release back up
        elapsed = 0f;
        while (elapsed < duration / 2)
        {
            materialGameObjects[index].transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsed / (duration / 2));
            elapsed += Time.deltaTime * bounceSpeed;
            yield return null;
        }

        // Ensure the object returns to the original position exactly
        materialGameObjects[index].transform.localPosition = originalPosition;
    }

    // Coroutine to handle the emission for a fixed time
    private IEnumerator EmitForFixedTime(Renderer renderer, float duration, int index)
    {
        // Enable the emission by setting a high enough emission color intensity
        if (renderer != null)
        {
            renderer.material.EnableKeyword("_EMISSION");
            Color finalEmissionColor = emissionColor * emissionIntensity;
            renderer.material.SetColor("_EmissionColor", finalEmissionColor);
        }
        // Wait for the emissionDuration
        yield return new WaitForSeconds(duration);

        // Disable the emission after the time has elapsed
        if (renderer != null) renderer.material.SetColor("_EmissionColor", Color.black);
        if (gameObject.name == "fox" || gameObject.name == "thrush")
            animator[index].SetBool("Sing", false);
    }
}