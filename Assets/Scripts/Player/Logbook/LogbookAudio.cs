// Author: Noah Stolz
// Manages audio sounds for logbook
// Should be attached to the logbook

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogbookAudio : MonoBehaviour {

    [SerializeField]
    private AudioClip tabSound;
    [SerializeField]
    private AudioClip buttonClickSound;
    [SerializeField]
    private AudioClip buttonHoverSound;
    [SerializeField]
    private AudioClip openLogbook;
    [SerializeField]
    private AudioClip closeLogbook;
    [SerializeField]
    [Tooltip ("The audio source attached to the logbook")]
    private AudioSource logbookAudioSource;

    
    public void onChangeTab()
    {

        logbookAudioSource.PlayOneShot(tabSound);

    }

    public void onOpenLogbook()
    {

        logbookAudioSource.PlayOneShot(openLogbook);

    }

    public void onCloseLogbook()
    {

        logbookAudioSource.PlayOneShot(closeLogbook);

    }

    public void onClickButton()
    {

        logbookAudioSource.PlayOneShot(buttonClickSound);

    }

    public void onHoverButton()
    {

        logbookAudioSource.PlayOneShot(buttonHoverSound);

    }

}
