// Author: Nico Mahler
// changes: Milas Baumgarten (07.10.2018), Felix Werner (11.10.2018)
// Clamps the Mouse Input-Axis
// Should be attached to the PlayerPrefab
// Changes: Noah Stolz 
// Slow mouse when using forcetool

using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public bool ClampedCameraMotion;
    public Settings playerSettings;
    public Transform playerCamera;

    // private variables for the Movementspeed of each axis, accessible through Inspector
    [SerializeField]
    private float hSens;
    [SerializeField]
    private float vSens;
    [SerializeField]
    private float hClamp;
    [SerializeField]
    private float vClamp;
    [SerializeField]
    private float minAngle = -90.0f;
    [SerializeField]
    private float maxAngle = 90.0f;

    RotateCameraToVector rotCamToVec;

    private float rotX;
    private float rotY;

    private void Awake() {

        rotCamToVec = playerCamera.GetComponent<RotateCameraToVector>();

        Cursor.lockState = CursorLockMode.Locked;

        hSens = playerSettings.horizontalSensitivity;
        vSens = playerSettings.verticalSensitivity;
        hClamp = playerSettings.horizontalClamp;
        vClamp = playerSettings.verticalClamp;

    }

    void Update() {
        UpdateCameraMovement();

    }

    private void UpdateCameraMovement() {
        // Clamp the Movement with the public Variables of the maximum movement across the horizontal / vertical axis
        if (ClampedCameraMotion) {
            rotX += Mathf.Clamp(Input.GetAxis("Mouse X") * hSens * Time.deltaTime, -hClamp, hClamp);
            rotY = Mathf.Clamp(rotY - Mathf.Clamp(Input.GetAxis("Mouse Y") * vSens * Time.deltaTime, -vClamp, vClamp), minAngle, maxAngle);
        } else {
            rotX += Input.GetAxis("Mouse X") * hSens * Time.deltaTime;
            rotY = Mathf.Clamp(rotY - Input.GetAxis("Mouse Y") * vSens * Time.deltaTime, minAngle, maxAngle);
        }

        if (rotCamToVec) {
            rotCamToVec.StopCoroutine("RotateToward");
        }

        // Rotate the player
        this.transform.localEulerAngles = new Vector3(0, rotX, 0);
        // Tilt the Camera
        playerCamera.transform.localEulerAngles = new Vector3(rotY, 0, 0);
    }

    // switch between motion modes (i.e. switch camera max speed)
    public void SetCameraFree(bool state) {
        if (state) {
            hSens = playerSettings.horizontalSensitivity;
            vSens = playerSettings.verticalSensitivity;
            hClamp = playerSettings.horizontalClamp;
            vClamp = playerSettings.verticalClamp;

            ClampedCameraMotion = false;
        } else {
            hSens = playerSettings.horizontalForceSensitivity;
            vSens = playerSettings.verticalForceSensitivity;
            hClamp = playerSettings.horizontalForceClamp;
            vClamp = playerSettings.verticalForceClamp;

            ClampedCameraMotion = true;
        }
    }

    /*
    /// <summary>Set the tilt of the player camera to its rotation </summary>
    public void setRotY()
    {

        rotY = Mathf.Clamp(playerCamera.transform.localRotation.eulerAngles.x, minAngle, maxAngle);

    }*/
}
