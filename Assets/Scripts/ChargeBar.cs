using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : UIBar {
	[SerializeField] Charge charge;

	void UpdateBarScale() {
		rt.localScale = new Vector3(charge.charge / charge.maxCharge, 1, 1);
	}

	void Start () {
		base.BaseStart();

		UpdateBarScale();
		charge.OnChargeLevelChange += UpdateBarScale;
	}
}
