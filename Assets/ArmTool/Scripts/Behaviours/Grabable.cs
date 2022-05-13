// Author: Felix Werner
// 
// Add this script to an object to make it grabable by the force tool.
// 
// Changes: Noah Stolz
// Stopped player from moving and clamped mouse movement when grabbing

using System.Collections.Generic;
using UnityEngine;
using Logic.Triggers;

[RequireComponent(typeof(Rigidbody), typeof(ArmToolInteractionTrigger))]
public class Grabable : ArmToolModuleBehaviour {
	protected List<Transform> targetPositions;    // a list of all players' hold positions
	protected List<LineRenderer> beamRenderers;
	protected Rigidbody rb;
	protected MeshRenderer meshRenderer;
	[SerializeField]
	private Material forceMaterial;
	[Tooltip("Magnitude of how much Force is used to move the object (more = faster) currently gets calculated in Start().")]
	protected float movePower = 1000.0f;    // calculated by mass, could also be a fixed value
	[SerializeField]
	protected bool requiresBothPlayers = false;
	protected ArmToolInteractionTrigger trigger;

	protected virtual void Start() {
		// initialize variables
		targetPositions = new List<Transform>();
		beamRenderers = new List<LineRenderer>();
		meshRenderer = GetComponent<MeshRenderer>();
		rb = GetComponent<Rigidbody>();
		/*movePower = 1000 / rb.mass;*/ // placeholder calculation for force applied
		trigger = GetComponent<ArmToolInteractionTrigger>();
	}

	private void FixedUpdate() {
		if (targetPositions.Count > (requiresBothPlayers ? 1 : 0)) {
			// calculate the middle of both player's holding positions
			Vector3 dist = Vector3.zero;
			foreach (Transform trans in targetPositions) {
				dist += trans.position - this.transform.position;
			}

			dist /= targetPositions.Count;

			// adjust drag of this object and push it towards the holding position
			rb.drag = 50 / (1 + dist.magnitude);
			ApplyForce(rb, dist, movePower, targetPositions.Count);

			UpdateBeams(true);
		} else {
			UpdateBeams(false);
		}
	}

	private void UpdateBeams(bool canLift) {
		foreach (LineRenderer beamRenderer in beamRenderers) {
			beamRenderer.SetPosition(1, beamRenderer.transform.InverseTransformPoint(transform.position));
			if (canLift) {
				beamRenderer.material.SetColor("_BeamColor", Color.blue);
			} else {
				beamRenderer.material.SetColor("_BeamColor", Color.red);
			}
		}
	}

	// adds another player's holding position to the list
	private void AddGrab(Transform pos) {
		// disable gravity while a player is holding the object
		if (targetPositions.Count == 0) {
			rb.useGravity = false;
		}

		targetPositions.Add(pos);
	}

	// removes a Player's holding position from the list
	private void RemoveGrab(Transform pos) {
		targetPositions.Remove(pos);

		// reenable gravity while noone is holding the object, reset the drag and stop it's movement
		if (targetPositions.Count <= (requiresBothPlayers ? 1 : 0)) {
			rb.useGravity = true;
			rb.drag = 0.1f;
			rb.velocity = Vector3.zero;
		}
	}

	// overridden Interact Method inherited from ForceToolBehaviour
	public override void Interact(ArmToolModule module) {
		ForceModule forceModule = module as ForceModule;
		LineRenderer beamRenderer = forceModule.GetBeamRenderer();
		if (forceModule.GetGrabStatus()) {
			RemoveGrab(forceModule.GetHoldPosition());
			beamRenderers.Remove(beamRenderer);
			beamRenderer.enabled = false;

			meshRenderer.materials = new Material[1] { meshRenderer.materials[0] };

			trigger.Interact();
		} else {
			AddGrab(forceModule.GetHoldPosition());
			beamRenderers.Add(beamRenderer);
			beamRenderer.enabled = true;

			meshRenderer.materials = new Material[2] { meshRenderer.materials[0], forceMaterial };

			if (targetPositions.Count > (requiresBothPlayers ? 1 : 0)) {
				meshRenderer.materials[1].SetColor("_Color", Color.blue);
			} else {
				meshRenderer.materials[1].SetColor("_Color", Color.red);
			}
			trigger.Interact();
		}
		forceModule.ToggleGrab();
	}

	protected virtual void ApplyForce(Rigidbody rb, Vector3 dist, float movePower, int targetCount) {
		rb.AddForce(dist.normalized * movePower * targetCount, ForceMode.Force);
	}
}
