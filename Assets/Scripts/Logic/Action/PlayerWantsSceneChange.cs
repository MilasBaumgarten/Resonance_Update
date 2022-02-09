using UnityEngine;
using UnityEngine.Networking;

namespace Logic.Actions {
	public class PlayerWantsSceneChange : Action {
		[SerializeField] private string loadScene;

		public override void Activate() {
			base.Activate();
			NetworkManager.singleton.ServerChangeScene(loadScene);
		}
	}
}