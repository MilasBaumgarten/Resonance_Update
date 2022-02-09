using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerSource : MonoBehaviour {

    private Socket socket;
    private PowerConsumer connectedConsumer;
    public UnityEvent OnConsumerConnected;
    public UnityEvent OnConsumerDisconnected;


    // Use this for initialization
    void Start() {
        socket = GetComponent<Socket>();
        socket.OnConnect.AddListener(AddConsumer);
        socket.OnDisconnect.AddListener(RemoveConsumer);
    }

    private void AddConsumer(Socket socket) {
        PowerConsumer consumer = socket.GetComponent<PowerConsumer>();
        if (consumer) {
            connectedConsumer = consumer;
            consumer.ConnectToSource(this);
            OnConsumerConnected.Invoke();
            return;
        }
        consumer = socket.GetComponent<PowerConsumer>();
        if (consumer) {
            connectedConsumer = consumer;
            consumer.ConnectToSource(this);
            OnConsumerConnected.Invoke();
            return;
        }
    }

    private void RemoveConsumer(Socket socket) {
        PowerConsumer consumer = socket.GetComponent<PowerConsumer>();
        if (consumer) {
            connectedConsumer = null;
            consumer.DisconnectFromSource();
            OnConsumerDisconnected.Invoke();
            return;
        }
        consumer = socket.GetComponent<PowerConsumer>();
        if (consumer) {
            connectedConsumer = null;
            consumer.DisconnectFromSource();
            OnConsumerDisconnected.Invoke();
            return;
        }
    }

}
