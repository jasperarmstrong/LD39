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
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 8f * Time.deltaTime);
			yield return null;
		}
		if (target != null) {
			transform.position = Vector3.zero;
		}
	}

	public void Grab(Transform target) {
		this.target = target;
		transform.SetParent(target);
		StartCoroutine(LerpPosition());
	}

	public void LetGo() {
		this.target = null;
	}
}
