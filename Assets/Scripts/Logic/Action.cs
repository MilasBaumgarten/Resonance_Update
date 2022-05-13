using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Logic {

    public abstract class Action : MonoBehaviourPun {

        [SerializeField]
        [Tooltip("Event is called when the Action gets activated")]
        private UnityEvent OnActionActivated;

        ///<summary> activates the action </summary>
        public virtual void Activate() {
            OnActionActivated.Invoke(); //invoke OnActionActivated
        }

        ///<summary> activates the action with generic parameters </summary>
        public virtual void Activate<T>(T param) {
            return; //does nothing by default
        }
    }
}