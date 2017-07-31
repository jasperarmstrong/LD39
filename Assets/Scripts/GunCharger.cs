using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCharger : MonoBehaviour {
	[SerializeField] float chargeRate = 0.25f;
	bool shouldChargeGun = false;

	[SerializeField] Color idleColor = Color.gray;
	[SerializeField] Color chargingColor = Color.green;

	[SerializeField] SpriteRenderer sr;

	public void Enter() {
		shouldChargeGun = true;
	}

	public void Exit() {
		shouldChargeGun = false;
	}

	void Update() {
		if (shouldChargeGun) {
			Charge charge = GameManager.pc?.GetComponent<Charge>();
			if (charge?.charge < charge?.maxCharge) {
				charge?.GiveCharge(chargeRate * Time.deltaTime);
				sr.color = chargingColor;
			} else {
				sr.color = idleColor;	
			}
		} else {
			sr.color = idleColor;
		}
	}
}
