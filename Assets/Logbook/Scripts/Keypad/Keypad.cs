using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour {

    [HideInInspector]
    public string correctCode = "NotSet";

    [SerializeField]
    [Tooltip("The Message to be displayed if the player enters a wrong code")]
    private string failMessage = "ERROR";

    [SerializeField]
    [Tooltip("The Message to be displayed if the player enters a correct code")]
    private string successMessage = "SUCCESS";

    [SerializeField]
    [Tooltip("The maximum amount of digits")]
    private int maxNummbers = 6;

    [SerializeField]
    [Tooltip("The logbookManager attached to the logbook")]
    private LogbookManager logbookManager;

    [SerializeField]
    [Tooltip("The panel that displays the result")]
    private Text resultDisplay;

    private string currentCode = "";

    private bool lockButtons = false;


    public void OpenKeypad()
    {

        logbookManager.OpenLogbook();
        logbookManager.EnableOnePanel("keypad");

        currentCode = "";
        resultDisplay.text = currentCode;

    }

    private bool CheckForSuccess()
    {

        return currentCode.Equals(correctCode);

    }

    public void NumberButton(string nummber)
    {
        if(!lockButtons)
            currentCode = currentCode.Length >= maxNummbers ? currentCode : currentCode += nummber;

        resultDisplay.text = currentCode;

    }

    public void EnterButton()
    {
        if (!lockButtons) {

            currentCode = CheckForSuccess() ? successMessage : failMessage;

            resultDisplay.text = currentCode;

            if (currentCode.Equals(successMessage))
            {

                EventManager.instance.TriggerEvent(correctCode);
                logbookManager.CloseLogbook();

            }
            else
            {
                lockButtons = true;
                Invoke("ResetKeypad", 1f);

            }

        }
    }

    public void DeleteButton()
    {
        if (!lockButtons)
            currentCode = currentCode.Length > 0 ? currentCode.Substring(0, currentCode.Length -1) : currentCode;

        resultDisplay.text = currentCode;

    }

    /*IEnumerator resetKeypad()
    {

        yield return new WaitForSeconds(3f);

        currentCode = "";

        resultDisplay.text = currentCode;

        lockButtons = false;

        yield return null;

    }*/

    void ResetKeypad()
    {

        currentCode = "";

        resultDisplay.text = currentCode;

        lockButtons = false;

    }

}
