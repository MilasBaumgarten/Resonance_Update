using UnityEngine;

namespace Logic.Connections {
    public class Min : Connection {
        [SerializeField]
        private int necessaryTriggers;

        protected override void Start(){
            base.Start();
            checkState = (uint)1 << necessaryTriggers; // bit-shifting 1 left by the amount of necessary triggers
        }
    }
}
