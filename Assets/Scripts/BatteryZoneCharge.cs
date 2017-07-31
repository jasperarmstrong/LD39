using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryZoneCharge : BatteryZone {
	[SerializeField] float chargeRate = 0.1f;
	
	[SerializeField] Charge source;

	[SerializeField] Color idleColor = Color.gray;
	[SerializeField] Color dischargingColor = Color.green;

	void ChargeBatteries() {
		float allowedChargeAmount = chargeRate * Time.deltaTime;
		bool hasGoodBattery = false;

		float chargeAmount = source.TakeCharge(allowedChargeAmount);

		foreach (Charge charge in batteries) {
			if (charge.charge < charge.maxCharge) {
				float amountCharged = charge.GiveCharge(chargeAmount);
				
				if (amountCharged < 0) {
					hasGoodBattery = true;
					break;
				} else {
					chargeAmount -= amountCharged;
				}
			}
		}

		if (hasGoodBattery) {
			sr.color = dischargingColor;
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
		} else {
			sr.color = idleColor;
			if (audioSource.isPlaying) {
				audioSource.Stop();
			}			
		}
	}

	void Update () {
		if (GameManager.isPaused) {
			return;
		}
		ChargeBatteries();
	}
}
