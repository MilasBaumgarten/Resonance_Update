using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class TrajectoryLineRenderer : MonoBehaviour
{
    [SerializeField] private bool drawTrajectory = false;

    [SerializeField, Range(1, 1000)] int linePoints;
    [SerializeField, Range(0.01f, 1f)] float timeBetweenPoints;
    [SerializeField] float startSpeed;
    [SerializeField] float gravityModifier;

    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] Transform startPosition;

    Vector3 tempStartPos, tempStartVelocity, lastPosition;


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(drawTrajectory){
            DrawTrajectory();
        }
    }

    public void ToggleDrawTrajectory(bool onOff){
        lineRenderer.enabled = onOff;
        drawTrajectory = onOff;
    }

    public void DrawTrajectory(){
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        tempStartPos = startPosition.position;
        tempStartVelocity = startSpeed * startPosition.forward;

        int i = 0;
        lineRenderer.SetPosition(i, tempStartPos);
        for(float time = 0f; time < linePoints; time += timeBetweenPoints){
            i++;
            Vector3 point = tempStartPos + time * tempStartVelocity;
            point.y = tempStartPos.y + tempStartVelocity.y * time + ((Physics.gravity.y * gravityModifier) / 2f * time * time);
            
            lineRenderer.SetPosition(i, point);

            lastPosition = lineRenderer.GetPosition(i - 1);
            if (Physics.Raycast(lastPosition,
                    (point - lastPosition).normalized,
                    out RaycastHit hit,
                    (point - lastPosition).sqrMagnitude))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }

    }

}
