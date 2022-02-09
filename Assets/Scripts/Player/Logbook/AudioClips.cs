// Author: Noah Stolz
// Defines methods for the buttons of the logbook audioEntries
// Should be attached to each audioEntry that is a child of display
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioClips : MonoBehaviour {

    [SerializeField]
    [Tooltip ("The Audio Source attached to this gameObject")]
    private AudioSource audioSource;

    /// <summary>Method for play button </summary>
    public void PlayAudio() {

        if (!audioSource.isPlaying) {

            audioSource.Play();

        }    
        
    }

    /// <summary>Method for pause button </summary>
    public void PauseAudio() {

        if (audioSource.isPlaying) {

            audioSource.Pause();

        } else {

            audioSource.UnPause();

        }

    }

    /// <summary>Method for reset button </summary>
    public void ResetAudio() {

        audioSource.Stop();
        audioSource.Play();

    }

    public void PlayPauseAudio()
    {

        if (audioSource.isPlaying)
        {

            audioSource.Pause();

        }
        else
        {

            audioSource.UnPause();

            if (!audioSource.isPlaying)
            {

                audioSource.Play();

            }

        }

    }
}
