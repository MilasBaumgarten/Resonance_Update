using Logic;
using UnityEditor;
using UnityEngine;

///<summary> custom editor for all triggers inheriting from this one </summary>
[CustomEditor(typeof(Trigger), true)]
public class TriggerEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector(); //draw the default inspector

		Trigger myScript = (Trigger)target; //define the target class

		if (GUILayout.Button("Test Trigger") && EditorApplication.isPlaying) {  //check if the inspector-button got pressed during play-mode
			OnButtonPressed(myScript);  //do whatever the inspector-button is supposed to
		}
	}

	///<summary> does whatever the inspector-button is supposed to </summary>
	public virtual void OnButtonPressed(Trigger myScript) {
		myScript.Interact();    //fakes a player interacting with the trigger for testing
	}
}