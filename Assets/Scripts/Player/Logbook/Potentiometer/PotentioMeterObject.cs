// Author: Noah Stolz
// Opens the potentio meter tab when interacted with and sets the target value
// Should be attached to the Object the player has to interact with

using UnityEngine;
using UnityEngine.Events;

public class PotentioMeterObject : Interactable {

    [SerializeField]
    [Tooltip("The value the Potentio Meter has to be set to, to trigger the event, MUST be unique")]
    private float targetValue;

    private bool solved = false;

    public UnityEvent onSuccess;

    void OnEnable()
    {

        // Start listening for the event
        EventManager.instance.StartListening(targetValue.ToString(), OnSuccess);

    }

    void OnDisable()
    {

        // Stop listening when this script is disabled
        EventManager.instance.StopListening(targetValue.ToString(), OnSuccess);

    }

    private void OnSuccess()
    {

        solved = true;
        onSuccess.Invoke();

    }

    public override void Interact(ArmTool armTool)
    {

        if (!solved)
        {

            Potentiometer potentioMeter = armTool.gameObject.GetComponent<Potentiometer>();
            potentioMeter.targetValue = this.targetValue;

            potentioMeter.OpenMeter();

        }
    }

}
