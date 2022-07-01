// Author: Noah Stolz
// Used to give GameObjects text and images that can be collected by CollectContent
// Should be attached to the Object that the Player needs to interact with

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class ObjectContent : Interactable {
	//The Player that interacted with the Object
	public ArmTool interactor { get; private set; }

	[SerializeField]
	private bool bothPlayers = true;

	[SerializeField]
	[Tooltip("The type of an object determines what tab of the logbook it is added to")]
	private string typeOfContent;

	[SerializeField]
	[Tooltip("The name of the logbook entry that will be activated by interacting with this object")]
	private string nameOfObject;

	public UnityEvent onInteract;

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
		SendCollectContentEvent(armTool.GetComponent<PhotonView>().ViewID);
	}

	private void SendCollectContentEvent(int playerId) {
		object[] content = new object[] { nameOfObject, typeOfContent, bothPlayers, playerId };
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
		PhotonNetwork.RaiseEvent((byte) EventCodes.CollectContent, content, raiseEventOptions, SendOptions.SendReliable);
	}
}