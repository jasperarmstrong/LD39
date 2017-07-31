using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
	Transform target;
	float lerpAmount = 5;
	[SerializeField] Vector3 offset;
	[SerializeField] float targetSize;

	Camera cam;

	void Start() {
		cam = GetComponent<Camera>();
	}

	void LateUpdate () {
		if (GameManager.isPaused) {
			return;
		}

		if (target == null) {
			target = GameObject.FindGameObjectWithTag("Player")?.transform;
		} else {
			float lerpFactor = lerpAmount * Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpFactor);
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, lerpFactor);
		}
	}
}
