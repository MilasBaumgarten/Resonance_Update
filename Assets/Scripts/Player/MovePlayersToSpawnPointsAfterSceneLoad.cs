using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/*
 * Script should lie on player
 * Author: Milas Baumgarten
 */
public class MovePlayersToSpawnPointsAfterSceneLoad : MonoBehaviour {

	[SerializeField] private UnityEvent onSceneLoaded;

	[SerializeField] private string spawnerTag = "SpawnPoint";
	[Tooltip("Player name in the character prefab and in the spawnpoints (can also be just a part like 'rob').")]
	[SerializeField] private string playerName = "rob";

	private GameObject[] spawners;

	// subscribe
	private void OnEnable() {
		SceneManager.sceneLoaded += SceneLoaded;
	}

	// unsubscribe
	private void OnDisable() {
		SceneManager.sceneLoaded -= SceneLoaded;
	}

	private void SceneLoaded(Scene scene, LoadSceneMode mode) {
		if (mode == LoadSceneMode.Single) {
			StartCoroutine(LoadDelayed());
		}
	}

	public void RespawnPlayers() {
		StartCoroutine(LoadDelayed());
	}

	private IEnumerator LoadDelayed() {
		// wait 60 frames before porting the player
		for (int i = 0; i < 60; i++) {
			yield return null;
		}

		onSceneLoaded.Invoke();

		// find spawnpoints
		spawners = GameObject.FindGameObjectsWithTag(spawnerTag);
		bool foundSpawner = false;

		// find spawn point for this player by name
		foreach (GameObject g in spawners) {

			if (g.name.ToLower().Contains(playerName.ToLower()) && gameObject.name.ToLower().Contains(playerName.ToLower())) {
				// teleport players to spawnpoints
				transform.position = g.transform.position;
				foundSpawner = true;

			}
		}

		if (!foundSpawner) {
			Debug.LogError("Spawner couldn't be found for " + playerName + "!");
		}

		// check for no spawners found
		if (spawners.Length == 0) {
			Debug.LogError("No Spawners could be found!");
		}
	}
}
