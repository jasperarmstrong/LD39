using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryZoneHealth : BatteryZone {
	float healthToChargeRatio = 1.5f;
	[SerializeField] float dischargeRate = 0.05f;
	
	[SerializeField] Health destination;

	[SerializeField] Color idleColor = Color.gray;
	[SerializeField] Color dischargingColor = Color.green;

	void DischargeBatteries() {
		float allowedDischargeAmount = dischargeRate * Time.deltaTime;
		bool hasGoodBattery = false;

		foreach (Charge charge in batteries) {
			if (charge.charge > 0) {
				float dischargeAmount = charge.TakeCharge(allowedDischargeAmount);

				if (Mathf.Abs(dischargeAmount - allowedDischargeAmount) < 0.1f) {
					hasGoodBattery = true;
				}
				
				if (destination.IsFull()) {
					charge.GiveCharge(dischargeAmount);
					break;
				} else {
					destination.Heal(dischargeAmount * healthToChargeRatio);
				}
			}
		}

		if (hasGoodBattery) {
			sr.color = dischargingColor;
		} else {
			sr.color = idleColor;
		}
	}

	void Update () {
		DischargeBatteries();
	}
}
