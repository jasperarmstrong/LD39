using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	[SerializeField] private float moveSpeed = 8;
	[SerializeField] LayerMask moveLayerMask;

	public bool canMove = true;

	Rigidbody2D rb;
	Collider2D col;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
	}

	float GetDistanceToEdge() {
		if (col.GetType() == typeof(CircleCollider2D)) {
			return ((CircleCollider2D)col).radius;
		} else if (col.GetType() == typeof(BoxCollider2D)) {
			return ((BoxCollider2D)col).bounds.extents.y;
		}
		Debug.LogError($"Need to implement Movement.GetDistanceToEdge for {col.GetType()}");
		return 0;
	}

	public void Move(Vector2 vec, Space relativeTo = Space.World) {
		Move(vec.x, vec.y, relativeTo);
	}

	public void Move(float horizontal, float vertical, Space relativeTo = Space.World, float multiplier = 1) {
		if (
			!canMove ||
			(Mathf.Abs(horizontal) < 0.01f && Mathf.Abs(vertical) < 0.01f)
		) {
			return;
		}

		rb.velocity = Vector3.zero;

		float moveFactor = moveSpeed * Time.deltaTime;
		Vector2 moveVector = new Vector2(horizontal, vertical).normalized * moveFactor * multiplier;
		if (relativeTo == Space.Self) {	
			moveVector = transform.TransformDirection(moveVector);
		}

		RaycastHit2D[] hits = Physics2D.CircleCastAll(
			transform.position,
			GetDistanceToEdge(),
			moveVector.normalized,
			moveVector.magnitude,
			moveLayerMask
		);

		foreach(RaycastHit2D hit in hits) {
			if (hit.transform == transform) {
				continue;
			}

			if (Vector2.Dot(hit.point - (Vector2)transform.position, moveVector.normalized) > 0) {
				moveVector += hit.normal * moveFactor * 0.65f;
			}
		}
		transform.position += (Vector3)moveVector;
	}
}
