// Author: Noah Stolz
// The Object the player has to interact with to start the Combination minigame

using UnityEngine.Events;
using UnityEngine;

public class CombinationObject : Interactable {

    [SerializeField]
    [Tooltip ("The name of the combination that you wish to start")]
    private string nameOfCombination;

    [SerializeField]
    [Tooltip("The name of the combination that you wish to start")]
    private int indexOfChoice;

    [SerializeField]
    [Tooltip("The name of the objects that need to be clicked in Order to succeed")]
    private string[] correctCombination = new string[0];

    [SerializeField]
    [Tooltip("The name of the objects that need to be clicked in Order to succeed")]
    private bool checkAtCompletion;

    [SerializeField]
    [Tooltip ("The amount of time the player has to solve the combination. 0 = infinite")]
    private int time;

    [SerializeField]
    [Tooltip("Is this the Object you have to interact with to start the combination?")]
    private bool startingPoint = false;

    public UnityEvent onSuccess;

    /*
    void OnEnable()
    {

        // Start listening for the event
        EventManager.instance.StartListening(nameOfCombination, OnSuccess);

    }

    void OnDisable()
    {

        // Stop listening when this script is disabled
        EventManager.instance.StopListening(nameOfCombination, OnSuccess);

    }

    void OnSuccess()
    {

        onSuccess.Invoke();

    }*/

    public override void Interact(ArmTool armTool)
    {

        Combination combination = armTool.gameObject.GetComponent<Combination>();

        if (startingPoint)
            combination.InitCombination(nameOfCombination, time, correctCombination, checkAtCompletion, gameObject);
        else
            combination.nextChoice(nameOfCombination, indexOfChoice);

    }

}
