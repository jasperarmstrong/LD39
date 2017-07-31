using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
	[SerializeField]Transform hitSpot;
	
	float speed = 16;

	bool hasHit = false;
	Action onHit;

	void Update () {
		if (GameManager.isPaused) {
			return;
		}

		if (hasHit) {
			onHit?.Invoke();
			Destroy(gameObject);
		}

		Vector3 moveVector = speed * Time.deltaTime * hitSpot.up;

		RaycastHit2D[] hits = Physics2D.RaycastAll(hitSpot.position, hitSpot.up, moveVector.magnitude);
		foreach(RaycastHit2D hit in hits) {
			if (hit && !hit.collider.isTrigger) {
				hasHit = true;
				transform.position += hit.distance * moveVector.normalized;
				onHit = () => {
					hit.transform?.GetComponent<Health>()?.Damage(DamageType.LASER);
				};
			}
		}
		if (!hasHit) {
			transform.position += moveVector;
		}
	}
}
