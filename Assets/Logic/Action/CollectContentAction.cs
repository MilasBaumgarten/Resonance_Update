using UnityEngine;

namespace Logic.Actions {

    public class CollectContentAction : Action {
        [SerializeField]
        [Tooltip("The Object Content used to trigger")]
        private ObjectContent objCont;

        private bool tryingToCollect = false;

		private void Update() {
			if (tryingToCollect) {
                if (!PlayerManager.localPlayerInstance.GetComponent<PlayerManager>().logbook.isActive) {
                    objCont.Interact(PlayerManager.localPlayerInstance.GetComponent<ArmTool>());
                    tryingToCollect = false;
                }
			}
		}

		public override void Activate() {
            tryingToCollect = true;
        }
    }
}