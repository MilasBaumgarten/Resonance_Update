using UnityEngine;
//using UnityEditor;

/**
 * Author: Leon Ullrich
 * Description:
 *  -uses an instance of the DialogTriggerListener class to activate the dialog when a trigger is triggered
 *  -put this script on the object which triggers the dialog (TODO implement multiple ways to trigger the dialog (i.e. TriggerEnter, interaction with the object etc.)
 *  -passes the filename to the DialogTriggerListener
 *  
 *  Author: Marisa Schmelzer
 *  Description:
 *   -added OnTriggerEnter() method
 *   
 *  Changes: Noah Stolz
 *  Added oneliner support
 *  
 */
public class DialogTrigger : MonoBehaviour {

	// the name of the csv-file (without .csv file-extension!)
	[Tooltip("The name of the csv-file (without .csv file-extension")]
	public string filename = "";
	// for showing dialog and playing audio just once
	private bool hasPlayed = false;

	[Tooltip("Is this a one liner or Dialog?")]
	public bool isOneLiner = false;

	[Tooltip("Used if you wish to play a one liner instead of a complete dialog")]
	public string oneLinerID;

	// start dialog and audio
	void OnTriggerEnter(Collider other) {
		// takes a look if gameobject has player tag and if it was triggered once
		if (other.tag == "Player" && !hasPlayed) {
			if (isOneLiner) {

                DialogTextFromExcel dialogText = new DialogTextFromExcel();

                dialogText.GetOneLinerValues();

				if (dialogText.oneLinerID.Contains(oneLinerID)) {
					DialogSystem.instance.StartOneLiner(oneLinerID);
				} else {
					Debug.LogError("One-Liner ID not found! " + oneLinerID);
				}
			} else {
				DialogSystem.instance.StartDialog(filename); // called from script DialogTriggerListener
			}

			hasPlayed = true;

            Destroy(gameObject);
		}
	}

    /// <summary>Used to play dialog or oneliners from outside of this script</summary>
    public void playDialog()
    {

        if (hasPlayed)
        {

            return;

        }

        if (isOneLiner)
        {

            DialogTextFromExcel dialogText = new DialogTextFromExcel();

            dialogText.GetOneLinerValues();

            if (dialogText.oneLinerID.Contains(oneLinerID))
            {
                DialogSystem.instance.StartOneLiner(oneLinerID);
            }
            else
            {
                Debug.LogError("One-Liner ID not found! " + oneLinerID);
            }
        }
        else
        {
            DialogSystem.instance.StartDialog(filename); // called from script DialogTriggerListener
        }

        hasPlayed = false;

    }
}


/*
[CustomEditor(typeof(DialogTrigger))]
public class DialogEditor : Editor{

    override public void OnInspectorGUI() {

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((DialogTrigger)target), typeof(DialogTrigger), false);
        GUI.enabled = true;

        var dialogTriggerScript = target as DialogTrigger;

        dialogTriggerScript.filename = EditorGUILayout.TextField("Filename", dialogTriggerScript.filename);

        dialogTriggerScript.isOneLiner = GUILayout.Toggle(dialogTriggerScript.isOneLiner, "Is OneLiner");

        if (dialogTriggerScript.isOneLiner) {
            dialogTriggerScript.oneLinerID = EditorGUILayout.TextField("OneLiner ID", dialogTriggerScript.oneLinerID);
        }        
    }
}*/

