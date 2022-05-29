using UnityEngine;
using UnityEngine.Events;

// use on trigger cube/sphere with trigger collider
// Author: Manuel Fischer

// TODO: remove
public class touchevent : MonoBehaviour {

	public string targetTag = "Player";
	public UnityEvent enterEvent;
    public UnityEvent exitEvent;
	
	private void OnTriggerEnter(Collider other)
	{
        if (other.tag.Equals(targetTag)) {
            enterEvent.Invoke();
        }
	}

    private void OnTriggerExit(Collider other) {
        if (other.tag.Equals(targetTag)) {
            exitEvent.Invoke();
        }
    }
}
