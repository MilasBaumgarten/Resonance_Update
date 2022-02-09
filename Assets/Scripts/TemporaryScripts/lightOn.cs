using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightOn : MonoBehaviour
{

    public Light myLight;
    void OnTriggerStay(Collider other)
    {
        myLight.enabled = true;
    }

}