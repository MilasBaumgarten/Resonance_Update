using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Networking;
/*
 * by Andre Spittel 04.10.2018
 * -----------------------------------------------------------------------------------------------
 * A singleton eventmanager to make it easier to use networking and the Unity eventsystem.
 * -----------------------------------------------------------------------------------------------
 * How to use this:
 * Add the script component to an empty gameobject in the scene.
 * Create a listener somewhere in the scene, with StartListening(), who wants to react to a specific event.
 * Do not forget to disable the listener (StopListening()), if no longer needed, to prevent memory leak.
 * Define an action somewhere to trigger the event with TriggerEvent().
 *
 * Code taken from official Unity Manual:
 * https://unity3d.com/de/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
 *
 * If still unclear on how to use this, see link above an look at the scripts "EventTest" (listener) and
 * "EventTriggerTest" (action).
 */

public class EventManager : NetworkBehaviour {
    
    private Dictionary<string, UnityEvent> eventDictionary;
    
    //------------------------------------------------------------------------------------------------------------------
    // Singleton format, if unclear please google Singleton

    public static EventManager instance = null;

    void Awake() {
        // Check if instance already exists
        if (instance == null) {
            // if not, set instance to this
            instance = this;
        }
        
        // If instance already exists and it's not this:
        else if (instance != this) {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        // Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
        
        Init();
    }

    void Init() {
        
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }
    
    // Singleton format end
    //------------------------------------------------------------------------------------------------------------------
    // This function is called through the singleton like: Eventmanager.instance.StartListening("ExampleEventName", ExampleFunctionName)
    // and should be called in the "Start" or "OnEnable" method.
    // It is used to decide, what function you want to call if an event is triggered.
    // Parameter:
    //             eventName = The name of the event you are waiting for to be called.
    //             listener = The name of the function you want to call, if someone called the event.

    public void StartListening(string eventName, UnityAction listener) {
        
        UnityEvent thisEvent = null;
        
        if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventName, thisEvent);
        }
    }
    
    // This function is called through the singleton like: Eventmanager.instance.StopListening("ExampleEventName", ExampleFunctionName)
    // and should be called in the "OnDisable" method.
    // This function removes Listener. The listener NEEDS to be removed or we will have memory leak.
    // Parameter:
    //             eventName = The name of the event that is registered in the list.
    //             listener = The name of the function that is registered in the list.

    public void StopListening(string eventName, UnityAction listener) {
        
        if (instance == null) 
            return;
        
        UnityEvent thisEvent = null;
        
        if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    // This function is called through the singleton like: Eventmanager.instance.TriggerEvent("ExampleEventName")
    // and should be called whenever you want to trigger an Event.
    // This function triggers an event on all Clients. (remember, the server is a client aswell)
    // Parameter:
    //             eventName = The name of the event you are calling.
    
    public void TriggerEvent(string eventName) {
        
        //need to check for isServer because clients are forbidden to use RpcCalls
        if (isServer) {
            RpcNetworkTriggerEvent(eventName);
        }
    }
    
    // RpcFunction to trigger the event on all clients. Used in TriggerEvent function.

    [ClientRpc]
    private void RpcNetworkTriggerEvent(string eventName) {
        
        UnityEvent thisEvent = null;
        
        if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke();
            
        }
    }

}