/* Script that controls audio levels
 * 
 * need to add on slider parent
 * 
 * Author: Maximilian Rietzsch
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLvl : MonoBehaviour {

    //set audioMixer reference 
    [SerializeField]
    private AudioMixer audioMixer;

    //set control float for music volume
    public void SetMusicLvl(float musicLevel) {
        audioMixer.SetFloat("MusicVol", musicLevel);
    }

    //set control float for effects volume
    public void SetSfxLvl(float sfxLevel)
    {
        audioMixer.SetFloat("SfxVol", sfxLevel);
    }
}
