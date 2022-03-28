using Unity.Netcode;
using UnityEngine;
using UnityEditor;

public class HelloWorldManager : MonoBehaviour {
	[SerializeField]
	private Rect guiSize = new Rect(10, 10, 300, 300);

	private void Awake() {
		guiSize = new Rect(
			guiSize.x,
			Screen.height - guiSize.y - guiSize.height,
			guiSize.width,
			guiSize.height
		);
	}

	void OnGUI() {
		GUILayout.BeginArea(guiSize);

		// if Client or Host
		if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost) {
			SubmitNewPosition();
		}

		GUILayout.EndArea();
	}

	static void SubmitNewPosition() {
		if (GUILayout.Button("Request Position Change")) {
			var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
			var player = playerObject.GetComponent<HelloWorldPlayer>();
			player.Move();
		}
	}

#if UNITY_EDITOR
	// Editor UI
	private void OnDrawGizmosSelected() {
		Rect bottomLeftAlligned = new Rect(
			guiSize.x,
			Screen.height - guiSize.y - guiSize.height,
			guiSize.width,
			guiSize.height
		);

		Handles.BeginGUI();
		Handles.DrawSolidRectangleWithOutline(bottomLeftAlligned, new Color(1, 1, 1, 0.1f), Color.black);
		Handles.EndGUI();
	}
#endif
}
