using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Leon Ullrich
 * Create this as an Object in the Assets-Folder and call the needed Methods from any script that uses this object
 */

[CreateAssetMenu(fileName = "new_soundDatabase", menuName = "SoundDatabase")]
public class SoundDatabase : ScriptableObject {

    [System.Serializable]
    public class SoundData {
        // the name of a group, for example "step-sounds", or "force-tool-sounds"
        public string groupName;
        // the AudioSource in the scene from which the AudioClips will play
        public AudioSource audioSource;
        // the list of AudioClips
        public List<AudioClip> audioClips;      
    }
       
    public List<SoundData> soundDataGroups;

    /// <summary>
    /// Can be used to set up an AudioSource from another Script.
    /// </summary>
    /// <param name="group">The group, of which the AudioSource needs to be set</param>
    /// <param name="source">The AudioSource that gets assigned to the group during runtime</param>
    public void SetAudioSource(string group, AudioSource source) {
        for(int i = 0; i < soundDataGroups.Count; i++) {
            if(group == soundDataGroups[i].groupName) {
                soundDataGroups[i].audioSource = source;
            }
        }
    }

    /// <summary>
    /// Plays a random AudioSource from the given group-name
    /// </summary>
    /// <param name="groupName">Must be one of the group names set in the ScriptableObject</param>
    public void PlayAudioFromGroup(string groupName) {

        List<AudioClip> audioClips = null;
        AudioSource source = null;

        foreach(SoundData data in soundDataGroups) {
            if(data.groupName == groupName) {
                audioClips = data.audioClips;
                source = data.audioSource;
            }
        }

        if(source != null && audioClips != null) {

            int index = Random.Range(0, audioClips.Count);
            source.PlayOneShot(audioClips[index]);

        } else if(audioClips == null){
            Debug.LogError("No AudioGroup with group-name " + groupName + " found. Make sure it exists in the linked ScriptableObject.");
        } else if(source == null) {
            Debug.LogError("No AudioSource found in given sound group");
        }
    }

    /// <summary>
    /// Plays an AudioSource from a given groupName at the index of the AudioClip in the List of AudioClips
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <param name="index">The index of the AudioSource</param>
    public void PlayAudioFromGroup(string groupName, int index) {

        List<AudioClip> audioClips = null;
        AudioSource source = null;

        foreach(SoundData data in soundDataGroups) {
            if(data.groupName == groupName) {
                audioClips = data.audioClips;
                source = data.audioSource;
            }
        }

        if(source != null && audioClips != null) {

            source.PlayOneShot(audioClips[index]);

        } else if (audioClips == null) {
            Debug.LogError("No AudioGroup with group-name " + groupName + " found. Make sure it exists in the linked ScriptableObject.");
        } else if (source == null) {
            Debug.LogError("No AudioSource found in given sound group");
        }

    }

}


