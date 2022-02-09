// Author: Noah Stolz
// Interact with this object to start hacking
// Attach to the object the player should interact with to start hacking

using UnityEngine;
using UnityEngine.Events;

public class HackableObject : Interactable
{
    /*[SerializeField]
    [Tooltip ("Decides what hacking minigame is goning to be started")]
    private typeOfHacking nameOfHackingGame;*/

    [SerializeField]
    [Tooltip("Decides what hacking minigame is goning to be started")]
    private string nameOfSuccessEvent;

    [SerializeField]
    private bool player1 = true;

    [SerializeField]
    private int time = 60;

    [SerializeField]
    private int difficulty = 3;

    [SerializeField]
    private int MasterMindTries = 6;

    [SerializeField]
    private int MasterMindColors = 4;

    [Tooltip("The event that is called when the correct code is entered")]
    public UnityEvent onSuccess;

    // Has this instance already been solved?
    private bool solved = false;

    //public enum typeOfHacking {hackingSwitchNummbers, hackingEvadeFlammes}

    void OnEnable()
    {

        // Start listening for the event
        EventManager.instance.StartListening(nameOfSuccessEvent, invokeOnSuccess);

    }

    void OnDisable()
    {

        // Stop listening when this script is disabled
        EventManager.instance.StopListening(nameOfSuccessEvent, invokeOnSuccess);

    }

    void invokeOnSuccess()
    {

        solved = true;
        onSuccess.Invoke();

    }

    public override void Interact(ArmTool armTool)
    {

        Hacking hacking = armTool.gameObject.GetComponent<Hacking>();

        if (/*Hacking.hackingName == "none" && !solved*/ player1)
        {

            //hacking.loadHacking("hackingEvadeFlammes", nameOfSuccessEvent);
            //hacking.evadeFlammesDifficulty = difficulty;
            //hacking.loadHacking("hackingSwitchNummbers", nameOfSuccessEvent);
            hacking.startTime = time;
            hacking.LoadHacking("hackingMasterMind", nameOfSuccessEvent, player1, difficulty, armTool.gameObject);
            hacking.hackingMasterMind.GetComponent<HackingMasterMind>().StartMasterMind(MasterMindTries, MasterMindColors);
            //hacking.loadingHacking = true;
        } else if (/*Hacking.hackingName == "hackingMasterMind" && !solved*/ !player1) {

            hacking.LoadHacking("hackingEvadeFlammes", nameOfSuccessEvent, player1, difficulty, armTool.gameObject);
            hacking.evadeFlammesDifficulty = difficulty;
            //hacking.loadingHacking = true;
            
        } else {

            print("name: " + Hacking.hackingName + "   solved: " + solved);

        }

    }
}