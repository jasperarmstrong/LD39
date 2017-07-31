using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour {
	public float maxCharge = 1;
	public float charge = 1;

	public Action OnChargeLevelChange;

	/// <summary>
	/// Charges this charge component.
	/// </summary>
	/// <param name="amount">The amount of charge to give.</param>
	/// <returns>The amount of charge not used in giving the charge. This is negative if all was taken.</returns>
	public float GiveCharge (float amount) {
		float amountOfRoom = maxCharge - charge;
		float result;

		if (amountOfRoom >= amount) {
			charge += amount;
			result = -1;
		} else {
			charge += amountOfRoom;
			result = amount - amountOfRoom;
		}

		OnChargeLevelChange?.Invoke();

		return result;
	}
	


	void Update () {
		GiveCharge(0.2f * Time.deltaTime);	
	}
}
