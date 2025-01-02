using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public Animator[] animators;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Grow(){
        foreach(var animator in animators){
            animator.SetTrigger("show");
        }
    }
    public void Press(){
        foreach (var animator in animators){
            animator.SetTrigger("press"); 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wand") && !collision.gameObject.CompareTag("hand")) return;
        Press();
    }
}
