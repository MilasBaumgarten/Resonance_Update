using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ResonanceAnimation : MonoBehaviour {

	public GameObject decisionA;
	public GameObject decisionB;

	public float lookSmoothness = 1f;
	public float waitTime = 2f;

	public string oneLinerA_ID;
	public string oneLinerB_ID;

	// public float zoomValue;
	private Transform savedRot;

	private Camera cam;
	private PlayerMovement playerMove;
	private CameraMovement camMove;
	private HeadBob headbob;

	private bool triggerA;
	private bool triggerB;

	private void OnTriggerEnter(Collider other) {

		if (other.GetComponent<PhotonView>().IsMine) {
			cam = other.transform.GetComponentInChildren<Camera>();
			playerMove = other.GetComponent<PlayerMovement>();
			camMove = other.GetComponent<CameraMovement>();
			headbob = other.transform.GetComponentInChildren<HeadBob>();

			StartCoroutine(AnimateMovement());
			savedRot = other.transform;
		}
	}

	private IEnumerator AnimateMovement() {
		//disable movement
		playerMove.enabled = false;
		camMove.enabled = false;
		headbob.enabled = false;
		// slowly let the player look at the decisions

		triggerA = true;
		DialogSystem.instance.StartOneLiner(oneLinerA_ID);
		yield return new WaitForSeconds(waitTime + DialogSystem.instance.dialogTextFromExcel.GetTimeToDisplay());
		//cam.fieldOfView = 60;
		triggerA = false;
		triggerB = true;
		DialogSystem.instance.StartOneLiner(oneLinerB_ID);
		yield return new WaitForSeconds(waitTime + DialogSystem.instance.dialogTextFromExcel.GetTimeToDisplay());
		triggerB = false;

		playerMove.enabled = true;
		camMove.enabled = true;
		headbob.enabled = true;
		//cam.fieldOfView = 60;
		cam.transform.SetPositionAndRotation(savedRot.position, savedRot.rotation);
		Destroy(gameObject);
	}

	private void FixedUpdate() {
		if (triggerA) {
			cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation,
			Quaternion.LookRotation(decisionA.transform.position - cam.transform.position), Time.deltaTime * lookSmoothness);
			//cam.fieldOfView -= zoomValue;
		}

		if (triggerB) {
			cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation,
			Quaternion.LookRotation(decisionB.transform.position - cam.transform.position), Time.deltaTime * lookSmoothness);
			//cam.fieldOfView -= zoomValue;
		}
	}
}