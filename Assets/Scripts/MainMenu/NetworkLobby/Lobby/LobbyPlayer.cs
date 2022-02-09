using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        public Button robertButton;
        public Button catButton;

        [SyncVar(hook = "OnRobertButtonColor")]
        public Color robertButtonColor = Color.white;
        [SyncVar(hook = "OnCatButtonColor")]
        public Color catButtonColor = Color.white;

        public int avatarIndex = 5;

        static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();

        public Button colorButton;
        public InputField nameInput;
        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        public GameObject networkLobby;

        public GameObject localIcone;
        public GameObject remoteIcone;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        [SyncVar(hook = "OnMyColor")]
        public Color playerColor = Color.white;

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(25.0f/255.0f, 25.0f/255.0f, 25.0f/255.0f,1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(255.0f, 255.0f, 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);

        //static Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        //static Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        // reference to client player
        private LobbyPlayer clientPlayer;
        private bool clientConnected;
        private GameObject playerList;

        void Start() {
            playerList = transform.parent.gameObject;
        }

        void Update() {

            // client connects
            if (!clientConnected && LobbyManager.s_Singleton.numPlayers > 1 && clientPlayer == null) {
                clientConnected = true;
                GameObject otherLobbyPlayer = playerList.transform.GetChild(1).gameObject;
                clientPlayer = otherLobbyPlayer.GetComponent<LobbyPlayer>();
                if (isServer && isLocalPlayer) {
                    robertButton.interactable = true;
                    catButton.interactable = true;

                    ChangeReadyButtonColor(JoinColor);
                    readyButton.transform.GetChild(0).GetComponent<Text>().text = "Select character";
                }
            }

            // client disconnects
            if(LobbyManager.s_Singleton.numPlayers == 1 && clientConnected) {
                // remove client player reference if player disconnects
                clientConnected = false;
                robertButton.interactable = false;
                catButton.interactable = false;

                readyButton.interactable = false;
                readyButton.transform.GetChild(0).GetComponent<Text>().text = "Waiting for players";
                ChangeReadyButtonColor(NotReadyColor);

                avatarIndex = 5;
                OnRobertButtonColor(Color.white);
                OnCatButtonColor(Color.white);

                CmdChangeRobertButtonColor(false);
                CmdChangeCatButtonColor(false);

            }
        }

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
            OnMyName(playerName);
            OnMyColor(playerColor);
            //Color character-selection buttons
            OnRobertButtonColor(robertButtonColor);
            OnCatButtonColor(catButtonColor);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            

            //if we return from a game, color of text can still be the one for "Ready"
            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

           SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void SetupOtherPlayer()
        {
            //Disable character selection-buttons on other player
            robertButton.interactable = false;
            catButton.interactable = false;

            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            // false until other player joins
            robertButton.interactable = false;
            catButton.interactable = false;

            robertButton.onClick.AddListener(delegate { AvatarPicker(robertButton.name); });
            catButton.onClick.AddListener(delegate { AvatarPicker(catButton.name); });

            nameInput.interactable = true;
            remoteIcone.gameObject.SetActive(false);
            localIcone.gameObject.SetActive(true);

            CheckRemoveButton();

            if (playerColor == Color.white)
                CmdColorChange();

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "Select character";

            if (!isServer) {

                robertButton.interactable = false;
                catButton.interactable = false;

                readyButton.interactable = true;
                readyButton.transform.GetChild(0).GetComponent<Text>().text = "READY?";
            }

            //have to use child count of player prefab already setup as "this.slot" is not set yet
            if (playerName == "")
                CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount-1));

            //we switch from simple name display to name input
            colorButton.interactable = true;
            nameInput.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            colorButton.onClick.RemoveAllListeners();
            colorButton.onClick.AddListener(OnColorClicked);

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }

        void AvatarPicker(string buttonName) {

            ChangeReadyButtonColor(JoinColor);
            readyButton.transform.GetChild(0).GetComponent<Text>().text = "READY?";
            // A character is selected, now the player can ready up
            readyButton.interactable = true;

            switch (buttonName) {
                case "Robert":
                    
                    clientPlayer.RpcAvatarPicked(1);
                    clientPlayer.OnRobertButtonColor(Color.white);
                    clientPlayer.OnCatButtonColor(Color.green);
                    clientPlayer.CmdChangeRobertButtonColor(false);
                    clientPlayer.CmdChangeCatButtonColor(true);

                    avatarIndex = 0;
                    OnRobertButtonColor(Color.green);
                    OnCatButtonColor(Color.white);
                    CmdChangeRobertButtonColor(true);
                    CmdChangeCatButtonColor(false);
                    break;

                case "Catriona":

                    clientPlayer.RpcAvatarPicked(0);
                    clientPlayer.OnRobertButtonColor(Color.green);
                    clientPlayer.OnCatButtonColor(Color.white);
                    clientPlayer.CmdChangeRobertButtonColor(true);
                    clientPlayer.CmdChangeCatButtonColor(false);

                    avatarIndex = 1;
                    OnRobertButtonColor(Color.white);
                    OnCatButtonColor(Color.green);
                    CmdChangeRobertButtonColor(false);
                    CmdChangeCatButtonColor(true);
                    break;

                default:
                    avatarIndex = 0;
                    break;
            }

            if (isServer)
                RpcAvatarPicked(avatarIndex);
            else
                CmdAvatarPicked(avatarIndex);

        }

        [ClientRpc]
        void RpcAvatarPicked(int avatarIndex) {
            CmdAvatarPicked(avatarIndex);
        }

        [Command]
        void CmdAvatarPicked(int avatarIndex) {
            LobbyManager.s_Singleton.SetPlayerTypeLobby(GetComponent<NetworkIdentity>().connectionToClient, avatarIndex);
            /*
            if(avatarIndex == 0) {
                robertButton.GetComponent<Image>().color = Color.green;
                catButton.GetComponent<Image>().color = Color.white;
            } else if(avatarIndex == 1) {
                catButton.GetComponent<Image>().color = Color.green;
                robertButton.GetComponent<Image>().color = Color.white;
            }
            */
        }

        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;
        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                ChangeReadyButtonColor(TransparentColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY!";
                textComponent.color = ReadyColor;
                readyButton.interactable = false;
                colorButton.interactable = false;
                nameInput.interactable = false;
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "READY?" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
                colorButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx)
        { 
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var

        public void OnMyName(string newName)
        {
            playerName = newName;
            nameInput.text = playerName;
        }

        public void OnMyColor(Color newColor)
        {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;
        }

        public void OnRobertButtonColor(Color newColor) {
            robertButtonColor = newColor;
            robertButton.GetComponent<Image>().color = newColor;
        }

        public void OnCatButtonColor(Color newColor) {
            catButtonColor = newColor;
            catButton.GetComponent<Image>().color = newColor;
        }

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnColorClicked()
        {
            CmdColorChange();
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick()
        {
            if (isLocalPlayer)
            {
                RemovePlayer();
            }
            else if (isServer)
                LobbyManager.s_Singleton.KickPlayer(connectionToClient);
                
        }

        public void ToggleJoinButton(bool enabled)
        {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            if(countdown == 0)
            {
                DontDestroyOnLoad(LobbyManager.s_Singleton.gameObject);
                LobbyManager.s_Singleton.contentUI.SetActive(false);
            }
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton()
        {
            CheckRemoveButton();
        }

        //====== Server Command

        [Command]
        public void CmdColorChange()
        {
            int idx = System.Array.IndexOf(Colors, playerColor);

            int inUseIdx = _colorInUse.IndexOf(idx);

            if (idx < 0) idx = 0;

            idx = (idx + 1) % Colors.Length;

            bool alreadyInUse = false;

            do
            {
                alreadyInUse = false;
                for (int i = 0; i < _colorInUse.Count; ++i)
                {
                    if (_colorInUse[i] == idx)
                    {//that color is already in use
                        alreadyInUse = true;
                        idx = (idx + 1) % Colors.Length;
                    }
                }
            }
            while (alreadyInUse);

            if (inUseIdx >= 0)
            {//if we already add an entry in the colorTabs, we change it
                _colorInUse[inUseIdx] = idx;
            }
            else
            {//else we add it
                _colorInUse.Add(idx);
            }

            playerColor = Colors[idx];
        }

        [Command]
        public void CmdChangeRobertButtonColor(bool selected) {
            if (selected)
                robertButtonColor = Color.green;
            else
                robertButtonColor = Color.white;
        }

        [Command]
        public void CmdChangeCatButtonColor(bool selected) {
            if (selected)
                catButtonColor = Color.green;
            else
                catButtonColor = Color.white;
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

            int idx = System.Array.IndexOf(Colors, playerColor);

            if (idx < 0)
                return;

            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    _colorInUse.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
