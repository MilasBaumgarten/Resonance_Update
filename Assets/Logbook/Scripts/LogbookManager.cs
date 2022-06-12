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
	private RectTransform[] cornersRect;
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

		cornersRect[0].anchoredPosition = startTransform.anchoredPosition;
		cornersRect[1].anchoredPosition = startTransform.anchoredPosition;

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

	public void DisablePanel() {
		armTool.enabled = true;

		logbookAudio.onCloseLogbook();

		rotateCamToVect.RotateCam(true);

		StartCoroutine(ScaleAnimation(false));

		isActive = false;

		//endOfClosing();
		/*
        // clear
        DisableAllPanels();

        // activate CameraMovement
        // Lock cursor to middle of the screen
        cameraMovement.enabled = true;
        //cameraMovement.setRotY();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isActive = false;*/

		// Animation stuff
		animator.ResetTrigger("logbook active");
		boneOverride.enabled = true;
	}

	private void endOfClosing() {
		panelParent.parent.gameObject.SetActive(false);
		// clear
		DisableAllPanels();

		playerMovement.enabled = true;
		headBob.enabled = true;
		//headBob.SetBobbing(true);

		// activate CameraMovement
		// Lock cursor to middle of the screen
		cameraMovement.enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		isActive = false;

	}

	public void OpenLogbook() {
		armTool.DeselectTool();

		panelParent.parent.gameObject.SetActive(true);

		StartCoroutine(ScaleAnimation(true));

		StartCoroutine("moveCorners1");
		rotateCamToVect.RotateCam(false);

		logbookAudio.onOpenLogbook();

		playerMovement.enabled = false;

		//headBob.SetBobbing(false);
		headBob.enabled = false;

		// deactivate CameraMovement
		cameraMovement.enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		isActive = true;

		// Animation stuff
		animator.SetTrigger("logbook active");
		boneOverride.enabled = false;
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
			endOfClosing();
		}

		yield return null;
	}



	IEnumerator moveCorners1() {
		float correction = 0.00055f;

		if (openCloseTime == 0f) {
			openCloseTime = 0.0001f;
		}

		yield return new WaitForSeconds(delay);

		int i = 0;

		float stepSizeY = (startTransform.anchoredPosition.y - targetTransform.anchoredPosition.y) / openCloseTime;
		float stepSizeX = (startTransform.anchoredPosition.x - targetTransform.anchoredPosition.x) / openCloseTime;

		cornersRect[0].anchoredPosition = startTransform.anchoredPosition;
		cornersRect[1].anchoredPosition = startTransform.anchoredPosition;

		while (i < 83 * openCloseTime) {
			cornersRect[0].anchoredPosition = new Vector3(cornersRect[0].anchoredPosition.x - (stepSizeX * Time.deltaTime * correction), cornersRect[0].anchoredPosition.y - (stepSizeY * Time.deltaTime * correction), 0);
			cornersRect[1].anchoredPosition = new Vector3(cornersRect[1].anchoredPosition.x + (stepSizeX * Time.deltaTime * correction), cornersRect[1].anchoredPosition.y - (stepSizeY * Time.deltaTime * correction), 0);

			i++;

			yield return null;

		}

		//corners[0].position = new Vector3(startScale, startScale, panelParent.parent.transform.localScale.z);
		yield return null;
	}

	// Makes the logbook first wide then full size
	/*IEnumerator AlternateScaleUp()
    {

        float startScale = defaultScale;      

        panelParent.parent.transform.localScale = new Vector3(0f, 0f, panelParent.parent.transform.localScale.z);

        float currentScale = panelParent.parent.transform.localScale.y;

        if (openCloseTime == 0f)
        {

            openCloseTime = 0.0001f;

        }

        float stepSize = startScale / openCloseTime;

        yield return new WaitForSeconds(delay);

        while (currentScale < startScale/200f)
        {

            panelParent.parent.transform.localScale = new Vector3(currentScale += (stepSize * Time.deltaTime), currentScale += (stepSize * Time.deltaTime), panelParent.parent.transform.localScale.z);
            yield return null;

        }
        
        while (currentScale < startScale)
        {

            panelParent.parent.transform.localScale = new Vector3(currentScale += (stepSize * Time.deltaTime), panelParent.parent.transform.localScale.y, panelParent.parent.transform.localScale.z);
            yield return null;

        }

        currentScale = panelParent.parent.transform.localScale.y;

        while (currentScale < startScale)
        {

            panelParent.parent.transform.localScale = new Vector3(startScale, currentScale += (stepSize * Time.deltaTime), panelParent.parent.transform.localScale.z);
            yield return null;

        }
    }*/
}