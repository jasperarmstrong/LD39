using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : UIBar {
	[SerializeField] Health health;

	void Start () {
		base.BaseStart();

		health.OnDamage += (DamageType dt) => {
			rt.localScale = new Vector3(health.health, 1, 1);
		};
	}
}
