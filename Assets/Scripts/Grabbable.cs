using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour {
	Transform target;

	IEnumerator LerpPosition() {
		while ((Vector3.zero - transform.localPosition).magnitude > 0.1f) {
			if (target == null) {
				break;
			}

			float lerpFactor = 8f * Time.deltaTime;
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, lerpFactor);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, lerpFactor);

			yield return null;
		}
		if (target != null) {
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}

	public void Grab(Transform target) {
		this.target = target;
		transform.SetParent(target);
		StartCoroutine(LerpPosition());
	}

	public void LetGo() {
		this.target = null;
		transform.SetParent(null);
	}
}
