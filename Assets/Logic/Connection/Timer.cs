using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Connections {
    public class Timer : Connection {
    
    /*
    * Author: Marisa Schmelzer
    *  - must be attached to the controller object in the scene
    */

        [Tooltip("set the timer")]
        [SerializeField] private float maxTime;
        // is used in Countdown-method
        private float timer = 0.0f;
        // do we need the Countdown now
        private bool runTimer = false;

        // Update is called once per frame
        void Update() {
            if (runTimer) {
                Countdown();
            }
        }

        // countdown method
        public void Countdown() {
            if (timer > 0.0f) {
                timer -= Time.deltaTime;
                // method from Connection.cs
                base.TriggerAction();
            }
            else if (timer <= 0.0f) {
                runTimer = false;
            }
        }

        // activate Countdown-method
        public override void TriggerAction() {
            // set timer to base time
            timer = maxTime;
            runTimer = true;
        }
    }
}
