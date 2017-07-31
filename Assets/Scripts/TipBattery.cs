using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipBattery : Tip {
	Grabbable gr;
	Charge charge;

	void Start() {
		gr = GetComponent<Grabbable>();
		charge = GetComponent<Charge>();
	}

	public override string GetTip() {
		if (gr.target == null) {
			return $"Press {GameManager.interactKeyString} to Pick Up/Drop Batteries.";
		} else if (charge.ChargePercent() < 0.05f) {
			return "You should probably bring this battery to the generator at the top right of the map to charge it up.";
		} else {
			return "Bring this battery either to the heart on the EMP to bring up its health or the lightning bolt to charge it up.";
		}
	}
}
