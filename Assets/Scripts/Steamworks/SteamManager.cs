using System;
using Steamworks;
using UnityEngine;

/// <summary> The Steam Manager. </summary>
public class SteamManager : MonoBehaviour {

    // only to check if there is already another steam manager
    private static SteamManager Instance;

    // The AppID of the Steam Game
    private const uint _appID = 2079230;

    private void Awake() {

        if (Instance != null) {
            Destroy(this);
            return;
        }

        try {
#if UNITY_EDITOR
            SteamClient.Init(_appID, false);
#else
            SteamClient.Init(_appID, true);
#endif
        }
        catch (Exception e) {
            Debug.Log("Couldn't Initialize Steam Client");
            Destroy(this);
            return;
        }

        //success
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

#if UNITY_EDITOR
    // steam client only shuts down, if you close the application, in editor
    // this means that the client will be still active at the start
    private void Update() {
        SteamClient.RunCallbacks();
    }
#endif

    private void OnDestroy() {
        if (Instance == this) SteamClient.Shutdown();
    }
}

