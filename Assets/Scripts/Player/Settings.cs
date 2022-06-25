// Author: Nico Mahler
// changes: Felix Werner (11.10.2018)
// Asset for the Player Settings
// Scriptable Object, does not need to be instantiated in the Scene
// Create a new Settings Asset, if needed with: RM -> Create -> Settings

using UnityEngine;

[CreateAssetMenu(fileName = "New_Settings", menuName = "Settings")]
public class Settings : ScriptableObject {

    [Header("All Player Settings can be found here")]

    [Space]
    [Space]

    #region Helm|Innenansicht-Einstellungen
    [Header("Helmet inner view settings")]
    [Tooltip("Should violence alert be displayed?")]
    public bool violenceAlert;
    [Tooltip("Should new message alert be displayed?")]
    public bool messageAlert;
    #endregion

    [Space]

    #region Dialog-Einstellungen
    [Header("Dialog settings")]
    [Tooltip("Should subtitles be displayed?")]
    public bool displaySubtitles;
    [Tooltip("Should dialog-subtitles be english or german?")]
    public bool isEnglish;
    #endregion

    [Space]

    #region Maus-Einstellungen
    [Header("Mouse-Settings")]

    [Range(0.1f, 30f)]
    [Tooltip("horizontal Mouse-Sensitivity (0.1 to 30)")]
    public float horizontalSensitivity = 20.0f;

    [Range(0.1f, 30f)]
    [Tooltip("vertical Mouse-Sensitivity (0.1 to 30)")]
    public float verticalSensitivity = 20.0f;

    [Range(0.1f, 10f)]
    [Tooltip("horizontal Mouse-Sensitivity in Force Modus (0.1 to 10)")]
    public float horizontalForceSensitivity = 5.0f;

    [Range(0.1f, 10f)]
    [Tooltip("vertical Mouse-Sensitivity in Force Modus (0.1 to 10)")]
    public float verticalForceSensitivity = 5.0f;

    [Range(0.1f, 100f)]
    [Tooltip("horizontal Mouse Clamp (experimental) (0.1 to 100)")]
    public float horizontalClamp = 10.0f;

    [Range(0.1f, 100f)]
    [Tooltip("vertical Mouse Clamp (experimental) (0.1 to 100)")]
    public float verticalClamp = 10.0f;

    [Range(0.1f, 100f)]
    [Tooltip("horizontal Mouse Clamp in Force Mode (experimental) (0.1 to 100)")]
    public float horizontalForceClamp = 10.0f;

    [Range(0.1f, 100f)]
    [Tooltip("vertical Mouse Clamp in Force Mode (experimental) (0.1 to 100)")]
    public float verticalForceClamp = 10.0f;

    [Range(-90.0f, 0.0f)]
    [Tooltip("how far the Camera can tilt down in degrees (-90° to 0°)")]
    public float minAngle = -90.0f;

    [Range(0.0f, 90.0f)]
    [Tooltip("how far the Camera can tilt up in degrees (0° to 90°")]
    public float maxAngle = 90.0f;

    #endregion

    [Space]

    #region PlayerMovement    
    [Header("Player Movement")]

    [Tooltip("The Player's Movement Speed")]
    public float moveSpeed;
    #endregion

    [Space]

    #region Interaction
    [Header("Interaction settings")]
    public float interactionDistance = 1.5f;
    #endregion

    [Space]

    #region ForceTool
    [Header("ForceTool")]
    [Tooltip("Speed of how fast the object will change distance to the player.")]
    public float distChangeSpeed = 10.0f;

    [Tooltip("Maximum distance between the object and the player.")]
    public float forceToolMaxDist = 5.0f;

    [Tooltip("Minimum distance between the object and the player.")]
    public float forceToolMinDist = 1.5f;
    #endregion

    [Space]

    #region Keybinds
    [Header("Keybinds")]

    [Tooltip("Button for using the Force Tool")]
    public KeyCode forceToolButton;

    [Tooltip("Button for moving held Objects further away")]
    public KeyCode moveOut;

    [Tooltip("Button for moving held Objects closer")]
    public KeyCode moveIn;
    #endregion

    [Space]

    #region HeadBobbin

    [Header("HeadBobbing")]

    [Tooltip("Enable or disable headbobbing")]
    public bool bobbingEnabled;

    [Tooltip("How 'high' the camera bobs during movement")]
    public float bobbingHeight = 0.4f;

    [Tooltip("How far the camera bobs to the sides during movement")]
    public float bobbingWidth = 0.2f;

    [Tooltip("How quick the camera cycles through the bobbing 'loops'")]
    public float bobbingSpeed = 3.3f;

    [Tooltip("How fast the camera resets to it's initial position after stopping")]
    public float transitionSpeed = 10f;
    #endregion
}