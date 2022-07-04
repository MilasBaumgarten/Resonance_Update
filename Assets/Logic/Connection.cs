using UnityEngine;

namespace Logic {

    public abstract class Connection : MonoBehaviour {

        [SerializeField]
        [Tooltip("Array of all Triggers linked to this Connection")]
        private Trigger[] triggers;
        [SerializeField]
        [Tooltip("Array of all Actions linked to this Connection")]
        private Action[] actions;

        [SerializeField]
        [Tooltip("Should the Connection lock it's State after passing it's State-Check once?")]
        private bool selfLocking;
        private bool locked;        //lock-state of the connection, if true the connection won't react to triggers
        protected uint checkState;    //an integer the connection compares to after a trigger got interacted with
        private uint state = 1;     //the current state of the connection

        ///<summary> used for intitialization </summary>
        protected virtual void Start() {
            checkState = (uint)1 << triggers.Length;            //bit-shifting 1 left by the amount of triggers linked to this connection
            foreach (Trigger trigger in triggers) {
                trigger.GetTriggerEvent().AddListener(Receive); //adding listeners to all events in the triggers
            }
        }

        ///<summary> shifts the state of the connection according to the trigger state </summary>
        public virtual void ShiftState(bool triggerState) {
            if (triggerState) { //bit-shift depending on the new trigger state
                state <<= 1;    //bit-shift left
            } else {
                state >>= 1;    //bit-shift right
            }
        }

        ///<summary> receives the events invoked on the triggers </summary>
        public void Receive(bool triggerState) {
            if (!locked) {                  //don't do anything if the connection is locked into it's current state
                ShiftState(triggerState);   //shift the state of this connection according to the trigger state
                Check();                    //check if the connection state has reached the checkState
            }

        }

        ///<summary> checks if the connection state has reached the checkState </summary>
        private void Check() {
            if (state == checkState) {  //is the current stae equal to the checkState?
                TriggerAction();        //trigger the actions linked to this connection
                locked = selfLocking;   //lock the connection if it is supposed to
            }
        }

        ///<summary> triggers all actions linked to this connection </summary>
        public virtual void TriggerAction() {
            foreach (Action action in actions) {
                action.Activate();  //call the "Activate" method on each action
            }
        }

        ///<summary> draws line-gizmos to all linked triggers and actions in scene view </summary>
        private void OnDrawGizmos() {
            if (triggers != null) {
                foreach (Trigger trigger in triggers) {
                    Gizmos.DrawLine(this.transform.position, trigger.gameObject.transform.position);    //draw lines to all triggers
                }
            }
            if (actions != null) {
                foreach (Action action in actions) {
                    Gizmos.DrawLine(this.transform.position, action.gameObject.transform.position);     //draw lines to all actions
                }
            }
        }
    }
}