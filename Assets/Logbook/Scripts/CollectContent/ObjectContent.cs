// Author: Noah Stolz
// Used to give GameObjects text and images that can be collected by CollectContent
// Should be attached to the Object that the Player needs to interact with

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class ObjectContent : Interactable {
	// The id of the object the player last interacted with
	public static string currentName;
	// The type of the object the player last interacted with
	public static ContentType currentType;
	// The index of the player who last interacted with an object
	public static int currentPlayerId { get; private set; }

	public static bool currentBothPlayers;

	//The Player that interacted with the Object
	public ArmTool interactor { get; private set; }

	[SerializeField]
	private bool bothPlayers = true;

	[SerializeField]
	[Tooltip("The type of an object determines what tab of the logbook it is added to")]
	private ContentType typeOfContent;

	[SerializeField]
	[Tooltip("The name of the logbook entry that will be activated by interacting with this object")]
	private string nameOfObject;


	public UnityEvent onInteract;

	private bool collected = false;

	/// <summary>Calls the collectContent event when the player interacts with the object this component is attached to/summary>
	public override void Interact(ArmTool armTool) {
		if (onlyExecuteLocally) {
			if (!armTool.photonView.IsMine){
				return;
			}
		}

		if (collected) {
			return;
		}

		collected = true;

		interactor = armTool;

		onInteract.Invoke();

		currentPlayerId = armTool.GetComponent<PhotonView>().ViewID;
		currentBothPlayers = bothPlayers;
		currentName = nameOfObject;
		currentType = typeOfContent;

		SendCollectContentEvent();
	}

	// If you have multiple custom events, it is recommended to define them in the used class
	public const byte CollectContentEventCode = 1;

	private void SendCollectContentEvent() {
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
		PhotonNetwork.RaiseEvent(CollectContentEventCode, null, raiseEventOptions, SendOptions.SendReliable);
	}
}