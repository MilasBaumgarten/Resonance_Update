using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerConsumer : MonoBehaviour {

    private PowerSource source;
    public UnityEvent OnConnectToSource;
    public UnityEvent OnDisconnectFromSource;

    public void ConnectToSource(PowerSource source) {
        OnConnectToSource.Invoke();
    }

    public void DisconnectFromSource() {
        OnDisconnectFromSource.Invoke();
    }
}
