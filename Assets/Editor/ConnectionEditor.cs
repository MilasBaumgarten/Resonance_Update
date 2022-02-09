using Logic;
using UnityEditor;
using UnityEngine;

///<summary> custom editor for all connections inheriting from this one </summary>
[CustomEditor(typeof(Connection), true)]
public class ConnectionEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector(); //draw the default inspector

		Connection myScript = (Connection)target;   //define the target class

		if (GUILayout.Button("Test Connection") && EditorApplication.isPlaying) {   //check if the inspector-button got pressed during play-mode
			OnButtonPressed(myScript);  //do whatever the inspector-button is supposed to
		}
	}

	///<summary> does whatever the inspector-button is supposed to </summary>
	public virtual void OnButtonPressed(Connection myScript) {
		myScript.TriggerAction();   //trigger all linked actions for testing
	}
}