// Author: Noah Stolz
// Used to rotate the camera towards a specified transform
// Has to be attached to the player camera
using System.Collections;
using UnityEngine;

public class RotateCameraToVector : MonoBehaviour {

    [SerializeField]
    [Tooltip("The transfrom the cam is rotated to")]
    private Transform targetTransform;

    [SerializeField]
    [Tooltip("How fast the cam should rotate")]
    private float speed;

    [SerializeField]
    [Tooltip("How long the rotation should last")]
    private int duration;

    private Transform transformFrom;
    private Quaternion lookRotation;
    private Quaternion startRotation;
    private Vector3 direction;

    void Start()
    {

        speed /= duration;

    }

    public void RotateCam(bool back) {

        transformFrom = transform;

        if (!back)
        {

            //find the vector pointing to the target
            direction = (targetTransform.position - transformFrom.position).normalized;

            //create the rotation
            lookRotation = Quaternion.LookRotation(direction);

            startRotation = transform.rotation;

            StartCoroutine("RotateToward");

        } else
        {

            direction = (targetTransform.position - transformFrom.position).normalized;

            lookRotation = startRotation;

            StartCoroutine("RotateToward");

        }

    }

    IEnumerator RotateToward() {

        for (int i = 0; i < duration; i++) {

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

            yield return null;

        }
    }
}
