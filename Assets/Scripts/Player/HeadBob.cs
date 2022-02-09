using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/**
 * Author: Leon Ullrich
 * Description:
 *  -place this script on the player-camera 
 * 
 * Changes: Noah Stolz
 * Added variable to disable bobbing outside of script
 */
public class HeadBob : MonoBehaviour {
    // Initial position of the camera
    Vector3 restPosition;
    // temporary camera position
    Vector3 camPos;

    private bool startBobbing;

    private bool bobbingEnabled;

    [HideInInspector]
    public bool stopSmooth = false;

    private Settings playerSettings;

    float timer = Mathf.PI / 2;

    void Start() {
		playerSettings = GameManager.instance.settings;

        // initialize values
        camPos = transform.localPosition;
        restPosition = camPos;
        startBobbing = true;
        bobbingEnabled = playerSettings.bobbingEnabled;
    }

    void Update() {
        Bob();
    }

    void Bob() {
        // if player is moving
        if (bobbingEnabled && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !stopSmooth) {

            timer += playerSettings.bobbingSpeed * Time.deltaTime;

            // if bobbing-motion just started and the camera's y-value has reached the peak of the parabolic path, turn off startBobbing-boolean and continue with normal bobbing motion
            if (startBobbing && camPos.y >= Mathf.Abs((Mathf.Sin(timer) * playerSettings.bobbingHeight) + restPosition.y)) {
                startBobbing = false;
            }
            // if bobbing motion just started and the camera's y-value has not yet reached it's peak, transition smoothly to said peak
            else if (startBobbing && camPos.y < Mathf.Abs((Mathf.Sin(timer) * playerSettings.bobbingHeight) + restPosition.y)) {


                Vector3 newPos = new Vector3(Mathf.Lerp(camPos.x, Mathf.Cos(timer) * playerSettings.bobbingWidth, playerSettings.transitionSpeed * Mathf.PI * Time.deltaTime), // lerp to the x-position according to the transition speed
                                             Mathf.Lerp(camPos.y, camPos.y + Mathf.Abs((Mathf.Sin(Mathf.PI / 2) * playerSettings.bobbingHeight)), playerSettings.transitionSpeed / (Mathf.PI * 2) * Time.deltaTime), // lerp to the y-position according to the transition speed
                                             camPos.z); // z-position stays the same
                camPos = newPos;

            }
            else if (!startBobbing) {
                // apply bobbing-effect  
                //use the timer value to set the position
                Vector3 newPosition = new Vector3(Mathf.Cos(timer) * playerSettings.bobbingWidth, restPosition.y + Mathf.Abs((Mathf.Sin(timer) * playerSettings.bobbingHeight)), restPosition.z); //abs val of y for a parabolic path
                camPos = newPosition;
            }

        }
        else {
            // reset
            timer = Mathf.PI / 2;
            Vector3 newPosition = new Vector3(Mathf.Lerp(camPos.x, restPosition.x, playerSettings.transitionSpeed * Time.deltaTime),
                                              Mathf.Lerp(camPos.y, restPosition.y, playerSettings.transitionSpeed * Time.deltaTime),
                                              Mathf.Lerp(camPos.z, restPosition.z, playerSettings.transitionSpeed * Time.deltaTime)); //transition smoothly from walking to stopping.
            camPos = newPosition;
            startBobbing = true;
        }

        if (timer > Mathf.PI * 2) { // full cycle complete
            timer = 0; // reset to 0 to avoid bloating values
        }
        // apply temporary position to localPosition
        transform.localPosition = camPos;
    }


    public void SetBobbing(bool state) {
		if (state) {
			bobbingEnabled = true;
		} else {
			bobbingEnabled = false;
            transform.localPosition = Vector3.zero;
		}
    }
}
