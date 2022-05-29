// Author: Noah Stolz
// Used to open the keypad tab and decide what the solution for this keypad is
// Should be attached to the object the player should interact with to open the keypad
using UnityEngine;
using System;
using UnityEngine.Events;

public class KeypadObject : Interactable{

    [SerializeField]
    [Tooltip("The code that will trigger the event, MUST be unique")]
    private string correctCode;

    [Tooltip("The event that is called when the correct code is entered")]
    public UnityEvent onSuccess;

    private bool solved = false;

    void Start()
    {

        int result;

        if (!Int32.TryParse(correctCode,out result))
        {

            Debug.LogWarning("correctCode not set to a nummber");

        }

    }

    void OnEnable()
    {

        // Start listening for the event
        EventManager.instance.StartListening(correctCode, invokeOnSuccess);

    }

    void OnDisable()
    {

        // Stop listening when this script is disabled
        EventManager.instance.StopListening(correctCode, invokeOnSuccess);

    }

    void invokeOnSuccess()
    {
        solved = true;
        onSuccess.Invoke();

    }

    public override void Interact(ArmTool armTool)
    {

        if (solved)
        {

            return;

        }

        Keypad keypad = armTool.gameObject.GetComponent<Keypad>();

        keypad.correctCode = this.correctCode;

        keypad.OpenKeypad();

    }
}
