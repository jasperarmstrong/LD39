using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryZone : MonoBehaviour {
	protected List<Charge> batteries;
	[SerializeField] protected SpriteRenderer sr;

	void Start() {
		batteries = new List<Charge>();
	}

	void Update () {
		foreach (Charge c in batteries) {
			Debug.Log(c);
		}
	}

	public void AddBattery(Charge charge) {
		if (charge != null && !batteries.Contains(charge)) {
			batteries.Add(charge);
		}
	}

	public void RemoveBattery(Charge charge) {
		if (charge != null && batteries.Contains(charge)) {
			batteries.Remove(charge);
		}
	}
}
