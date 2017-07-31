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

	void UpdateTip(Transform t) {
		if (t == null) {
			GameManager.GiveTip("");
			return;
		}
		Tip tip = t.GetComponent<Tip>();
		if (tip != null) {
			GameManager.GiveTip(tip.GetTip());
		}
	}

	void Update() {
		if (GameManager.isPaused || GameManager.isGameOver || GameManager.isGameWon) {
			return;
		}
		Transform target = ClosestInteractableObjectInRange();
		if (closestTarget != target) {
			closestTarget = target;
			UpdateTip(closestTarget);
		}	
		if (currentInteraction != null) {
			UpdateTip(currentInteraction);
		}

		if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)) {
			Interactable interactable = closestTarget?.GetComponent<Interactable>();
			if (currentInteraction != null) {
				Grabbable gr = currentInteraction?.GetComponent<Grabbable>();
				if (gr != null) {
					LetGo(gr);
				}
			} else if (interactable != null) {
				interactable.Interact();
			} else {
				Grabbable gr = closestTarget?.GetComponent<Grabbable>();
				if (gr != null) {
					Grab(gr);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (
			(
				col.transform.GetComponent<Grabbable>() != null || 
				col.transform.GetComponent<Interactable>() != null ||
				col.transform.GetComponent<Tip>() != null
			) && !collidingObjects.Contains(col.transform)
		) {
			collidingObjects.Add(col.transform);
		}
		col.transform.GetComponent<GunCharger>()?.Enter();
		col.transform.GetComponent<HealthRegen>()?.Enter();
	}

	void OnTriggerExit2D(Collider2D col) {
		if (
			(
				col.transform.GetComponent<Grabbable>() != null || 
				col.transform.GetComponent<Interactable>() != null ||
				col.transform.GetComponent<Tip>() != null
			) && collidingObjects.Contains(col.transform)
		) {
			collidingObjects.Remove(col.transform);
		}
		col.transform.GetComponent<GunCharger>()?.Exit();
		col.transform.GetComponent<HealthRegen>()?.Exit();
	}
}
