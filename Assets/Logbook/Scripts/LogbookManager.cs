/**
* Author: Marisa Schmelzer
* Description:
*  - must be attached to Logbook Canvas
*  - enables and disables the wanted screens from the logbook
*  - each canvas child needs a unique name
* 
*/

using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogbookManager : SerializedMonoBehaviour {
	[SerializeField]
	[Tooltip("The parent that all panels are children of")]
	private Transform panelParent;

	[SerializeField]
	[Tooltip("The armTool attached to the player")]
	private ArmTool armTool;

	[SerializeField]
	private RectTransform startTransform;
	[SerializeField]
	private RectTransform targetTransform;

	[SerializeField]
	[Tooltip("The RotateCameraToVector component attached to the camera")]
	private RotateCameraToVector rotateCamToVect;
	[SerializeField]
	[Tooltip("The Headbob component attached to the camera")]
	private HeadBob headBob;

	[SerializeField]
	[Tooltip("How long it should take for the logbook to open and close")]
	private float openCloseTime;

	[SerializeField]
	[Tooltip("The delay after pressing the button before the logbook scales up (in seconds)")]
	private float delay;


	[SerializeField]
	[Tooltip("the audio manager attached to the logbook")]
	private LogbookAudio logbookAudio;

	private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

	[HideInInspector]
	public bool isActive = false;

	[SerializeField]
	private GameObject player;

	[SerializeField]
	private BoneOverride boneOverrideCatriona;
	[SerializeField]
	private BoneOverride boneOverrideRobert;
	private BoneOverride boneOverride;

	private Animator animator;

	private CameraMovement cameraMovement;

	private PlayerMovement playerMovement;

	private float defaultScale;

	void Awake() {
		defaultScale = panelParent.parent.transform.localScale.y;
	}

	void Start() {
		if (!player.GetPhotonView().IsMine) {
			base.enabled = false;
			return;
		}

		foreach (Transform child in panelParent.transform) {
			panels.Add(child.name, child.gameObject);
		}

		if (player.GetComponent<PlayerManager>().nickname.Equals(CharacterEnum.CATRIONA)) {
			boneOverride = boneOverrideCatriona;
		} else {
			boneOverride = boneOverrideRobert;
		}

		DisableAllPanels();

		// store for CameraMovment script
		cameraMovement = player.GetComponent<CameraMovement>();
		playerMovement = player.GetComponent<PlayerMovement>();

		animator = player.GetComponent<PlayerManager>().animator;
	}


	// disable all panels who are children to the canvas where the script is attached to
	public void DisableAllPanels() {
		foreach (KeyValuePair<string, GameObject> panel in panels) {
			panel.Value.SetActive(false);
		}
	}

	// enables the wanted Panel through comparing the name who is saved in the dictionary
	public void EnableOnePanel(string panelName) {
		// clear
		DisableAllPanels();
		// look if it exists in the dictionary and enable it
		if (panels[panelName]) {
			panels[panelName].gameObject.SetActive(true);
		}
	}

	public void OpenLogbook() {
		StartCoroutine(OpenLogbookRoutine());
	}

	public IEnumerator OpenLogbookRoutine() {
		armTool.DeselectTool();

		panelParent.parent.gameObject.SetActive(true);

		rotateCamToVect.RotateCam(false);
		logbookAudio.onOpenLogbook();

		// deactivate Player & Camera movement
		playerMovement.enabled = false;
		headBob.enabled = false;
		cameraMovement.enabled = false;

		// Animation stuff
		boneOverride.enabled = false;
		animator.SetTrigger("logbook active");

		yield return ScaleAnimation(true);

		// unlock Cursor
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		isActive = true;
	}

	public void CloseLogbook() {
		StartCoroutine(CloseLogbookRoutine());
	}

	public IEnumerator CloseLogbookRoutine() {
		rotateCamToVect.RotateCam(true);
		logbookAudio.onCloseLogbook();

		// Animation stuff
		animator.ResetTrigger("logbook active");
		boneOverride.enabled = true;

		yield return ScaleAnimation(false);

		armTool.enabled = true;
		panelParent.parent.gameObject.SetActive(false);

		// clear
		DisableAllPanels();

		playerMovement.enabled = true;
		headBob.enabled = true;

		// activate CameraMovement
		cameraMovement.enabled = true;

		// Lock cursor to middle of the screen
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		isActive = false;
	}

	IEnumerator ScaleAnimation(bool scaleUp) {
		float goalScale;
		float stepSize;

		if (scaleUp) {
			goalScale = defaultScale;
			panelParent.parent.transform.localScale = new Vector3(0f, 0f, panelParent.parent.transform.localScale.z);
		} else {
			goalScale = 0f;
			panelParent.parent.transform.localScale = new Vector3(defaultScale, defaultScale, panelParent.parent.transform.localScale.z);
		}
		stepSize = defaultScale / (openCloseTime + 0.00001f);

		float currentScale = panelParent.parent.transform.localScale.y;

		// only wait when opening the logbook
		if (scaleUp) {
			yield return new WaitForSeconds(delay);
		}

		if (scaleUp) {
			while (currentScale < goalScale) {
				panelParent.parent.transform.localScale = new Vector3(currentScale += (stepSize * Time.deltaTime), currentScale += (stepSize * Time.deltaTime), panelParent.parent.transform.localScale.z);
				yield return null;
			}
		} else {
			while (currentScale > 0f) {
				panelParent.parent.transform.localScale = new Vector3(currentScale -= (stepSize * Time.deltaTime), currentScale -= (stepSize * Time.deltaTime), panelParent.parent.transform.localScale.z);
				yield return null;
			}
		}

		if (scaleUp) {
			panelParent.parent.transform.localScale = new Vector3(goalScale, goalScale, panelParent.parent.transform.localScale.z);
		} else {
			panelParent.parent.transform.localScale = new Vector3(0f, 0f, panelParent.parent.transform.localScale.z);
		}
	}
}