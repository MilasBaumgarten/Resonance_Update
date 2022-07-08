using UnityEngine;

public class LoadingScreen : MonoBehaviour {
	public static LoadingScreen instance;

	[SerializeField]
	private GameObject visualisationParent;

	// Start is called before the first frame update
	void Start() {
		if (instance) {
			Destroy(this);
		} else {
			instance = this;
			DontDestroyOnLoad(this);
		}
	}

	public static void SetState(bool state) {
		if (instance) {
			instance.visualisationParent.SetActive(state);
		} else {
			Debug.LogError("No LoadingScreen instance found.");
		}
	}
}
