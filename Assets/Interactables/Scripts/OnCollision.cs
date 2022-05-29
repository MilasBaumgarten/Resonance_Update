// Author: Noah Stolz
// Used to Call an event when a collision is detected and the condition is set to true
// Should be attached to the Object that has the collider that needs to be hit to trigger the event

using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour {

    [Tooltip("Used to check if the neccesary condition has been met")]
    public bool condition = false;

    public UnityEvent onCollision;

    void OnCollisionEnter(Collision collision)
    {

        if (condition)
        {

            onCollision.Invoke();

        }

    }

    public void setCondition(bool condition)
    {

        this.condition = condition;

    }
}
