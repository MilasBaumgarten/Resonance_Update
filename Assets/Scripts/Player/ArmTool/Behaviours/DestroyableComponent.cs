using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyableComponent : Grabable {

    [SerializeField]
    private float breakDistance = 3f;
    private bool isDetached = false;
    public UnityEvent OnComponentBreak;

    protected override void Start() {
        base.Start();
        rb.isKinematic = true;

    }

    protected override void ApplyForce(Rigidbody rb, Vector3 dist, float movePower, int targetCount) {
        if (!isDetached) {
            if (dist.magnitude > breakDistance) {
                isDetached = true;
                rb.isKinematic = false;
                OnComponentBreak.Invoke();
            }
        }
        if (isDetached) {
            rb.AddForce(dist.normalized * movePower * targetCount, ForceMode.Force);
        }
    }
}
