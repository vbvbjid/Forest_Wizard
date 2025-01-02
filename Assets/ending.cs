using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ending : MonoBehaviour
{
    public bool end = false;
    public List<Material> materials;
    private ShaderMethod pulseUtility;
    // Start is called before the first frame update
    void Start()
    {
        materials.AddRange(GetComponent<Renderer>().materials);
        pulseUtility = gameObject.AddComponent<ShaderMethod>();
    }
    public void End(){
        pulseUtility.StopPulsing(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("hand") || collision.gameObject.CompareTag("Wand") && !end){
            end = true;
            pulseUtility.StartPulsing(materials);
            GameManager.Instance.EndGame();
        }
    }
}
