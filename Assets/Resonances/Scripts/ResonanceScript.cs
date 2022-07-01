using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * by Andre Spittel 15.10.2018
 * updated by Andre Spittel 30.10.2018 with help from Felix Werner
 * ---------------------------------------------------------------------------------------------------------------------
 * A script to simulate the behavior of a resonance in our game.
 * ---------------------------------------------------------------------------------------------------------------------
 * Place this onto the object, that wants to trigger a resonance.
 * 
 * Known Issues:
 * 		-since there is no way of getting the length of the dialogues, i cant make the Dialogue B or C play out without
 * 		 teleporting them back instantly
 * 		-cant show Dialogue B or C without showing A
 * 		-Dialogues wont show up on a builded scene
 */

public class ResonanceScript : MonoBehaviour {

	[SerializeField] private Transform spawnPoint;

	[Header("Which Resonance should be activated?")]
	[Tooltip("Drag in the prefab of the ResonanceIllusion")]
	[SerializeField] private ResonanceTriggerScript resonanceTrigger;

	[SerializeField]
	private List<ResonanceScenery> sceneryList;

	[SerializeField]
	[Tooltip("After the players teleported, how long is the break")]
	private float waitTimeAfterTeleportation = 1.5f;

	[SerializeField]
	[Tooltip("The Dialog that is played, after the resonance is over")]
	private DialogEvent dialogAfterResonance;

	// The places where the players are, when they activate the resonance
	private Vector3[] oldPlayersPositions;
	private Quaternion[] oldPlayersRotations;

	private GameObject[] players;

	private bool shouldStopDialog = false;
	//------------------------------------------------------------------------------------------------------------------

	// A function to express the whole sequence of the resonance.
	public void BeginResonance() {
		EnableArea();

		players = GetPlayers();
		oldPlayersPositions = GetPlayersPositions(players);
		oldPlayersRotations = GetPlayersRotations(players);

		// Get the current positions of the players, so we can set them back to default at the end of the resonance
		TeleportPlayers(players, spawnPoint.transform.position, spawnPoint.transform.rotation);

		// We need a Coroutine, so we can stop the function until the player gave an input.
		StartCoroutine(DialoguesStage());
	}

	private IEnumerator DialoguesStage() {
		yield return new WaitForSeconds(waitTimeAfterTeleportation);

		for (int i = 0; i < sceneryList.Count; i++) {
			if (!sceneryList[i].isDecision) {
				// Normal scenery
				sceneryList[i].gameObject.SetActive(true);
				StartCoroutine(sceneryList[i].FadeIn());
				yield return new WaitUntil(() => sceneryList[i].isTriggered);

				yield return PlayScenery(i);
			} else {
				// display both options
				sceneryList[i].gameObject.SetActive(true);
				sceneryList[i + 1].gameObject.SetActive(true);

				StartCoroutine(sceneryList[i].FadeIn());
				StartCoroutine(sceneryList[i + 1].FadeIn());

				// wait until player triggers an option
				yield return new WaitUntil((
					() => sceneryList[i].isTriggered || sceneryList[i + 1].isTriggered));

				if (sceneryList[i].isTriggered) {
					StartCoroutine(sceneryList[i + 1].FadeOut());

					yield return PlayScenery(i);
				} else {
					StartCoroutine(sceneryList[i].FadeOut());

					yield return PlayScenery(i+1);
				}
				i += 1;
			}
		}
		// Fade to Black
		resonanceTrigger.image.GetComponent<Animation>().Play();
		yield return new WaitUntil(() => resonanceTrigger.image.faded);

		// Teleport the players back and set their speed to normal
		TeleportPlayers(players, oldPlayersPositions, oldPlayersRotations);

		if (dialogAfterResonance) {
			dialogAfterResonance.StartDialog();
		}
	}

	private IEnumerator PlayScenery(int i){
		DialogSystem.instance.StartDialog(sceneryList[i].dialogueFileName);
		yield return WaitUntilDialogFinished(DialogSystem.instance.dialogTextFromExcel.GetTimeToDisplay());

		yield return sceneryList[i].FadeOut();
	}

	private IEnumerator WaitUntilDialogFinished(float dialogTime) {
		for (float timer = dialogTime; timer >= 0; timer -= Time.deltaTime){
			if (shouldStopDialog){
				shouldStopDialog = false;
				DialogSystem.instance.StopCurrentDialogOrOneLiner();
				yield break;
			} else {
				yield return null;
			}
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			shouldStopDialog = true;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	//	Helping Functions
	//------------------------------------------------------------------------------------------------------------------

	private void EnableArea() {
		GameObject tmp = transform.Find("Area").gameObject;

		if (tmp) {
			tmp.SetActive(true);
		} else {
			Debug.LogError(name + " has no ChildGameObject with Name: Area. Someone changed the name, please change it back to Area.");
		}
	}

	private Vector3[] GetPlayersPositions(GameObject[] _players) {
		Vector3[] tmp = new Vector3[_players.Length];

		for (int i = 0; i < _players.Length; i++) {
			tmp[i] = _players[i].transform.position;
		}
		return tmp;
	}

	private Quaternion[] GetPlayersRotations(GameObject[] _players) {
		Quaternion[] tmp = new Quaternion[_players.Length];

		for (int i = 0; i < _players.Length; i++) {
			tmp[i] = _players[i].transform.rotation;
		}
		return tmp;
	}

	private GameObject[] GetPlayers() {
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		return allPlayers;
	}

	private void TeleportPlayers(GameObject[] _players, Vector3 destination, Quaternion rotation) {
		for (int i = 0; i < _players.Length; i++) {
			_players[i].transform.position = destination;
			_players[i].transform.rotation = rotation;
		}
	}

	private void TeleportPlayers(GameObject[] _players, Vector3[] destinations, Quaternion[] rotations) {
		for (int i = 0; i < _players.Length; i++) {
			_players[i].transform.position = destinations[i];
			_players[i].transform.rotation = rotations[i];
		}
	}
}
