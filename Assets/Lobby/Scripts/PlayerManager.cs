using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerManager : NetworkBehaviour {
	[SerializeField]
	private string spawnerTag = "SpawnPoint";
	[SerializeField]
	private string lobbySceneName;
	[SerializeField]
	private GameObject playerPrefab;

	[HideInInspector]
	public Player player;

	public static PlayerManager Instance { get; private set; }

	private void Start() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(this);
		}
	}

	public void Init() {
		NetworkManager.Singleton.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
	}

	public void Cleanup() {
		NetworkManager.Singleton.SceneManager.OnLoadComplete -= SceneManager_OnLoadComplete;
	}

	private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode) {
		if (!IsOwner) {
			return;
		}

		if (sceneName == lobbySceneName) {
			return;
		}

		string characterName = "CATRIONA";

		if (player.Data.ContainsKey("Character")) {
			characterName = player.Data["Character"].Value;
			Debug.Log("Character set to: " + characterName);
		} else {
			Debug.LogError("Character name was not set!");
		}

		SpawnPlayerServerRPC(clientId, characterName);
	}

	[ServerRpc]
	private void SpawnPlayerServerRPC(ulong clientId, string characterName) {
		// instantiate player Prefab
		GameObject newPlayer = Instantiate(playerPrefab);
		NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
		newPlayer.SetActive(true);
		netObj.SpawnAsPlayerObject(clientId, false);

		SelectCharacterClientRPC(newPlayer.GetComponent<NetworkObject>(), characterName);

		Debug.Log("Player was spawned as: " + characterName);

		// teleport player to spawner
		// find spawnpoints
		GameObject[] spawners = GameObject.FindGameObjectsWithTag(spawnerTag);
		bool foundSpawner = false;

		// check for no spawners found
		if (spawners.Length == 0) {
			Debug.LogError("No Spawners could be found!");
		}

		// find spawn point for this player by name
		string playerName = characterName;

		foreach (GameObject g in spawners) {
			if (g.name.ToLower().Contains(playerName.ToLower())) {
				// teleport players to spawnpoints
				newPlayer.transform.position = g.transform.position;
				foundSpawner = true;

				Debug.Log("Player was teleported to their Spawner");
			}
		}

		if (!foundSpawner) {
			Debug.LogError("Spawner couldn't be found for " + playerName + "!");
		}
	}

	[ClientRpc]
	private void SelectCharacterClientRPC(NetworkObjectReference playerReference, string characterName) {
		if (playerReference.TryGet(out NetworkObject playerObject)) {
			playerObject.GetComponent<CharacterSelector>().SetSelectedCharacterServerRPC(characterName);
		} else {
			Debug.LogError("Player Reference " + playerReference + " not found on server!");
		}
		
	}
}
