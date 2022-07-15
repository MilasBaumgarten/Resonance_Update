/* This wizard will replace a selection with an object or prefab.
 * Scene objects will be cloned (destroying their prefab links).
 * Original coding by 'yesfish', nabbed from Unity Forums
 * 'keep parent' added by Dave A (also removed 'rotation' option, using localRotation
 */
using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class ReplaceSelection : ScriptableWizard
{
    static GameObject replacement = null;
    static bool keep = false;
 
    public GameObject ReplacementObject = null;
    public bool KeepOriginals = false;

    static ScriptableWizard replaceSelectionWizard;
 
    [MenuItem("GameObject/-Replace Selection...")]
    static void CreateWizard()
    {
        if(replaceSelectionWizard){
            replaceSelectionWizard.Show();
        } else {
            replaceSelectionWizard = ScriptableWizard.DisplayWizard(
                "Replace Selection", typeof(ReplaceSelection), "Replace");
        }
    }
 
    public ReplaceSelection()
    {
        ReplacementObject = replacement;
        KeepOriginals = keep;
    }
 
    void OnWizardUpdate()
    {
        replacement = ReplacementObject;
        keep = KeepOriginals;
    }

    [System.Obsolete]
    void OnWizardCreate()
    {
        if (replacement == null)
            return;
 
        int undoInt = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Replaced Selection");
 
        Transform[] transforms = Selection.GetTransforms(
            SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
 
        foreach (Transform t in transforms)
        {
            GameObject g;
            PrefabAssetType pref = PrefabUtility.GetPrefabAssetType(replacement);
 
            if (pref != PrefabAssetType.NotAPrefab)
            {
                g = (GameObject) PrefabUtility.InstantiatePrefab(replacement);
            }
            else
            {
                g = (GameObject) Instantiate(replacement);
            }
 
            Transform gTransform = g.transform;
            gTransform.parent = t.parent;
            g.name = replacement.name;
            gTransform.localPosition = t.localPosition;
            gTransform.localScale = t.localScale;
            gTransform.localRotation = t.localRotation;
            Undo.RegisterCreatedObjectUndo(g, "Instantiated replacement");
        }
 
        if (!keep)
        {
            foreach (GameObject g in Selection.gameObjects)
            {
                Undo.DestroyObjectImmediate(g);
            }
        }
        Undo.CollapseUndoOperations(undoInt);
    }
}