using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;

    private void Start()
    {
        if (target != null)
        {
            target.SetActive(false);
        }
        else
        {
            Debug.LogError("Target is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger");
        if (other.gameObject.tag == "Player" || other.CompareTag("Wand"))
            target.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            target.SetActive(false);
    }
}
