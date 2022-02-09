// Author: Noah Stolz
// Used to get text and images from GameObjects with ObjectContent
// Should be attached to the PlayerPrefab

using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CollectContent : MonoBehaviour {

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

	private ulong networkInstance;

	// The Dictionary that contains all logbook entries with the name if their gameobject
	private Dictionary<string, GameObject> logbookEntryDict = new Dictionary<string, GameObject>();
	private Dictionary<string, GameObject> logbookEntryButtonDict = new Dictionary<string, GameObject>();

	void Start() {
		networkInstance = player.GetComponent<NetworkObject>().NetworkObjectId;

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

	void OnEnable() {
		// Start listening for the event
		EventManager.instance.StartListening("collectContent", CollectCont);
	}

	void OnDisable() {
		// Stop listening when this script is disabled
		EventManager.instance.StopListening("collectContent", CollectCont);
	}


	/// <summary>Set a logbook entry to active based on the currentID of the ObjectContent class</summary>
	public void CollectCont() {
		if (!ObjectContent.currentBothPlayers) {
			if (networkInstance != ObjectContent.currentPlayerId) {
				//print("wrong player");
				return;
			}
		}

		// Can the entry be found inside the logbook?
		if (!logbookEntryDict.ContainsKey(ObjectContent.currentName) && !logbookEntryButtonDict.ContainsKey(ObjectContent.currentName)) {
			Debug.LogWarning("This entry does not exist in logbook: " + ObjectContent.currentName);
			return;
		}

		if (logbookEntryDict.ContainsKey(ObjectContent.currentName)) {
			GameObject entry = logbookEntryDict[ObjectContent.currentName];

			// TODO add special entries to the entryButton dictionary instead of normal entry dicionary
			foreach (KeyValuePair<string, GameObject> ent in logbookEntryDict) {
				ent.Value.SetActive(false);
			}

			entry.SetActive(true);
		}

		if (logbookEntryButtonDict.ContainsKey(ObjectContent.currentName)) {
			GameObject entryButton = logbookEntryButtonDict[ObjectContent.currentName];
			entryButton.SetActive(true);
		}

		if (networkInstance == ObjectContent.currentPlayerId && ObjectContent.currentType != "special") {
			logbookManager.openLogbook();
			logbookManager.EnableOnePanel(ObjectContent.currentType);
		}
	}
}
