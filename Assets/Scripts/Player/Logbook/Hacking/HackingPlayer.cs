// Author: Noah Stolz
// Used to controll the player during the evadeFlammes hacking-minigame
// Should be attached to the player
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class HackingPlayer : MonoBehaviour {

    [SerializeField]
    private InputSettings inputSettings;

    [SerializeField]
    private Hacking hacking;

    [SerializeField]
    private RectTransform recTrans;

    [SerializeField]
    private float playerSpeed = 10f;

    [SerializeField]
    private float invulnerabiltyTime = 3f;

    [SerializeField]
    private Image img;

    [SerializeField]
    private Text strikeText;

    // How fast the player should blink while he is invulnerable after tacking a hit
    private float invulBlinckingFrequenz = 0.15f;

    private bool invulnerable;

    Vector2 moveDirection;

    int strikes = 0;

    void Start()
    {
        StopCoroutine("Invulnerability");
        invulnerable = true;
        StartCoroutine("Invulnerability");
        strikeText.text = "";
    }

	// Update is called once per frame
	void Update () {

        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * playerSpeed * Time.deltaTime;

        if ((Input.GetKey(inputSettings.left) || Input.GetKey(inputSettings.right)) || (Input.GetKey(inputSettings.forward) || Input.GetKey(inputSettings.backward)))
        {

            recTrans.anchoredPosition += moveDirection;

            if (recTrans.anchoredPosition.x < 0f)
                recTrans.anchoredPosition = new Vector2(0f, recTrans.anchoredPosition.y);
            if (recTrans.anchoredPosition.x > 3200f)
                recTrans.anchoredPosition = new Vector2(3200f, recTrans.anchoredPosition.y);
            if (recTrans.anchoredPosition.y > 0f)
                recTrans.anchoredPosition = new Vector2(recTrans.anchoredPosition.x, 0f);
            if (recTrans.anchoredPosition.y < -1300f)
                recTrans.anchoredPosition = new Vector2(recTrans.anchoredPosition.x, -1300f);

        }

    }

    void OnTriggerEnter(Collider coll)
    {

        if (!invulnerable && coll.tag.Equals("Hacking"))
        {
            invulnerable = true;
            StartCoroutine("Invulnerability");
            //hacking.DecreaseTime(5);
            strikes++;
            strikeText.text += "X";

            if(strikes == 3)
            {

                strikes = 0;
                //Hacking.localInstance.AddMistakeServerRpc();
                StopCoroutine("Strike");
                StartCoroutine("Strike");
                //hacking.AddMistake();
                
            }

        }
            
    }

    IEnumerator Invulnerability()
    {

        float count = 0f;        

        while (count < invulnerabiltyTime)
        {

            if (img.enabled)
            {

                img.enabled = false;

            }
            else {

                img.enabled = true;

            }

            yield return new WaitForSeconds(invulBlinckingFrequenz);
            count += invulBlinckingFrequenz;
        }

        invulnerable = false;

    }

    IEnumerator Strike()
    {

        strikeText.color = new Color(1f, 0f, 0f, .5f);

        float count = 0f;

        while (count < invulnerabiltyTime)
        {

            yield return new WaitForSeconds(invulBlinckingFrequenz);
            count += invulBlinckingFrequenz;

        }

        strikeText.color = new Color(1f, 1f, 1f, .5f);
        strikeText.text = "";
    }

}
