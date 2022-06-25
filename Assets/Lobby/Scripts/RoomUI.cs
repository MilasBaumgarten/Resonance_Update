using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomUI : MonoBehaviour {
	[SerializeField]
	private TMP_Text textAsset;

	private string roomName;
	private GameObject lobbyRoom;
	private GameObject clientRoom;

	public void Setup(string roomName, GameObject lobbyRoom, GameObject clientRoom) {
		this.roomName = roomName;
		textAsset.text = roomName;
		this.lobbyRoom = lobbyRoom;
		this.clientRoom = clientRoom;
	}

	public void Join() {
		PhotonNetwork.JoinRoom(roomName);
		clientRoom.SetActive(true);
		lobbyRoom.SetActive(false);
	}
}
