using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class Launcher : MonoBehaviourPunCallbacks {
	/// <summary>
	/// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
	/// </summary>
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	[SerializeField]
	private byte maxPlayersPerRoom = 2;

	[SerializeField]
	private string lobbyScene;

	/// <summary>
	/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
	/// </summary>
	[SerializeField]
	private string gameVersion = "0.0.1";

	[Header("UI")]
	[SerializeField]
	private GameObject lobbyPanel;
	[Tooltip("The Ui Panel to let the user enter name, connect and play")]
	[SerializeField]
	private GameObject controlPanel;
	[Tooltip("The UI Label to inform the user that the connection is in progress")]
	[SerializeField]
	private GameObject progressLabel;
	[SerializeField]
	private TMP_InputField roomNameInputField;

	[Header("Character Selection")]
	[SerializeField]
	private Button catrionaButton;
	[SerializeField]
	private TMP_Text catrionaSelectorTextHost;
	[SerializeField]
	private TMP_Text catrionaSelectorTextClient;
	[SerializeField]
	private TMP_Text robertSelectorTextHost;
	[SerializeField]
	private TMP_Text robertSelectorTextClient;

	[Header("Room List")]
	[SerializeField]
	private Transform roomListParent;
	[SerializeField]
	private GameObject roomListPanel;
	[SerializeField]
	private GameObject roomJoinedClientPanel;
	[SerializeField]
	private GameObject roomPrefab;

	private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
	private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
	private string roomName = null;

	void Awake() {
		// #Critical
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	void Start() {
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
	}

	public void OpenLobbyPanel() {
		if (!PhotonNetwork.IsConnected) {
			// #Critical, we must first and foremost connect to Photon Online Server.
			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
		} else {
			PhotonNetwork.JoinLobby(customLobby);
			lobbyPanel.SetActive(true);
		}
	}

	public void LeaveLobby() {
		if (PhotonNetwork.InRoom) {
			PhotonNetwork.LeaveRoom();
		}
		PhotonNetwork.LeaveLobby();
	}

	public void SetRoomName(string newRoomName) {
		roomName = newRoomName;
	}

	public void CreateRoom() {
		RoomOptions roomOptions = new RoomOptions();
		PhotonNetwork.CreateRoom(roomName, roomOptions, null);
	}

	public void LeaveRoom() {
		PhotonNetwork.LeaveRoom();
	}

	public override void OnCreateRoomFailed(short returnCode, string message) {
		Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
	}

	public override void OnCreatedRoom() {
		Debug.Log("Successfully created a Room.");
		SelectNickname("CATRIONA");
		catrionaButton.Select();
	}

	private void UpdateCachedRoomList(List<RoomInfo> roomList) {
		for (int i = 0; i < roomList.Count; i++) {
			RoomInfo info = roomList[i];
			if (info.RemovedFromList) {
				cachedRoomList.Remove(info.Name);
			} else {
				cachedRoomList[info.Name] = info;
			}

			GameObject room = Instantiate(roomPrefab, roomListParent);
			room.GetComponent<RoomUI>().Setup(info.Name, roomListPanel, roomJoinedClientPanel);
			Debug.Log("Room Name: " + info.Name);
		}
	}

	public void StartGame() {
		progressLabel.SetActive(true);
		controlPanel.SetActive(false);

		LoadingScreen.SetState(true);

		// Load the Room Level.
		PhotonNetwork.LoadLevel(lobbyScene);
	}

	public void SelectNickname(string value) {
		PhotonNetwork.NickName = value;

		if (value == CharacterEnum.CATRIONA.ToString()) {
			catrionaSelectorTextHost.text = "Host";

			photonView.RPC("SetNicknameRpc", RpcTarget.OthersBuffered, CharacterEnum.ROBERT.ToString());

			if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
				robertSelectorTextHost.text = "Client";
			} else {
				robertSelectorTextHost.text = "";
			}
		} else {
			robertSelectorTextHost.text = "Host";

			photonView.RPC("SetNicknameRpc", RpcTarget.OthersBuffered, CharacterEnum.CATRIONA.ToString());

			if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
				catrionaSelectorTextHost.text = "Client";
			} else {
				catrionaSelectorTextHost.text = "";
			}
		}
	}

	public override void OnConnectedToMaster() {
		Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

		if (!PhotonNetwork.InLobby) {
			PhotonNetwork.JoinLobby(customLobby);
			lobbyPanel.SetActive(true);
		}
	}

	public override void OnJoinedRoom() {
		Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
	}

	public override void OnDisconnected(DisconnectCause cause) {
		cachedRoomList.Clear();
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
		Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
	}

	public override void OnJoinedLobby() {
		Debug.Log("Joined Lobby");
		cachedRoomList.Clear();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList) {
		UpdateCachedRoomList(roomList);
	}

	public override void OnLeftLobby() {
		cachedRoomList.Clear();
	}

	[PunRPC]
	public void SetNicknameRpc(string nickname) {
		PhotonNetwork.NickName = nickname;

		if (nickname == CharacterEnum.CATRIONA.ToString()) {
			catrionaSelectorTextClient.text = "Client";
			robertSelectorTextClient.text = "Host";
		} else {
			catrionaSelectorTextClient.text = "Host";
			robertSelectorTextClient.text = "Client";
		}
	}

	// update UI when player leaves or enters the room
	public override void OnPlayerEnteredRoom(Player other) {
		if (PhotonNetwork.NickName == CharacterEnum.CATRIONA.ToString()) {
			robertSelectorTextHost.text = "Client";
		} else {
			catrionaSelectorTextHost.text = "Client";
		}
	}

	public override void OnPlayerLeftRoom(Player other) {
		if (PhotonNetwork.NickName == CharacterEnum.CATRIONA.ToString()) {
			robertSelectorTextHost.text = "";
		} else {
			catrionaSelectorTextHost.text = "";
		}
	}
}