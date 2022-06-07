using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks {
	[SerializeField]
	private bool localDebugMode = false;

	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject localPlayerInstance;

	[SerializeField]
	private GameObject catrionaModel;
	[SerializeField]
	private GameObject robertModel;

	[SerializeField]
	private LogbookManager logbookCatriona;
	[SerializeField]
	private LogbookManager logbookRobert;

	public LogbookManager logbook { get; private set; }

	[SerializeField]
	private Animator catrionaAnimator;
	[SerializeField]
	private Animator robertAnimator;
	public Animator animator { get; private set; }

	[SerializeField]
	private Vector3 spawnOffset = new Vector3(0.0f, 0.6f, 0.0f);

	public string nickname { get; private set; }

	void Awake() {
		// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
		if (photonView.IsMine) {
			localPlayerInstance = gameObject;
		}

		DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += OnSceneLoaded;

		try {
			nickname = photonView.Owner.NickName;
		} catch (Exception e) {
			Debug.Log("Playing in Offline Mode and owner was not found fast enough.\n" + e.Message);
			nickname = CharacterEnum.CATRIONA.ToString();
		}

		SetupPlayer();
	}

	private void Start() {
		// setup offline mode for Debugging
		if (!PhotonNetwork.IsConnected) {
			Debug.Log("Using Photon in Offline Mode.");
			PhotonNetwork.OfflineMode = true;
			PhotonNetwork.NickName = CharacterEnum.CATRIONA.ToString();
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode) {
		// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
		if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
			transform.position = new Vector3(0f, 5f, 0f);
		}

		if (!localPlayerInstance && !PhotonNetwork.OfflineMode) {
			SetupPlayer();
		}

		if (!localDebugMode) {
			// move player to spawnpoint
			GameObject[] spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");

			foreach (GameObject spawner in spawners) {
				if (spawner.name.ToLower().Contains(nickname.ToLower())) {
					transform.position = spawner.transform.position + spawnOffset;
					break;
				}
			}
		}
	}

	private void SetupPlayer() {
		// set player visuals
		if (nickname.Equals(CharacterEnum.CATRIONA.ToString())) {
			catrionaModel.SetActive(true);
			robertModel.SetActive(false);
			logbook = logbookCatriona;
			animator = catrionaAnimator;
			Debug.Log("<Color=Green><a>Player</a></Color> set to Catriona.");
		} else if (nickname.Equals(CharacterEnum.ROBERT.ToString())) {
			catrionaModel.SetActive(false);
			robertModel.SetActive(true);
			logbook = logbookRobert;
			animator = robertAnimator;
			Debug.Log("<Color=Green><a>Player</a></Color> set to Robert.");
		} else {
			Debug.Log("<Color=Red><a>Player</a></Color> nickname: " + nickname + " is unknown.");
		}
	}

	public override void OnDisable() {
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public override void OnConnectedToMaster() {
		Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
		PhotonNetwork.CreateRoom("DebugRoom");
	}

	public override void OnJoinedRoom() {
		photonView.TransferOwnership(PhotonNetwork.PlayerList[0]);
		localPlayerInstance = gameObject;
	}
}
