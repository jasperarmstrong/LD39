using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarCharge : UIBar {
	[SerializeField] Charge charge;

	void Start () {
		base.BaseStart();

		charge.OnChargeLevelChange += () => {
			rt.localScale = new Vector3(charge.ChargePercent(), 1, 1);
		};
	}
}
