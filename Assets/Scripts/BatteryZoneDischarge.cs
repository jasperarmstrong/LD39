using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryZoneDischarge : BatteryZone {
	[SerializeField] float dischargeRate = 0.05f;
	
	[SerializeField] Charge destination;

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
				
				float amountDischarged = destination.GiveCharge(dischargeAmount);
				allowedDischargeAmount -= amountDischarged;
				if (amountDischarged < 0) {
					break;
				} else {
					charge.GiveCharge(amountDischarged);
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
