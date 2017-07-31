using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {
	PlayerController pc;

	List<Transform> collidingObjects;
	Transform closestTarget;
	public Transform currentInteraction;

	bool canInteract = true;

	public Action<Grabbable> OnGrab;
	public Action<Grabbable> OnLetGo;

	void Start() {
		pc = GetComponentInParent<PlayerController>();
		collidingObjects = new List<Transform>();
	}

	Transform ClosestInteractableObjectInRange() {
		if (collidingObjects.Count > 0) {
			return collidingObjects.OrderBy(t => Vector3.Distance(t.position, transform.position)).ToArray()[0].transform;
		}
		return null;
	}

	void Grab(Grabbable gr) {
		if (gr == null) {
			return;
		}
		gr.Grab(pc.itemHolder);
		currentInteraction = closestTarget;
		OnGrab?.Invoke(gr);
	}

	void LetGo(Grabbable gr) {
		if (gr == null) {
			return;
		}
		gr.LetGo();
		currentInteraction = null;
		OnLetGo?.Invoke(gr);
	}

	public void LetGo() {
		if (currentInteraction != null) {
			LetGo(currentInteraction.GetComponent<Grabbable>());
		}
	}

	void Update() {
		Transform target = ClosestInteractableObjectInRange();
		if (closestTarget == target) {
			// trigger interaction ui if not already done
		} else {
			// disable the ui for the old closest target
			closestTarget = target;
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			if (currentInteraction == null) {
				Grabbable gr = closestTarget?.GetComponent<Grabbable>();
				if (gr != null) {
					Grab(gr);
				}
			} else {
				Grabbable gr = currentInteraction?.GetComponent<Grabbable>();
				if (gr != null) {
					LetGo(gr);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.transform.GetComponent<Grabbable>() != null && !collidingObjects.Contains(col.transform)) {
			collidingObjects.Add(col.transform);
		}
		col.transform.GetComponent<GunCharger>()?.Enter();
		col.transform.GetComponent<HealthRegen>()?.Enter();
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.transform.GetComponent<Grabbable>() != null && collidingObjects.Contains(col.transform)) {
			collidingObjects.Remove(col.transform);
		}
		col.transform.GetComponent<GunCharger>()?.Exit();
		col.transform.GetComponent<HealthRegen>()?.Exit();
	}
}
