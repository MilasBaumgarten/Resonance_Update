// Author: Noah Stolz
// Used to give GameObjects text and images that can be collected by CollectContent
// Should be attached to the Object that the Player needs to interact with

using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class ObjectContent : Interactable {

	// The id of the object the player last interacted with
	public static string currentName;
	// The type of the object the player last interacted with
	public static string currentType;
	// The index of the logEntry child the player last interacted with
	//public static int currentLogIndex;
	// The index of the player who last interacted with an object
	public static ulong currentPlayerId;

	[HideInInspector]
	//The Player that interacted with the Object
	public GameObject player;

	public static bool currentBothPlayers;

	public bool bothPlayers = true;

	// An enum containing all possible types of content. Mostly for convenience and to make sure no wrong values are selected
	private enum contentType { text, audio, pictures, special/*,log*/}

	[SerializeField]
	[Tooltip("The type of an object determines what tab of the logbook it is added to")]
	private contentType typeOfContent;

	/*
    [SerializeField]
    [Tooltip("If the type is log: Set what part of the entry is activated by interacting with this object")]
    private int logIndex;*/

	[SerializeField]
	[Tooltip("The name of the logbook entry that will be activated by interacting with this object")]
	private string nameOfObject;

	private ulong networkInstance;

	public UnityEvent onInteract;

	private bool collected = false;

	public static int getPlayerNetworkId(GameObject player) {
		return (int)player.GetComponent<NetworkObject>().NetworkObjectId;
	}

	/// <summary>Calls the collectContent event when the player interacts with the object this component is attached to/summary>
	public override void Interact(ArmTool armTool) {
		if (collected) {
			return;
		}

		collected = true;

		networkInstance = armTool.gameObject.GetComponent<NetworkObject>().NetworkObjectId;

		player = armTool.gameObject;

		//print(networkInstance.Value);
		onInteract.Invoke();

		currentPlayerId = networkInstance;
		currentBothPlayers = bothPlayers;
		currentName = this.nameOfObject;
		currentType = typeOfContent.ToString().ToLower();
		//currentLogIndex = logIndex;

		EventManager.instance.TriggerEvent("collectContent");

	}
}