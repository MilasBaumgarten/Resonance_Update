using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Soundchanger : MonoBehaviour {
    public AudioClip backGround;
    public AudioSource source;
    public bool repeat;
    private bool trigger = true;


    private void OnTriggerEnter(Collider michi)
    {
        if (trigger)
        {
            if (michi.tag.Equals("Player"))
            {
                NetworkIdentity netID = michi.GetComponent<NetworkIdentity>();
                if (netID != null)
                {
                    if (netID.isLocalPlayer)
                    {
                        source.clip = backGround;
                        source.Play();
                    }
                    if (!repeat) trigger = false;

                }

            }
        }
    }


}
