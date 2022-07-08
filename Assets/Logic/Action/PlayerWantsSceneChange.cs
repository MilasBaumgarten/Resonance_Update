using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic.Actions {
	[RequireComponent(typeof(PhotonView))]
	public class PlayerWantsSceneChange : Action {
		[SerializeField] private string loadScene;

		public override void Activate() {
			base.Activate();

			LoadingScreen.SetState(true);

			photonView.RPC("RequestSceneChangeRPC", RpcTarget.MasterClient);
		}

		[PunRPC]
		private void RequestSceneChangeRPC() {
			SceneManager.LoadScene(loadScene);
		}
	}
}