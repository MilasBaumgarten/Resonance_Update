using UnityEngine;

public class SceneChanger : MonoBehaviour {

	public string sceneName;
	
	private void OnTriggerEnter(Collider other) {
        ChangeScene();
    }

    public void ChangeScene() {
        // TODO: reenable
        //LobbyManager.s_Singleton.ChangeScene(sceneName);
    }
}
