using Photon.Pun;
using UnityEngine;

namespace Logic.Actions {
    public class CollectContentAction : Action {
        [SerializeField]
        [Tooltip("The Object Content used to trigger")]
        private ObjectContent objCont;

        [SerializeField]
        private bool broadcastActivation = false;

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
            Debug.Log("Getting Activation", this);
            if (!broadcastActivation) {
                tryingToCollect = true;
            } else {
				photonView.RPC("ActivateActionRpc", RpcTarget.All);
            }
        }

        [PunRPC]
        void ActivateActionRpc() {
            tryingToCollect = true;
        }
    }
}