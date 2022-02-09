using System.Collections;
using UnityEngine;

public class UnLockPlayer : MonoBehaviour {
	[SerializeField] private string playerTag = "Player";

	private GameObject[] players;

	void Start() {
		players = GameObject.FindGameObjectsWithTag(playerTag);
	}

	public void LockPlayer() {
		foreach (GameObject player in players) {
			player.GetComponent<Animator>().SetBool("LockPlayer", true);
		}
	}

	public void UnlockPlayer() {
		foreach (GameObject player in players) {
			player.GetComponent<Animator>().SetBool("UnlockPlayer", true);
		}
	}

	// reset UnLockPlayer variables in animator after 1 Frame
	public void ResetLock() {

		foreach (GameObject player in players) {
			player.GetComponent<Animator>().SetBool("UnlockPlayer", false);
			player.GetComponent<Animator>().SetBool("LockPlayer", false);
		}
	}
}
