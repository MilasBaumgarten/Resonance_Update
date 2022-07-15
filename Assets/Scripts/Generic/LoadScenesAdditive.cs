using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenesAdditive : MonoBehaviour {

	[Tooltip("Input all names of the scenes which should be loaded additive")]
	[SerializeField] private string[] sceneNames;

	// load all specified Scenes
	void Start () {
		foreach(string scene in sceneNames){
			if(SceneManager.GetSceneByName(scene).isLoaded) continue;
			SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		}
	}
}
