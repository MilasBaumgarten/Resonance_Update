using Photon.Pun;
using UnityEngine;

namespace Logic.Actions {

    public class CollectContentAction : Action {
        [SerializeField]
        [Tooltip("The Object Content used to trigger")]
        private ObjectContent objCont;

        private bool tryingToCollect = false;
        private GameObject localPlayer;

		private void Update() {
			if (tryingToCollect) {
                if (!localPlayer.GetComponent<PlayerManager>().logbook.isActive) {
                    objCont.Interact(localPlayer.GetComponent<ArmTool>());
                    tryingToCollect = false;
                }
			}
		}

		public override void Activate() {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach(GameObject player in players) {
                if(player.GetPhotonView().IsMine) {
                    localPlayer = player;
                    tryingToCollect = true;
                    return;
                }
			}

            Debug.LogWarning("Trying to call Activate but can't find a local player!", this);
        }
    }
}