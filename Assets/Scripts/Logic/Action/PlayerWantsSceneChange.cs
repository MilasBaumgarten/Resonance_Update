using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic.Actions {
	public class PlayerWantsSceneChange : Action {
		[SerializeField] private string loadScene;

		public override void Activate() {
			base.Activate();
			// TODO: reenable
			//NetworkManager.Singleton.SceneManager.LoadScene(loadScene, LoadSceneMode.Single);
		}
	}
}