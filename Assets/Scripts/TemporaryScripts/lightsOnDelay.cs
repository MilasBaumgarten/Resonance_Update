using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Realtime lights on after delay
//by Christian
public class lightsOnDelay : MonoBehaviour {


    [Tooltip("Drop all Lights in Order and set delay")]
    [SerializeField] private GameObject[] lights;

    public Material off;
    public Material on;
    Material[] myMaterials;
    public float delay;


    public void Awake()
    {
        
        foreach (GameObject light in lights)
        {
            light.GetComponentInChildren<Light>().enabled = false;
                
        }
        foreach (GameObject mesh in lights)
        {
            myMaterials = mesh.GetComponent<Renderer>().materials;
            myMaterials[1] = off;
            mesh.GetComponent<Renderer>().materials = myMaterials;
        }
    }

    IEnumerator TurnLightsOn()
    {
        for(int i = 0; i < lights.Length; i++)
        {
            lights[i].GetComponentInChildren<Light>().enabled = true;
            myMaterials[1] = on;
            lights[i].GetComponent<Renderer>().materials = myMaterials;
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(TurnLightsOn());  
    }
}
