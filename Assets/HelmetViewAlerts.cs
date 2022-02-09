using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelmetViewAlerts : MonoBehaviour {

    [SerializeField]
    private Image violenceAlert;
    [SerializeField]
    private Image messageAlert;

    public Settings playerSettings;
    private bool violenceValue;
    private bool messageValue;

    private void Start()
    {
        violenceAlert.gameObject.SetActive(false);
        messageAlert.gameObject.SetActive(false);

        playerSettings.violenceAlert = false;
        playerSettings.messageAlert = false;

        violenceValue = playerSettings.violenceAlert;
        messageValue = playerSettings.messageAlert;
    }

    private void Update()
    {
        if(playerSettings.violenceAlert != violenceValue || playerSettings.messageAlert != messageValue)
        {
            if (playerSettings.violenceAlert)
                violenceAlert.gameObject.SetActive(true);
            else
                violenceAlert.gameObject.SetActive(false);

            violenceValue = playerSettings.violenceAlert;

            if (playerSettings.messageAlert)
                messageAlert.gameObject.SetActive(true);
            else
                messageAlert.gameObject.SetActive(false);

            messageValue = playerSettings.messageAlert;
        }
    }
}
