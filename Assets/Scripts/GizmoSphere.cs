using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoSphere : MonoBehaviour {
	[SerializeField] Color color = Color.white;

	void OnDrawGizmos() {
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, 0.1f);
	}
}
