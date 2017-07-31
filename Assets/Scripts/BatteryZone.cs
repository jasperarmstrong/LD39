using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryZone : MonoBehaviour {
	protected List<Charge> batteries;
	[SerializeField] protected SpriteRenderer sr;

	protected AudioSource audioSource;

	void Start() {
		batteries = new List<Charge>();
		audioSource = GetComponent<AudioSource>();
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
