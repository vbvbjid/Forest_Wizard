using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class touch : MonoBehaviour
{
    public GM gM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hand"))
        {
            StartCoroutine(gM.SwitchScene(1, 2, "morning"));
        }
    }
}
