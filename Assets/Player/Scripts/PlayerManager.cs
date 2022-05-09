using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPun {
	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject LocalPlayerInstance;

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

	void Awake() {
		// #Important
		// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			LocalPlayerInstance = gameObject;
		}
		// #Critical
		// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
		DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += OnSceneLoaded;

		SetupPlayer();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode) {
		CalledOnLevelWasLoaded(scene.buildIndex);
	}

	void CalledOnLevelWasLoaded(int level) {
		// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
		if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
			transform.position = new Vector3(0f, 5f, 0f);
		}

		SetupPlayer();
	}

	private void SetupPlayer() {
		// set player visuals
		string nickname = photonView.Owner.NickName;
		if (nickname.Equals(CharacterEnum.CATRIONA.ToString())) {
			catrionaModel.SetActive(true);
			robertModel.SetActive(false);
			logbook = logbookCatriona;
			animator = catrionaAnimator;
			Debug.LogWarning("<Color=Green><a>Player</a></Color> set to Catriona.");
		}
		else if (nickname.Equals(CharacterEnum.ROBERT.ToString())) {
			catrionaModel.SetActive(false);
			robertModel.SetActive(true);
			logbook = logbookRobert;
			animator = robertAnimator;
			Debug.LogWarning("<Color=Green><a>Player</a></Color> set to Robert.");
		}
		else {
			Debug.LogWarning("<Color=Red><a>Player</a></Color> nickname: " + nickname + " is unknown.");
		}

		// move player to spawnpoint
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");

		foreach (GameObject spawner in spawners) {
			if (spawner.name.ToLower().Contains(nickname.ToLower())) {
				transform.position = spawner.transform.position + spawnOffset;
				break;
			}
		}
	}

	public void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
