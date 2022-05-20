using ExitGames.Client.Photon;
using Photon.Pun;
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
	
	//private DialogSystem listener;
	
	private GameObject[] players;
	//------------------------------------------------------------------------------------------------------------------

	private void Start() {
		//listener = GameObject.Find("DialogManager").GetComponent<DialogSystem>();
	}

	private void OnEnable() {
		PhotonNetwork.AddCallbackTarget(this);
	}

	private void OnDisable() {
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public void OnEvent(EventData photonEvent) {
		BeginResonance();
	}

	// A function to express the whole sequence of the resonance.
	private void BeginResonance() {
	    EnableArea();
	    
	    players = GetPlayers();
	    oldPlayersPositions = GetPlayersPositions(players);
	    
        // If this player is the one, who activated the resonance, set "activated" to true
	    if (resonanceTrigger.rts != null) {
		    resonanceTrigger.rts.activated = true;
	    } else {
		    Debug.Log("Warning: No Reference to the ResonanceTestScript found.");
	    }

        // Get the current positions of the players, so we can set them back to default at the end of the resonance
		
		TeleportPlayers(players, spawnPoint.transform.position);
	    
        // We need a Coroutine, so we can stop the function until the player gave an input.
		StartCoroutine(DialoguesStage());
	}

	private IEnumerator DialoguesStage() {
		
		yield return new WaitForSeconds(waitTimeAfterTeleportation);

        for (int i = 0; i < sceneryList.Count; i++)
        {
            if (!sceneryList[i].isDecision)
            {
                // Normal scenery

                sceneryList[i].gameObject.SetActive(true);
                yield return new WaitUntil(() => sceneryList[i].isTriggered);

                DialogSystem.instance.StartDialog(sceneryList[i].dialogueFileName);

                yield return new WaitForSeconds(DialogSystem.instance.dialogTextFromExcel.GetTimeToDisplay() + 1f);

                foreach (var mat in sceneryList[i].materials)
                {
                    StartCoroutine(sceneryList[i].Fade(mat));
                }

                yield return new WaitUntil(() => sceneryList[i].faded);

                sceneryList[i].gameObject.SetActive(false);
            }
            else
            {
                sceneryList[i].gameObject.SetActive(true);
                sceneryList[i + 1].gameObject.SetActive(true);

                yield return new WaitUntil((
                    () => sceneryList[i].isTriggered || sceneryList[i + 1].isTriggered));

                if (sceneryList[i].isTriggered)
                {
	                foreach (var mat in sceneryList[i + 1].materials)
	                {
		                StartCoroutine(sceneryList[i + 1].Fade(mat));
	                }
	                
	                yield return new WaitUntil(() => sceneryList[i + 1].faded);
	                
	                sceneryList[i + 1].gameObject.SetActive(false);
	                
                    DialogSystem.instance.StartDialog(sceneryList[i].dialogueFileName);
                    yield return new WaitForSeconds(DialogSystem.instance.dialogTextFromExcel.GetTimeToDisplay() + 1f);

                    foreach (var mat in sceneryList[i].materials)
                    {
                        StartCoroutine(sceneryList[i].Fade(mat));
                    }

                    yield return new WaitUntil(() => sceneryList[i].faded);

                    sceneryList[i].gameObject.SetActive(false);
                }
                else
                {
	                foreach (var mat in sceneryList[i].materials)
	                {
		                StartCoroutine(sceneryList[i].Fade(mat));
	                }
	                
	                yield return new WaitUntil(() => sceneryList[i].faded);
	                
	                sceneryList[i].gameObject.SetActive(false);
	                
                    DialogSystem.instance.StartDialog(sceneryList[i + 1].dialogueFileName);
                    yield return new WaitForSeconds(DialogSystem.instance.dialogTextFromExcel.GetTimeToDisplay() + 1f);

                    foreach (var mat in sceneryList[i + 1].materials)
                    {
                        StartCoroutine(sceneryList[i + 1].Fade(mat));
                    }

                    yield return new WaitUntil(() => sceneryList[i + 1].faded);

                    sceneryList[i + 1].gameObject.SetActive(false);
                }
                i += 1;
            }
        }
        // Fade to Black
		resonanceTrigger.image.GetComponent<Animation>().Play();
        yield return new WaitUntil(() => resonanceTrigger.image.faded);
        
		// Teleport the players back and set their speed to normal
		TeleportPlayers(players, oldPlayersPositions);
		
		// Destroy the ResonanceTrigger and the ResonanceIllusion
		EventManager.instance.StopListening("ResonanceTrigger", BeginResonance);
		gameObject.SetActive(false);
		resonanceTrigger.gameObject.SetActive(false);

        if (dialogAfterResonance) {
            dialogAfterResonance.StartDialog();
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
	
	private GameObject[] GetPlayers() {
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		return allPlayers;
	}

	private void TeleportPlayers(GameObject[] _players, Vector3 destination) {
		for (int i = 0; i < _players.Length; i++) {
			_players[i].transform.position = destination;
		}
	}
	
	private void TeleportPlayers(GameObject[] _players, Vector3[] destination) {
		for (int i = 0; i < _players.Length; i++) {
			_players[i].transform.position = destination[i];
		}
	}
	
}
