using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/**
 * Author: Leon Ullrich
 * - Dialog-Event
 * - use this as UnityEvent in various triggers
 * - put this script on the object which triggers the dialog
 */

public class DialogEvent : MonoBehaviour {

	// the name of the csv-file (without .csv file-extension!)
	[Tooltip("The name of the csv-file (without .csv file-extension")]
	public string filename = "";
	// for showing dialog and playing audio just once
	private bool hasPlayed = false;

	[Tooltip("Is this a one liner or Dialog?")]
	public bool isOneLiner = false;

	[Tooltip("Used if you wish to play a one liner instead of a complete dialog")]
	public string oneLinerID;

	[SerializeField]
	private UnityEvent afterDialogFinishedEvent;

	public void StartDialog() {
		Play();
	}

	private void Play() {
		if (hasPlayed) {
			return;
		}

		DialogTextFromExcel dialogText = new DialogTextFromExcel();

		if (isOneLiner) {
			dialogText.GetOneLinerValues();

			if (dialogText.oneLinerID.Contains(oneLinerID)) {
				DialogSystem.instance.dialogQueue.Add(oneLinerID);
			} else {
				Debug.LogError("One-Liner ID not found! " + oneLinerID);
			}
		} else {
			dialogText.GetValues(filename);
			float dialogTime = dialogText.timeToDisplay.Sum();

			StartCoroutine(WaitForDialogFinish(dialogTime));
			DialogSystem.instance.StartDialog(filename); // called from script DialogTriggerListener
		}

		hasPlayed = true;
	}

	/// <summary>Used to play dialog or oneliners from outside of this script</summary>
	public void playDialog() {
		if (hasPlayed) {
			Play();
		} else {
			Play();
			hasPlayed = false;
		}
	}

	IEnumerator WaitForDialogFinish(float duration) {
		yield return new WaitForSeconds(duration);
		afterDialogFinishedEvent.Invoke();
	}
}