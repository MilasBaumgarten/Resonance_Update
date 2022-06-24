using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour {
	public void LoadScene(string sceneToLoad) {
		SceneManager.LoadScene(sceneToLoad);
	}
}
