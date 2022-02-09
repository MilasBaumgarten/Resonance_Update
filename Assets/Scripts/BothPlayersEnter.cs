// Author: Noah Stolz
// Triggers an event when both players enter the trigger
// Should be attached to the GameObject that has the trigger the players should enter

using UnityEngine;
using UnityEngine.Events;

public class BothPlayersEnter : MonoBehaviour {


    [SerializeField]
    [Tooltip ("The name of the event to be called")]
    private string nameOfEvent;

    public static GameObject player1;
    public static GameObject player2;

    public UnityEvent onBothPlayersEntered;

    //Is the first player already inside the trigger area?
    private bool player1Entered = false;

    void OnTriggerEnter(Collider other) {

        if (other.tag == "Player") {

            // If the first player is already inside the area, trigger the event
            if (player1Entered) {

                player2 = other.gameObject;
                onBothPlayersEntered.Invoke();

            }
            else
            {

                player1 = other.gameObject;
                player1Entered = true;
            }

        }

    }

    void OnTriggerExit(Collider other) {

        // If the first player exits the trigger area before the other player enters it
        if (other.tag == "Player") {

            player1Entered = false;

        }
    }
}
