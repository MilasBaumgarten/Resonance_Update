// Author: Noah Stolz
// Used to controll input settings
// Create a new Settings Asset, if needed with: RM -> Create -> InputSettings

using UnityEngine;

[CreateAssetMenu(fileName = "NewInputSettings", menuName = "InputSettings")]
public class InputSettings : ScriptableObject {

    #region Keybinds
    [Header("Keybinds")]

    [Tooltip("Button for moving forward")]
    public KeyCode forward;

    [Tooltip("Button for moving backward")]
    public KeyCode backward;

    [Tooltip("Button for moving left")]
    public KeyCode left;

    [Tooltip("Button for moving right")]
    public KeyCode right;

    [Tooltip("Button for moving held Objects further away")]
    public KeyCode moveHeldObjectOut;

    [Tooltip("Button for moving held Objects closer")]
    public KeyCode moveHeldObjectIn;

    [Tooltip("Button for using the Arm Tool")]
    public KeyCode armTool;

    [Tooltip("Button for opening and closing the logbook")]
    public KeyCode logbook;

    [Tooltip("Button for interacting")]
    public KeyCode interact;

    [Tooltip("Button for opening and closing the menu")]
    public KeyCode closeLogbook;
    #endregion
}
