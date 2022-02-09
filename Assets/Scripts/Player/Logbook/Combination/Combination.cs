// Author: Noah Stolz
// Is anyone even still reading these?
// Should be attached to the player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.UI;

public class Combination : NetworkBehaviour {

	[SerializeField]
	private LogbookManager logbookManager;

	[SerializeField]
	[Tooltip("The parent Object of all combination panels")]
	private Transform combinationParent;

	[SerializeField]
	[Tooltip("How long the failure or success message should be shown")]
	private float displayResultTime = 1f;

	[SerializeField]
	[Tooltip("The text component of the UI Object that displays the remaining time")]
	Text timerText;

	private Dictionary<string, GameObject> combinations = new Dictionary<string, GameObject>();

	// The amount of time the player has to solve the combination
	private int time;
	private int startTime;

	private string[] correctCombination;

	private List<string> currentCombination = new List<string>();

	private bool checkAtCompletion;

	private bool combining = false;

	//private string nameOfCombination;

	private static Text timerTextStatic;

	private static Combination localInstance;

	private UnityEvent onSuccess;
	private UnityEvent onCorrectChoice;

	GameObject comObj;

	void Awake() {
		foreach (Transform child in combinationParent) {
			child.gameObject.SetActive(false);
			combinations.Add(child.name, child.gameObject);
		}
	}

	void Start() {
		if (IsLocalPlayer) {
			localInstance = this;
		}
	}

	public void InitCombination(string nameOfCombination, int time, string[] correctCombination, bool checkAt, GameObject comObj) {
		startTime = time;
		this.correctCombination = correctCombination;
		checkAtCompletion = checkAt;
		onSuccess = comObj.GetComponent<CombinationObject>().onSuccess;
		//this.nameOfCombination = nameOfCombination;

		this.comObj = comObj;
		disableAllCombinations();
		logbookManager.openLogbook();
		logbookManager.EnableOnePanel("combine");
		combinations[nameOfCombination].SetActive(true);
		combinations[nameOfCombination].transform.GetChild(0).gameObject.SetActive(true);

	}

	public void StartCombination() {
		//OnStartServerRpc(correctCombination, checkAtCompletion, startTime, comObj);
	}

	public void StartCountdown() {
		if (time > 0) {
			CancelInvoke("Countdown");
			InvokeRepeating("Countdown", 1f, 1f);
		}
	}

	public void nextChoice(string nameOfCombination, int indexOfChoice) {
		if (combining) {
			disableAllCombinations();
			logbookManager.openLogbook();
			logbookManager.EnableOnePanel("combine");
			combinations[nameOfCombination].SetActive(true);
			combinations[nameOfCombination].transform.GetChild(indexOfChoice).gameObject.SetActive(true);
		}
	}

	private void disableAllCombinations() {
		foreach (KeyValuePair<string, GameObject> values in combinations) {
			foreach (Transform child in values.Value.transform) {
				if (!child.name.Equals("TimeDisplay")) {
					child.gameObject.SetActive(false);
				}
			}
		}
	}

	#region Countdowns

	public void Countdown() {
		if (time > 0) {
			time--;
			timerText.text = (Mathf.FloorToInt(time / 60).ToString() + ":" + (time % 60).ToString());
			//CmdUpdateTimer(timerText.text);
		} else {
			OnFailServerRpc();
			CancelInvoke("Countdown");
		}
	}

	IEnumerator ResultDisplayTime() {
		yield return new WaitForSeconds(displayResultTime);
		logbookManager.DisablePanel();
		if (!checkAtCompletion) {
			MainProtocol.resetMainProtocol();
		}
	}

	#endregion

	/// <summary>Displays either a failure or success message when the Player has completed the combination</summary>
	/// <param name="success">Should a success or a failure message be shown?</param>
	private void ResultDisplay(bool success) {
		GameObject resultDisplay = combinations["combineResult"];

		foreach (KeyValuePair<string, GameObject> keyValue in combinations) {
			keyValue.Value.SetActive(false);
		}

		resultDisplay.SetActive(true);

		if (success) {
			resultDisplay.transform.GetChild(0).gameObject.SetActive(true);
			resultDisplay.transform.GetChild(1).gameObject.SetActive(false);
		} else {
			resultDisplay.transform.GetChild(1).gameObject.SetActive(true);
			resultDisplay.transform.GetChild(0).gameObject.SetActive(false);
		}

		StartCoroutine("ResultDisplayTime");
	}

	/// <summary>Adds a string to the currentCode list and compares it with the correctCode</summary>
	/// <param name="code">The string to be added</param>
	public void AddSelection(string code) {
		currentCombination.Add(code);

		if (!checkAtCompletion) {
			MainProtocol.addProtocolEntry();
		}

		if (checkAtCompletion && correctCombination.Length == currentCombination.Count) {
			if (checkForSuccess()) {
				OnSuccessServerRpc();
			} else {
				OnFailServerRpc();
			}

		} else if (correctCombination.Length == currentCombination.Count) {
			if (checkForSuccess()) {
				OnSuccessServerRpc();
			} else {
				OnFailServerRpc();
			}

		} else if (!checkAtCompletion) {
			if (!checkForSuccess()) {
				OnFailServerRpc();
			} else {
				logbookManager.DisablePanel();
			}
		}
	}

	/// <summary>Returns true if the currentCombination is the same as the correctCombination, only checks for the lenght of currentCombination</summary>
	public bool checkForSuccess() {
		bool success = true;

		for (int i = 0; i < currentCombination.Count; i++) {
			if (currentCombination[i] != correctCombination[i])
				success = false;
		}

		StopCoroutine("Countdown");

		return success;
	}

	public void GetText(Text text) {
		AddSelectionServerRpc(text.text);
	}

	public void Failed() {
		//print("faaaaail");
		CancelInvoke("Countdown");
		ResultDisplay(false);
		combining = false;
		clearCurrent();
	}

	private void clearCurrent() {
		currentCombination.Clear();
	}

	public void OnStart(string[] correctCombination, bool checkAtCompletion, int startTime, GameObject comObj) {
		this.correctCombination = correctCombination;
		this.checkAtCompletion = checkAtCompletion;
		this.onSuccess = comObj.GetComponent<CombinationObject>().onSuccess;
		this.startTime = startTime;
		time = startTime;

		if (!checkAtCompletion) {
			MainProtocol.resetMainProtocol();
			MainProtocol.addProtocolEntry();
		}

		//Combination.timerTextStatic = timerText;
		timerText.text = startTime.ToString();
		StartCountdown();
		combining = true;
		clearCurrent();
	}

	public void OnSuccess() {
		CancelInvoke("Countdown");
		ResultDisplay(true);
		onSuccess.Invoke();
		combining = false;
		clearCurrent();
	}

	[ServerRpc]
	void AddSelectionServerRpc(string code) {
		AddSelectionClientRpc(code);
	}

	[ClientRpc]
	void AddSelectionClientRpc(string code) {
		localInstance.AddSelection(code);
	}

	[ServerRpc]
	void OnFailServerRpc() {
		OnFailClientRpc();
	}

	[ClientRpc]
	void OnFailClientRpc() {
		localInstance.Failed();
	}

	[ServerRpc]
	void OnSuccessServerRpc() {
		OnSuccessClientRpc();
	}

	[ClientRpc]
	void OnSuccessClientRpc() {
		localInstance.OnSuccess();
	}

	// TODO use it somewhere
	//[ServerRpc]
	//void OnStartServerRpc(string[] correctCombination, bool checkAtCompletion, int startTime, GameObject comObj) {
	//	OnStartClientRpc(correctCombination, checkAtCompletion, startTime, comObj);
	//}

	//[ClientRpc]
	//void OnStartClientRpc(string[] correctCombination, bool checkAtCompletion, int startTime, GameObject comObj) {
	//	Combination.localInstance.OnStart(correctCombination, checkAtCompletion, startTime, comObj);
	//}
}
