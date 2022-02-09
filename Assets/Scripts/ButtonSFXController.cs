using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFXController : MonoBehaviour {

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip hoverSFX;

    [SerializeField]
    private AudioClip clickSFX;


    public void HoverSound()
    {
        source.PlayOneShot(hoverSFX);
    }

    public void ClickSound()
    {
        source.PlayOneShot(clickSFX);
    }
}
