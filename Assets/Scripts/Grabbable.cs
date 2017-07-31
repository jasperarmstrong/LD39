using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour {
	[SerializeField] LayerMask chargeZoneLayerMask;
	public Transform target;

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
		transform.gameObject.layer = LayerMask.NameToLayer("GrabbedItem");
		StartCoroutine(LerpPosition());

		Charge c = GetComponent<Charge>();
		if (c != null) {
			Collider2D hit = Physics2D.OverlapCircle(transform.position, GetComponent<Collider2D>()?.bounds.extents.x ?? 0.2f, chargeZoneLayerMask);
			if (hit) {
				hit.transform.GetComponent<BatteryZone>()?.RemoveBattery(c);
			}
		}
	}

	public void LetGo() {
		this.target = null;
		transform.SetParent(null);
		transform.gameObject.layer = LayerMask.NameToLayer("Default");

		Charge c = GetComponent<Charge>();
		if (c != null) {
			Collider2D hit = Physics2D.OverlapCircle(transform.position, GetComponent<Collider2D>()?.bounds.extents.x ?? 0.2f, chargeZoneLayerMask);
			if (hit) {
				hit.transform.GetComponent<BatteryZone>()?.AddBattery(c);
			}
		}
	}
}
