using UnityEditor;
using UnityEngine;

// Group- and GroupUndofunction, Strg + G
// automatically creates a gameobject containing all selected objects
// 
// by Christian

public static class GroupCommand {

    [MenuItem("GameObject/Group Selected %g")]

    private static void GroupSelected() {
        if (!Selection.activeTransform) return;
        var go = new GameObject(Selection.activeTransform.name + "Group");
        Undo.RegisterCreatedObjectUndo(go, "Group Selected");
        go.transform.SetParent(Selection.activeTransform.parent, false);
        foreach (var transform in Selection.transforms) Undo.SetTransformParent(transform, go.transform, "Group Selected");
        Selection.activeGameObject = go;
    }
}