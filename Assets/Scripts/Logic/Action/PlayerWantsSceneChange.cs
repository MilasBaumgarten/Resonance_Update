using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Logic.Actions {
	public class PlayerWantsSceneChange : Action {
		[SerializeField] private string loadScene;

		public override void Activate() {
			base.Activate();
			NetworkManager.Singleton.SceneManager.LoadScene(loadScene, LoadSceneMode.Single);
		}
	}
}