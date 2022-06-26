using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class GameManager_PUN : MonoBehaviourPunCallbacks {
	[Tooltip("The prefab to use for representing the player")]
	[SerializeField]
	private GameObject playerPrefab;

	[SerializeField]
	private string gameScene;

	public Dictionary<string, GameObject> spawnPoints;

	private void Start() {
		DontDestroyOnLoad(this);
	}

	public override void OnEnable() {
		base.OnEnable();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDisable() {
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode) {
		// ignore credits scene
		if (scene.name.ToLower().Contains("credits")) {
			return;
		}

		if (PhotonNetwork.OfflineMode || !PhotonNetwork.IsConnected) {
			return;
		}

		if (playerPrefab == null) {
			Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
		} else {
			if (PlayerManager.localPlayerInstance == null) {
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
			} else {
				Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
			}
		}
	}

	public void LeaveRoom() {
		PhotonNetwork.LeaveRoom();
	}

	/// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom() {
		SceneManager.LoadScene(0);
	}

	public override void OnPlayerEnteredRoom(Player other) {
		Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
		if (PhotonNetwork.IsMasterClient) {
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
		}
	}

	public override void OnPlayerLeftRoom(Player other) {
		Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
		if (PhotonNetwork.IsMasterClient) {
			Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
		}
	}
}