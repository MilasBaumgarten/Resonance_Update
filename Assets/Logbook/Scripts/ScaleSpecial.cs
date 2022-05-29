using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScaleSpecial : MonoBehaviour {
	[SerializeField]
	[Tooltip("The scale the image should scale to when clicked")]
	private float bigScale;

	[SerializeField]
	[Tooltip("The scale the image should scale to when clicked")]
	private float smallScale;

	[SerializeField]
	[Tooltip("The scaled up image")]
	private Image scaledUpImageObject;

	[SerializeField]
	[Tooltip("The time until the image is scaled up/down")]
	private float scaleTime;

	public static Image scaledUpImage;

	void Start() {
		scaledUpImage = scaledUpImageObject;
	}


	IEnumerator ScaleUp() {
		scaledUpImage.gameObject.SetActive(true);

		scaledUpImage.rectTransform.localScale = new Vector3(smallScale, smallScale, scaledUpImage.rectTransform.localScale.z);

		float currentScale = scaledUpImage.rectTransform.localScale.y;
		float stepSize = (bigScale - currentScale) / scaleTime;

		while (currentScale < bigScale) {
			currentScale += stepSize * Time.deltaTime;

			scaledUpImage.rectTransform.localScale = new Vector3(currentScale, currentScale, scaledUpImage.rectTransform.localScale.z);

			yield return null;
		}

		scaledUpImage.rectTransform.localScale = new Vector3(bigScale, bigScale, scaledUpImage.rectTransform.localScale.z);

		yield return null;
	}

	IEnumerator ScaleDown() {
		//scaledUpImage.rectTransform.localScale = new Vector3(smallScale, smallScale, scaledUpImage.rectTransform.localScale.z);

		float currentScale = scaledUpImage.rectTransform.localScale.y;
		float stepSize = (currentScale - smallScale) / scaleTime;

		print("down1");

		while (currentScale > smallScale) {
			print("down");
			currentScale -= stepSize * Time.deltaTime;

			scaledUpImage.rectTransform.localScale = new Vector3(currentScale, currentScale, scaledUpImage.rectTransform.localScale.z);

			yield return null;
		}

		scaledUpImage.rectTransform.localScale = new Vector3(smallScale, smallScale, scaledUpImage.rectTransform.localScale.z);

		scaledUpImage.gameObject.SetActive(false);

		yield return null;
	}

	public void onClickSmallPic() {
		StartCoroutine("ScaleUp");
	}

	public void onClickBigPic() {
		StartCoroutine("ScaleDown");
	}
}
