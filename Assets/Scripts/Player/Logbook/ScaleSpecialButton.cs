using UnityEngine;
using UnityEngine.UI;

public class ScaleSpecialButton : MonoBehaviour {
	
    public void onScaleUp()
    {

        //ScaleSpecial.scaledUpImage.sprite = img.sprite;
        ScaleSpecial.scaledUpImage.sprite = this.transform.GetChild(1).GetComponent<Image>().sprite;

    }


}
