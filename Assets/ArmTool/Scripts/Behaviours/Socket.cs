// Author: Feix Werner

using UnityEngine;
using UnityEngine.Events;

public class Socket : EnergyModuleBehaviour {

    public SocketConnectionEvent OnConnect;
    public SocketConnectionEvent OnDisconnect;
    public UnityEvent OnSelected;
    public UnityEvent OnDeselect;
    public Transform attachPosition;
    [SerializeField]
    private Socket adjacentSocket;
    private Socket connectedSocket;
    private EnergyModule inUseBy;

    private void Start() {
        if (adjacentSocket) {
            adjacentSocket.OnConnect.AddListener(OnSocketConnect);
            adjacentSocket.OnDisconnect.AddListener(OnSocketDisconnect);
        }
    }

    public override void Interact(ArmToolModule module) {
		//Socket targetSocket = target.GetComponent<Socket>();
		//Socket connectedSocket = targetSocket.GetConnectedSocket();
		//if (connectedSocket) {
		//	if (targetSocket.GetInUseBy() != this) {
		//		return;
		//	}
		//	connectedSocket.Disconnect();
		//	targetSocket.Disconnect();
		//	Destroy(cable);
		//	cable = null;
		//	return;
		//}
		//if (!cable) {
		//	if (start) {
		//		if (targetSocket == start) {
		//			targetSocket.Deselect();
		//			start = null;
		//			return;
		//		}
		//		if ((targetSocket.transform.position - start.transform.position).magnitude > maxDist) {
		//			return;
		//		}
		//		start.Connect(targetSocket, this);
		//		targetSocket.Connect(start, this);
		//		cable = Instantiate(cablePrefab, start.attachPosition.position + (targetSocket.attachPosition.position - start.attachPosition.position) / 2, Quaternion.identity);
		//		LineRenderer lr = cable.GetComponent<LineRenderer>();
		//		lr.SetPosition(0, targetSocket.attachPosition.position);
		//		lr.SetPosition(1, start.attachPosition.position);
		//		start = null;
		//	} else {
		//		targetSocket.Select(this);
		//		start = targetSocket;
		//	}
		//}
	}

    public void Select(EnergyModule module) {
        inUseBy = module;
        // Start playing some effect to visualize the selection
        OnSelected.Invoke();
    }

    public void Deselect() {
        inUseBy = null;
        // Stop playing the selection effect
        OnDeselect.Invoke();
    }

    public void Connect(Socket other, EnergyModule module) {
        connectedSocket = other;
        inUseBy = module;
        OnConnect.Invoke(connectedSocket);
    }

    public void Disconnect() {
        OnDisconnect.Invoke(connectedSocket);
        connectedSocket = null;
    }

    private void OnSocketConnect(Socket socket) {
        if (connectedSocket) {
            connectedSocket.OnConnect.Invoke(socket);
        }
    }

    private void OnSocketDisconnect(Socket socket) {
        if (connectedSocket) {
            connectedSocket.OnDisconnect.Invoke(socket);
        }
    }

    public EnergyModule GetInUseBy() {
        return inUseBy;
    }

    public Socket GetConnectedSocket() {
        return connectedSocket;
    }
}

[System.Serializable]
public class SocketConnectionEvent : UnityEvent<Socket> { }
