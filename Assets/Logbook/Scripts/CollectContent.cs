// Author: Noah Stolz
// Used to get text and images from GameObjects with ObjectContent
// Should be attached to the PlayerPrefab

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectContent : MonoBehaviour, IOnEventCallback {
	[SerializeField]
	[Tooltip("The CanvasController attached to the logbook")]
	private LogbookManager logbookManager;

	[SerializeField]
	[Tooltip("The panels that the entries are children of")]
	private GameObject[] EntryParents;

	[SerializeField]
	[Tooltip("The panels that the buttons are children of")]
	private GameObject[] EntryButtonParents;

	[SerializeField]
	[Tooltip("The player")]
	private GameObject player;

	public bool bothPlayers = true;

	private int networkInstance;

	// The Dictionary that contains all logbook entries with the name if their gameobject
	private Dictionary<string, GameObject> logbookEntryDict = new Dictionary<string, GameObject>();
	private Dictionary<string, GameObject> logbookEntryButtonDict = new Dictionary<string, GameObject>();

	void Start() {
		networkInstance = player.GetPhotonView().ViewID;

		// Get all the Entries
		for (int i = 0; i < EntryParents.Length; i++) {
			foreach (Transform child in EntryParents[i].transform) {
				if (!logbookEntryDict.ContainsKey(child.name))
					logbookEntryDict.Add(child.name, child.gameObject);
			}
		}

		// Get all the Entry-buttons
		for (int i = 0; i < EntryButtonParents.Length; i++) {
			foreach (Transform child in EntryButtonParents[i].transform) {
				if (!logbookEntryButtonDict.ContainsKey(child.name))
					logbookEntryButtonDict.Add(child.name, child.gameObject);
			}
		}
	}

	private void OnEnable() {
		PhotonNetwork.AddCallbackTarget(this);
	}

	private void OnDisable() {
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public void OnEvent(EventData photonEvent) {
		if (photonEvent.Code == (byte) EventCodes.CollectContent) {
			object[] data = (object[])photonEvent.CustomData;
			CollectCont((string) data[0], (string) data[1], (bool) data[2], (int) data[3]);
		}
	}


	/// <summary>Set a logbook entry to active based on the currentID of the ObjectContent class</summary>
	public void CollectCont(string objectName, string objectType, bool bothPlayers, int playerId) {
		if (!bothPlayers) {
			if (networkInstance != playerId) {
				Debug.LogWarning("wrong player");
				return;
			}
		}

		Debug.Log("Collecting: " + objectName);

		// Can the entry be found inside the logbook?
		if (!logbookEntryDict.ContainsKey(objectName) || !logbookEntryButtonDict.ContainsKey(objectName)) {
			Debug.LogWarning("This entry does not exist in logbook: " + objectName);
			return;
		}

		GameObject entry = logbookEntryDict[objectName];

		foreach (KeyValuePair<string, GameObject> ent in logbookEntryDict) {
			ent.Value.SetActive(false);
		}

		StartCoroutine(OpenLogbookWhenCollectionContent(entry, objectType, objectName));
	}

	private IEnumerator OpenLogbookWhenCollectionContent(GameObject entry, string objectType, string objectName) {
		yield return logbookManager.OpenLogbookRoutine();

		logbookManager.EnableOnePanel(objectType);

		entry.SetActive(true);

		GameObject entryButton = logbookEntryButtonDict[objectName];
		entryButton.SetActive(true);
	}
}