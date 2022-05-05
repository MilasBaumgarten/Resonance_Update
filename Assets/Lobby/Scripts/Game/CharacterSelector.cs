using Unity.Netcode;
using UnityEngine;
using Unity.Collections;

public class CharacterSelector : NetworkBehaviour {
	[SerializeField]
	private GameObject catriona;
	[SerializeField]
	private GameObject robert;

	public NetworkVariable<FixedString64Bytes> selectedCharacter = new NetworkVariable<FixedString64Bytes>("");

	[ServerRpc(RequireOwnership = false)]
	public void SetSelectedCharacterServerRPC(string character) {
		selectedCharacter.Value = character;
	}

	private void OnCharacterSelected(FixedString64Bytes oldCharacter, FixedString64Bytes newCharacter) {
		// return when this is only server
		if (!IsClient) { return; }

		switch (newCharacter.ToString()) {
			case "CATRIONA":
				catriona.SetActive(true);
				robert.SetActive(false);
				break;
			case "ROBERT":
				catriona.SetActive(false);
				robert.SetActive(true);
				break;
			default:
				Debug.Log($"character string not set or unknown: {newCharacter}");
				break;
		}
	}

	private void OnEnable() {
		selectedCharacter.OnValueChanged += OnCharacterSelected;
	}

	private void OnDisable() {
		selectedCharacter.OnValueChanged = OnCharacterSelected;
	}
}
