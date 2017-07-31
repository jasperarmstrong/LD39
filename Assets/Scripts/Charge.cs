using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour {
	public bool isInfinite = false;
	
	public float maxCharge = 1;
	public float charge = 1;

	public Action OnChargeLevelChange;

	public float ChargePercent() {
		return charge / maxCharge;
	}

	/// <summary>
	/// Charges this charge component.
	/// </summary>
	/// <param name="amount">The amount of charge you want to give.</param>
	/// <returns>The amount of charge not able to be given. This is negative if all was taken.</returns>
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

	/// <summary>
	/// Takes energy from this charge component.
	/// </summary>
	/// <param name="amount">The amount of charge you want to take.</param>
	/// <returns>The amount of charge able to be taken. This is the the amount passed in the arguments if all of it was available.</returns>
	public float TakeCharge(float amount) {
		if (isInfinite) {
			return amount;
		}

		if (amount <= charge) {
			charge -= amount;
		} else {
			charge -= charge;
			amount -= charge;
		}

		OnChargeLevelChange?.Invoke();

		return amount;
	}
}
