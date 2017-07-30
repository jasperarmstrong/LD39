using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
	[SerializeField] Transform target;
	float lerpAmount = 5;
	[SerializeField] Vector3 offset;

	void LateUpdate () {
		if (target != null) {
			transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpAmount * Time.deltaTime);
		}
	}
}
