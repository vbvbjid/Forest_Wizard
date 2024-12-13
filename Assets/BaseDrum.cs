using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDrum : MonoBehaviour
{

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
        if (collision.gameObject.CompareTag("Wand"))
        {
            if (gameObject.name == "Drum")
            {
                if (GameManager.Instance.Drum.mute == true)
                {
                    GameManager.Instance.Drum.mute = false;
                }
                else
                {
                    GameManager.Instance.Drum.mute = true;
                }

            }
            else
            {
                if (GameManager.Instance.Base.mute == true)
                {
                    GameManager.Instance.Base.mute = false;
                }
                else
                {
                    GameManager.Instance.Base.mute = true;
                }
            }
        }
    }
}
