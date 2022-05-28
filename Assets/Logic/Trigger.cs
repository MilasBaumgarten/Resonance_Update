using UnityEngine;
using UnityEngine.Events;

namespace Logic {

    public abstract class Trigger : MonoBehaviour {

        [SerializeField]
        [Tooltip("Event is called when the Trigger's state is flipped")]
        private UnityEvent OnStateFlipped;
        private TriggerEvent triggerEvent;  //the event all linked connections are listening to

        [SerializeField]
        [Tooltip("Should the Trigger lock it's State after flipping it's State once?")]
        private bool selfLocking;
        private bool locked;        //lock-state of the trigger
        private bool state = false; //current state of the trigger (default is "false")

        ///<summary> used for initialization </summary>
        private void Awake() {
            if (triggerEvent == null) {
                triggerEvent = new TriggerEvent();  //initialize triggerEvent
            }
        }

        ///<summary> the method called by the interacting player </summary>
        public virtual void Interact() {
            if (!locked) {      //do nothing if the trigger is locked
                FlipState();    //flip the current state
                TriggerEvent(); //trigger the triggerEvent
            }
        }

        ///<summary> flips the state of the trigger </summary>
        public void FlipState() {
            if (selfLocking) {            //lock the trigger if it should
                Lock();
            }
            state = !state;             //flip the value of state
            OnStateFlipped.Invoke();    //invoke OnStateFlipped event
        }

        ///<summary> locks the trigger by setting locked to true </summary>
        public void Lock() {
            locked = true;
        }

        ///<summary> unlock the trigger by setting locked to false </summary>
        public void Unlock() {
            locked = false;
        }

        ///<summary> triggers the event all connections are listening to </summary>
        private void TriggerEvent() {
            triggerEvent.Invoke(state); //invoke triggerEvent
        }

        ///<summary> returns triggerEvent (used by Connection to add listeners) </summary>
        public TriggerEvent GetTriggerEvent() {
            return triggerEvent;
        }
    }

    [System.Serializable]
    public class TriggerEvent : UnityEvent<bool> { }    //custom event using a bool as a parameter
}