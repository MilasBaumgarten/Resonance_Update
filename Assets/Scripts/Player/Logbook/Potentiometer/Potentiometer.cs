using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: René Fuhrmann   
 * Script to trigger events with a Potentiometer
 * Attach this script to the Potentiometer
 * 
 * Changes: Noah Stolz
 * Overhauled script for new logbook
 * Script is no longer a trigger
 */


public class Potentiometer : MonoBehaviour {

    [SerializeField]
    [Tooltip("Text that displays SliderValue")]
    private LogbookManager logbookManager;
    [SerializeField]
    [Tooltip("Text that displays SliderValue")]
    private Text displayText;
    [SerializeField]
    [Tooltip("Slider this script is attached to")]
    private Slider slider;   
    [SerializeField]
    [Tooltip("Smalest number on the range of the slider")]
    private float minValue;
    [SerializeField]
    [Tooltip("Highest number on the range of the slider")]
    private float maxValue;

    [HideInInspector]
    public float targetValue;

    ///<summary> initialize variables </summary>
    private void Start() {
        // set range of slider and initialize text
        slider.minValue = minValue * 10;    
        slider.maxValue = maxValue * 10;
        ChangeText();                       
    }

    /// <summary>
    /// Called by the onInteract event of the PotentioMeterObject. Sets targetValue and opens the logbook
    /// </summary>
    public void OpenMeter()
    {

        logbookManager.openLogbook();
        logbookManager.EnableOnePanel("potentioMeter");

    }

    ///<summary> method to call onValueChanged, changes Text </summary>
    public void OnSliderChange() {
        // update text
        ChangeText();  
    }

    ///<summary> changes displayed text to value of slider </summary>
    private void ChangeText() {
        float outputNr = slider.value / 10;
        displayText.text = outputNr.ToString() + " Hz";

    }

    ///<summary> method to call on EndDrag, interacts when targetvalue is met </summary>
    public void OnEndDrag() {

        // when targetvalue is met, trigger event
        if (Mathf.Floor(slider.value) == Mathf.Floor(targetValue * 10)) {

            logbookManager.DisablePanel();
            EventManager.instance.TriggerEvent(targetValue.ToString());
        } 
    }
}
