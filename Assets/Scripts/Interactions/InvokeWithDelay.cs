// Author: Noah Stolz
// Used to continue only after an interaction has happened

using UnityEngine;
using UnityEngine.Events;

public class InvokeWithDelay : MonoBehaviour {

    [SerializeField]
    [Tooltip("How long before the next event is called, in seconds")]
    private float delay;

    public UnityEvent nextEvent;

    public void OnButtonClicked()
    {

        Invoke("NextEvent", delay);

    }

    void NextEvent()
    {

        nextEvent.Invoke();

    }

}
