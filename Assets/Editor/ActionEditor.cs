using Logic;
using UnityEditor;
using UnityEngine;

///<summary> custom editor for all connections inheriting from this one </summary>
[CustomEditor(typeof(Action), true)]
public class ActionEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector(); //draw the default inspector

		Action myScript = (Action)target;   //define the target class

		if (GUILayout.Button("Test Action") && EditorApplication.isPlaying) {   //check if the inspector-button got pressed during play-mode
			OnButtonPressed(myScript);  //do whatever the inspector-button is supposed to
		}
	}

	///<summary>  does whatever the inspector-button is supposed to </summary>
	public virtual void OnButtonPressed(Action myScript) {
		myScript.Activate();    //activate the action for testing
	}
}
