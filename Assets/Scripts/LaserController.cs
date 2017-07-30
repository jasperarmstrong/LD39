using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
	[SerializeField]Transform hitSpot;
	
	float speed = 16;

	bool hasHit = false;

	void Update () {
		if (hasHit) {
			Destroy(gameObject);
		}

		Vector3 moveVector = speed * Time.deltaTime * hitSpot.up;

		RaycastHit2D hit = Physics2D.Raycast(hitSpot.position, hitSpot.up, moveVector.magnitude);
		if (hit) {
			hasHit = true;
			transform.position += hit.distance * moveVector.normalized;
		} else {
			transform.position += moveVector;
		}
	}
}
