using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {
	PlayerController pc;
	List<Transform> collidingObjects;

	void Start() {
		pc = GetComponentInParent<PlayerController>();
		collidingObjects = new List<Transform>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
			foreach (Transform t in collidingObjects) {
				Debug.Log(t.name);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.transform.GetComponent<Grabbable>() != null && !collidingObjects.Contains(col.transform)) {
			collidingObjects.Add(col.transform);
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.transform.GetComponent<Grabbable>() != null && collidingObjects.Contains(col.transform)) {
			collidingObjects.Remove(col.transform);
		}
	}
}
