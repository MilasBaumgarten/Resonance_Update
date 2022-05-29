using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

/**
 * Author: Leon Ullrich
 * Description
 *  -this script handles DialogTriggers which are triggered by DialogTrigger.cs
 *  -script needs to take in the filename which is set in the editor under the object which holds the DialogTrigger.cs component
 *		column 1: delay 
 *		column 2: german
 *		column 3: english
 *		column 4: name of german audio file
 *		column 5: name of english audio file
 *  -put this script on the DialogManager gameobject
 *  -look for the object where the AudioSource is on (line 41)
 *  -make sure the csv file has the structure from Test-Dialog 1.csv (for now)
 *  -audio files needs to be in a folder named resources
 *  -TODO add another dialog to the end if a dialog is triggered while another one is playing
 *  
 * Author: Marisa Schmelzer
 * Description:
 *  -added audio to this script
 *  -TODO synchronize shown text with length of audio in csv file
 *  
 *  Author: Milas Baumgarten
 *  Description:
 *	 - added Tooltips
 *	 - adjusted variable accessability
 *	 - refactored
 *
 * 	Author: Andre Spittel
 *  Description:
 * 	- added Singleton behavior
 * 	
 * 	Changes: Noah Stolz
 * 	Added oneliner support
 */
[RequireComponent(typeof(AudioSource))]
public class DialogSystem : MonoBehaviour {
	[SerializeField]
	private PlayerManager player;
    [Tooltip("Reference to player settings to ask if dialog-subtitles should be displayed in english or german")]
	[SerializeField]
	private Settings playerSettings;

	// the text object in which the subtitles are displayed
	[SerializeField]
	private TextMeshProUGUI dialogSubtitles;
	// should the text be displayed in german?
	[Tooltip("Should the Dialog be in german or english?")]
    [SerializeField]
	private bool isEnglish;
    [Tooltip("Should subtitles be displayed?")]
	// audio source
	private AudioSource audioSource;
	// for loading the needed audio source
    [SerializeField]
	private AudioClip audioClip;
	// path to the dialog audio files
	[Tooltip("Path to audio files. Mostly shouldn't be changed!")]
	[SerializeField]
    private string audioPath = "Audio/Dialog/";
    [Tooltip("Path to oneliner audio files")]
    [SerializeField]
    private string onelinerAudioPath = "Audio/Oneliner/";

	// get subtitles from here
	[HideInInspector]
	public DialogTextFromExcel dialogTextFromExcel;
    // bool to check if a dialog is playing
    [HideInInspector]
    public bool dialogPlaying = false;

	// TODO: use dialog Queue & Callbacks instead of directly playing audio
    [HideInInspector]
    public ArrayList dialogQueue;

	//------------------------------------------------------------------------------------------------------------------
	// Singleton format, if unclear please google Singleton

	public static DialogSystem instance = null;

	void Start() {
		if (!player.photonView.IsMine) {
			Destroy(this);
			return;
		}

		// Check if instance already exists
		if (instance == null) {
			// if not, set instance to this
			instance = this;
		}

		// If instance already exists and it's not this:
		else if (instance != this) {
			// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);
			Debug.LogError("Additional Dialog System was found! Only one Dialog System should exist and it should be part of the player.");
		}

        dialogQueue = new ArrayList();

		audioSource = GetComponent<AudioSource>();
        audioClip = audioSource.clip;
		dialogTextFromExcel = new DialogTextFromExcel();

		dialogSubtitles.enabled = false;

        isEnglish = playerSettings.isEnglish;
    }

    void Update() {
        if(dialogQueue.Count > 0 && !dialogPlaying) {

            //check if parameter is numeric ( = onelinerID)
            int n;
            bool isNumeric = int.TryParse(dialogQueue[0].ToString(), out n);
          
            if (isNumeric) {
                StartOneLiner(dialogQueue[0].ToString());
            } else {
                StartDialog(dialogQueue[0].ToString());
            }

			dialogQueue.RemoveAt(0);
		}
    }

    // call this method from another gameobject/class to start the dialog routine
    /// <param name="filename">The name of the csv-File (without .csv file extension!)</param>
    public void StartDialog(string filename) {
		StartCoroutine(PlayDialog(filename));
	}

    public void StartOneLiner(string id) {
        StartCoroutine(PlayOneLiner(id));
    }

	IEnumerator PlayDialog(string filename) {
		// get values from csv-file
		dialogTextFromExcel.GetValues(filename);
		//enable text-object, if they should be displayed
        if(playerSettings.displaySubtitles)
		    dialogSubtitles.enabled = true;
        // set dialogPlaying-flag to true
        dialogPlaying = true;

		for (int i = 0; i < dialogTextFromExcel.timeToDisplay.Count; i++) {
			// if subtitles should be english play english dialog, otherwise play german dialog
			if (isEnglish) {
                
                PlayeOneShot(dialogTextFromExcel.englishSubtitles[i], audioPath + dialogTextFromExcel.germanAudio[i]);

            } else {
                
                PlayeOneShot(dialogTextFromExcel.germanSubtitles[i], audioPath + dialogTextFromExcel.germanAudio[i]);
			}
			// wait for the set number of seconds (in the csv-file) before displaying the next bit of text
			yield return new WaitForSeconds(dialogTextFromExcel.timeToDisplay[i]);
		}
		// after the dialog is done, disable text-object
		dialogSubtitles.enabled = false;
        // set dialogPlaying-flag to false
        dialogPlaying = false;
        // clear values
        dialogTextFromExcel.ClearData();

		// remove first element in queue
		//dialogQueue.RemoveAt(0);
	}

    /// <summary>Play a one Liner in german or english</summary>
    /// <param name="ID">The ID of the oneliner</param>
    IEnumerator PlayOneLiner(string ID) {

		// Enable text-object that displays subtitles, if they should be displayed
        if(playerSettings.displaySubtitles)
		    dialogSubtitles.enabled = true;
        // set dialogPlaying-flag to true
        dialogPlaying = true;

        dialogTextFromExcel.GetOneLinerValues();

		// get the index of the id
		int i = dialogTextFromExcel.oneLinerID.IndexOf(ID);

		{
			// if subtitles should be english play english dialog, otherwise play german dialog
			if (isEnglish) {
				PlayeOneShot(dialogTextFromExcel.oneLinerEnglishSubtitles[i], onelinerAudioPath + dialogTextFromExcel.oneLinerEnglishAudio[i]);
			} else {
				PlayeOneShot(dialogTextFromExcel.oneLinerGermanSubtitles[i], onelinerAudioPath + dialogTextFromExcel.oneLinerGermanAudio[i]);
			}
            yield return new WaitForSeconds(dialogTextFromExcel.oneLinerPlayTimer[i]);
		}
		// after the dialog is done, disable text-object
		dialogSubtitles.enabled = false;
        // set dialogPlaying-flag to false
        dialogPlaying = false;
        // remove first element in queue
        dialogQueue.RemoveAt(0);
	}

	private void PlayeOneShot(string subtitleText, string audioFilePath) {
		// stop audio to prevent multiple files playing simultaneously
		if (audioSource.isPlaying) {
			audioSource.Stop();
		}

		// display subtitles
		dialogSubtitles.text = subtitleText;

		// load the file from folder resources
		audioClip = Resources.Load<AudioClip>(audioFilePath);

		// check if audio file is available
		if (audioClip != null) {
			// play the audio file
			audioSource.PlayOneShot(audioClip, 1.0F);
		} else {
			Debug.LogWarning("Audio file not found! " + audioFilePath);
		}
	}
}
