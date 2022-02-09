// Author: Noah Stolz
// Controlls the hacking minigame
// Should be attached to the hacking tab in the logbook

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class Hacking : NetworkBehaviour {

    [SerializeField]
    [Tooltip("The logbookManager attached to the logbook")]
    private LogbookManager logbookManager;

    [SerializeField]
    [Tooltip("The flame for the evadeFlame minigame")]
    private GameObject hackingFlame;

    /*[SerializeField]
    [Tooltip("The parent of the buttons that display and swicht nummbers")]
    private Transform numberButtonParent;*/

    [SerializeField]
    [Tooltip("The panel for the evade flammes minigame")]
    private GameObject hackingEvadeFlammes;

    /*[SerializeField]
    [Tooltip("The panel for the switch nummbers minigame")]
    private GameObject hackingSwitchNummbers;*/

    public GameObject hackingMasterMind;

    public bool debugMode = false;

    [SerializeField]
    [Tooltip("The text that displays how much time is left")]
    private Text timerText;

    [SerializeField]
    PlayerMovement playerMovement;
    [SerializeField]
    HeadBob headBob;

    // The time the players has left to solve the minigame
    private static int time = 10;
    private static int punishmentTime = 10;

    [SerializeField]
    [Tooltip("The time within wich the second player has to start hacking")]
    private int punishmentStartTime = 10;

    [HideInInspector]
    // The time avaiable for hacking in seconds
    public int startTime = 10;
    [HideInInspector]
    // The name of hacking game that is played
    public static string hackingName = "none";
    // The name of the event that is called when hack is successfull
    private static string hackingEventName = "none";
    // The currently selected nummber
    private int selectedNummber = -1;
    // The text of the currently selected button
    private Text button1Text;
    // The text of the buttons that display and swicht nummbers
    //private Text[] numberButtonsText;
    // The array used to keep track of the nummbers
    //private int[] targetArray;
    [HideInInspector]
    public bool loadingHacking = false;
    [HideInInspector]
    public int evadeFlammesDifficulty = 0;

    private bool player1;

    private bool currentlyHacking = false;

    public static Hacking localInstance;

    private HackingMasterMind hackMasterMind;

    private GameObject player;

    void Awake()
    {

        hackMasterMind = hackingMasterMind.GetComponent<HackingMasterMind>();
        hackMasterMind.InitMasterMind();

    }

    void Start()
    {

        if (isLocalPlayer)
        {

            localInstance = this;

        }

        //numberButtonsText = new Text[numberButtonParent.childCount];
        
        //targetArray = new int[numberButtonsText.Length];

        /*
        for (int i = 0; i < numberButtonsText.Length; i++)
        {

            numberButtonsText[i] = numberButtonParent.transform.GetChild(i).GetChild(0).GetComponent<Text>();
            //print(nummberButtonsText[i].text);
            //targetArray[i] = Int32.Parse(numberButtonsText[i].text);

        }*/

    }

    public void StartHacking()
    {
        //print("started");

        logbookManager.openLogbook();
        playerMovement.enabled = false;
        headBob.stopSmooth = true;

        if (player1)
        {

            localInstance.StopCoroutine("Countdown");
            localInstance.StopCoroutine("PunishmentCountdown");
            localInstance.StartCoroutine("Countdown");
            localInstance.StartCoroutine("PunishmentCountdown");
            logbookManager.openLogbook();
            logbookManager.EnableOnePanel(hackingMasterMind.name);

        } else {

            for (int i = 0; i < evadeFlammesDifficulty; i++)
            {

                GameObject moveFlame = Instantiate(hackingFlame, hackingEvadeFlammes.transform);
                moveFlame.GetComponent<HackingFlame>().move = true;
                GameObject staticFlame = Instantiate(hackingFlame, hackingEvadeFlammes.transform);
                staticFlame.GetComponent<HackingFlame>().move = false;

            }

            StopCoroutine("PunishmentCountdown");
            logbookManager.EnableOnePanel(hackingEvadeFlammes.name);

        }

    }

    #region countdowns

    IEnumerator Countdown()
    {
        while(time > 0)
        {

            time--;
            timerText.text = (Mathf.FloorToInt(time / 60).ToString() + ":" + (time % 60).ToString());
            yield return new WaitForSeconds(1f);

        }

        //print("failure");
        StopHacking();

        yield return null;

    }

    IEnumerator PunishmentCountdown()
    {
        while (punishmentTime > 0)
        {

            punishmentTime--;
            yield return new WaitForSeconds(1f);

        }

        //print("failure");
        StopHacking();

        yield return null;

    }

    #endregion

    public void OnSuccess()
    {

        EventManager.instance.TriggerEvent(hackingEventName);
        StopCoroutine("Countdown");
        StopHacking();

    }

    #region addMistake
    [Command]
    public void CmdAddMistake()
    {

        RpcAddMistake();
    }

    [ClientRpc]
    void RpcAddMistake()
    {

        Hacking.localInstance.AddMistake();

    }

    public void AddMistake()
    {

        hackMasterMind.AddChoice(0, true);

    }
    #endregion

    #region loadHacking
    public void LoadHacking(string hackingName, string hackingEventName, bool player1, int evadeFlamesDif, GameObject player)
    {

        if (Hacking.hackingName == "none")
        {

            time = startTime;

        }        

        this.player1 = player1;
        evadeFlammesDifficulty = evadeFlamesDif;

        if (Hacking.hackingName == "none" && !player1)
        {

            return;

        } else if( Hacking.hackingName != "none" && player1)
        {

            return;

        }

        if (isLocalPlayer)
        {

            StartHacking();

        }

        punishmentTime = punishmentStartTime;
        currentlyHacking = true;

        Hacking.hackingName = "started";

    }
    #endregion

    #region stopHacking
    public void StopHacking()
    {

        CmdStopHacking();
        
    }

    [Command]
    public void CmdStopHacking()
    {

        RpcStopHacking();

    }

    [ClientRpc]
    void RpcStopHacking()
    {

        localInstance.OnStopHacking(true);

    }

    public void OnStopHacking(bool loss)
    {
        if (loss)
            logbookManager.DisablePanel();

        foreach (Transform child in hackingEvadeFlammes.transform)
        {

            if (child.tag == "Hacking")
            {

                Destroy(child.gameObject);
            }

        }

        if (!player1 && currentlyHacking)
        {

            CmdStartPunishmentCountdown(true);
            
        }

        if (player1)
        {

            hackingName = "none";

        }

        currentlyHacking = false;

    }
    #endregion

    [Command]
    public void CmdStartPunishmentCountdown(bool start)
    {

        RpcStartPunishmentCountdown(start);

    }

    [ClientRpc]
    void RpcStartPunishmentCountdown(bool start)
    {

        StartPunishmentCountdown(start);

    }

    public void StartPunishmentCountdown(bool start)
    {
        if (start)
        {
            localInstance.StopCoroutine("PunishmentCountdown");
            localInstance.StartCoroutine("PunishmentCountdown");

        } else
        {

            localInstance.StopCoroutine("PunishmentCountdown");

        }
       

    }
}
