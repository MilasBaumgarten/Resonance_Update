using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ResonanceAnimation : MonoBehaviour {
	[SerializeField]
	private GameObject decisionA;
	[SerializeField]
	private GameObject decisionB;

	[SerializeField]
	private float waitTime = 2f;

	[SerializeField]
	private string oneLinerA_ID;
	[SerializeField]
	private string oneLinerB_ID;

	[SerializeField]
	private RotateCameraToVector cameraRotator_A;
	[SerializeField]
	private RotateCameraToVector cameraRotator_B;

	private CameraMovement cameraMovement;
	private PlayerMovement playerMovement;

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<PhotonView>().IsMine) {
			cameraMovement = other.GetComponent<CameraMovement>();
			playerMovement = other.GetComponent<PlayerMovement>();

			cameraRotator_A.Setup(
				other.transform.Find("Head"),
				cameraMovement
			);
			cameraRotator_B.Setup(
				other.transform.Find("Head"),
				cameraMovement
			);

			cameraMovement.enabled = false;
			playerMovement.enabled = false;
			StartCoroutine(AnimateMovement());
		}
	}

	private IEnumerator AnimateMovement() {
		cameraRotator_A.RotateCam(false);

		DialogSystem.instance.StartOneLiner(oneLinerA_ID);
		int id_a = DialogSystem.instance.dialogTextFromExcel.oneLinerID.IndexOf(oneLinerA_ID);
		yield return new WaitForSeconds(waitTime + DialogSystem.instance.dialogTextFromExcel.oneLinerPlayTimer[id_a]);
		cameraRotator_A.RotateCam(true);
		yield return new WaitUntil(() => cameraRotator_A.state == CameraState.IDLE);

		cameraRotator_B.RotateCam(false);
		DialogSystem.instance.StartOneLiner(oneLinerB_ID);
		int id_b = DialogSystem.instance.dialogTextFromExcel.oneLinerID.IndexOf(oneLinerB_ID);
		yield return new WaitForSeconds(waitTime + DialogSystem.instance.dialogTextFromExcel.oneLinerPlayTimer[id_b]);

		cameraRotator_B.RotateCam(true);

		yield return new WaitUntil(() => cameraRotator_B.state == CameraState.IDLE);

		cameraMovement.enabled = true;
		playerMovement.enabled = true;
		Destroy(gameObject);
	}
}