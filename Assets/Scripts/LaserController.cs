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
		if (hasHit) {
			onHit?.Invoke();
			Destroy(gameObject);
		}

		Vector3 moveVector = speed * Time.deltaTime * hitSpot.up;

		RaycastHit2D hit = Physics2D.Raycast(hitSpot.position, hitSpot.up, moveVector.magnitude);
		if (hit && !hit.collider.isTrigger) {
			hasHit = true;
			transform.position += hit.distance * moveVector.normalized;
			onHit = () => {
				hit.transform.GetComponent<Health>()?.Damage(DamageType.LASER);
			};
		} else {
			transform.position += moveVector;
		}
	}
}
